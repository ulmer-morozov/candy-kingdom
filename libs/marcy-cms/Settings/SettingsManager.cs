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

    public Task<ResultOrError> UpdateAsync(IEnumerable<Setting> settings, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultOrError<ImmutableList2<SettingGroup>>> GetGroupsAsync(IEnumerable<Guid>? ids = null, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var groupDbs = await context.SettingGroups
            .Include(x => x.Records)
            .ToListAsync(cancellationToken);

        var groups = groupDbs.Select(ToDto).ToImmutableList2();

        return ResultOrError.Success(groups);
    }

    private SettingGroup ToDto(SettingGroupDb groupDb)
    {
        return new SettingGroup
        {
            Id = groupDb.Id,
            Title = groupDb.Title,
            Records = groupDb.Records.Select(ToDto).ToImmutableList2()
        };
    }

    private Setting ToDto(SettingDb db)
    {
        var setting = Setting.NewFromData(db.Data);

        setting = setting with
        {
            Id = db.Id,
            Title = db.Title
        };

        return setting;
    }

    public Task<ResultOrError> StoreAsync(SettingGroup group, bool deep = false, bool @override = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
