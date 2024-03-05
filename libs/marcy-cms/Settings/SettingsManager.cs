using System.Collections.Immutable;

using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.MarcyCms.Data;

using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms.Settings;

public sealed class SettingsManager<TDbContext> : ISettingsManager
    where TDbContext : DbContext, IMarcyCmsDbContext
{
    private readonly IDbContextFactory<TDbContext> _contextFactory;

    public SettingsManager(IDbContextFactory<TDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<ResultOrError<ImmutableList2<Setting>>> GetSettingsAsync(IEnumerable<Guid> settingIds, CancellationToken cancellationToken = default)
    {
        var settingIdList = settingIds.ToImmutableList();

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var settingDbDict = await context.SettingRecords
                .Where(x => settingIdList.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (var settingId in settingIdList)
        {
            if (!settingDbDict.TryGetValue(settingId, out var settingDb))
            {
                return ResultOrError.Fail<ImmutableList2<Setting>>($"Setting with Id {settingId} is not found", (int)CRUDErrorCode.NotFound);
            }
        }

        var dtos = settingDbDict.Values.Select(ToDto).ToImmutableList2();

        return ResultOrError.Success(dtos);
    }

    public async Task<ResultOrError> UpdateAsync(IEnumerable<Setting> settings, CancellationToken cancellationToken = default)
    {
        var newSettingDict = settings.ToImmutableDictionary(x => x.Id);

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var settingDbDict = await context.SettingRecords
                .Where(x => newSettingDict.Keys.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (var newSetting in newSettingDict.Values)
        {
            if (!settingDbDict.TryGetValue(newSetting.Id, out var settingDb))
            {
                return ResultOrError.Fail($"Setting with Id {newSetting.Id} is not found", (int)CRUDErrorCode.NotFound);
            }

            if (newSetting is not ISetting<SettingData> newSettingWithData)
            {
                // nothing to copy
                continue;
            }

            if (settingDb.Data.Type != newSettingWithData.Data.Type)
            {
                return ResultOrError.Fail($"Couldn't update setting data because it's Id in Db has type {settingDb.Data.Type} but updated setting has type {newSettingWithData.Data.Type}", (int)CRUDErrorCode.NotFound);
            }

            settingDb.SetData(newSettingWithData.Data);
        }

        await context.SaveChangesAsync(CancellationToken.None);

        return ResultOrError.Success();
    }

    public async Task<ResultOrError<ImmutableList2<SettingGroup>>> GetGroupsAsync(IEnumerable<Guid>? ids = null, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var groupDbs = await context.SettingGroups
            .Include(x => x.Records)
            .ToListAsync(cancellationToken);

        var groups = groupDbs
            .Select(ToDto)
            .OrderBy(x => x.Order)
            .ToImmutableList2();

        return ResultOrError.Success(groups);
    }

    private SettingGroup ToDto(SettingGroupDb groupDb)
    {
        return new SettingGroup
        {
            Id = groupDb.Id,
            Title = groupDb.Title,
            Records = groupDb.Records
                .Select(ToDto)
                .OrderBy(x => x.Order)
                .ToImmutableList2()
        };
    }

    private Setting ToDto(SettingDb db)
    {
        var setting = Setting.NewFromData(db.Data);

        setting = setting with
        {
            Id = db.Id,
            Order = db.Order,
            Title = db.Title
        };

        return setting;
    }
}
