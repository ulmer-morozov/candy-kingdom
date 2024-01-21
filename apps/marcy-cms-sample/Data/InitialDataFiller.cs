using Autofac;

using CandyKingdom.Marcy.Immutables;
using CandyKingdom.MarcyCms.Data;
using CandyKingdom.MarcyCms.Sample.Content;
using CandyKingdom.MarcyCms.Settings;

using Microsoft.EntityFrameworkCore;

using static CandyKingdom.Marcy.LocalizedStringHelpers;

namespace CandyKingdom.MarcyCms.Sample.Data;

public sealed class InitialDataFiller
{
    private readonly IDbContextFactory<CmsSampleDbContext> _contextFactory;

    public InitialDataFiller(IDbContextFactory<CmsSampleDbContext> dbContextFactory)
    {
        _contextFactory = dbContextFactory;
    }

    public async Task InitializeIfNecessary()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();

        await CreateIfNotExist
        (
            context,
            new SettingGroup
            {
                Id = CmsSampleSettingGroupIds.Main,
                Title = "Main",
                Records = new List<Setting>
                {
                        new Setting<LocalizedTextSettingData>
                        {
                            Id = CmsSampleSettingIds.Company,
                            Title = "Company",
                            Data = new LocalizedTextSettingData
                            {
                                Text = En("English Company Title"),
                                TextType = TextSettingType.SingleLine
                            }
                        },
                        new Setting<LocalizedTextSettingData>
                        {
                            Id = CmsSampleSettingIds.Description,
                            Title = "Description",
                            Data = new LocalizedTextSettingData
                            {
                                Text = En(@"""This is sample
                                multiline description
                                of a sample company"""),
                                TextType = TextSettingType.MultiLine
                            }
                        },
                        new Setting<TextSettingData>
                        {
                            Id = CmsSampleSettingIds.Email,
                            Title = "Email",
                            Data = new TextSettingData
                            {
                                Text = "example@example.com",
                                TextType = TextSettingType.SingleLine
                            }
                        }
                }.ToImmutableList2()
            }
        );

        await context.SaveChangesAsync();

        var ds = context.SettingGroups.Include(x => x.Records).ToList();

    }

    private static async Task CreateIfNotExist(CmsSampleDbContext context, SettingGroup settingGroup)
    {
        var groupDb = await context.SettingGroups
            .Include(x => x.Records)
            .SingleOrDefaultAsync(x => x.Id == settingGroup.Id) ?? new SettingGroupDb(settingGroup.Id, settingGroup.Title, []);

        var existingSetingIds = groupDb.Records.Select(x => x.Id);

        var newRecords = settingGroup.Records
                .Where(x => groupDb.Records.All(y => y.Id != x.Id))
                .Select(x => FromDto(x, groupDb))
                .ToList();

        await context.SettingRecords.AddRangeAsync(newRecords);
    }

    private static SettingDb FromDto(Setting setting, SettingGroupDb groupDb)
    {
        var settingType = setting.GetType();

        if (setting is ISetting<SettingData> typedSetting)
        {
            return new SettingDb(setting.Id, setting.Title, typedSetting.Data, groupDb);
        }

        throw new Exception($"Unknown type of setting {settingType}");
    }
}
