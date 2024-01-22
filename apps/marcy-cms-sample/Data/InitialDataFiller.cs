using Autofac;

using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;
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
                        }
                }.ToImmutableList2()
            }
        );

        await CreateIfNotExist
        (
              context,
              new SettingGroup
              {
                  Id = CmsSampleSettingGroupIds.Contacts,
                  Title = "Contacts",
                  Records = new List<Setting>
                  {
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

        await CreateIfNotExist
        (
            context,
            new WebsiteRoot()
                    .AddChild<HomePage>()
                    .AddChild<ProjectsPage>(
                        projects =>
                            projects
                                .AddChild<ProjectA>()
                                .AddChild<ProjectB>()
                    )
                    .AddChild<AboutPage>()
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

    private static async Task CreateIfNotExist(CmsSampleDbContext context, PageFactory pageFactory)
    {
        var anyPageExist = await context.Pages.AnyAsync();

        if (anyPageExist)
        {
            return;
        }

        var page = pageFactory.Create("");

        var pageDb = FromDto(page);

        await context.Pages.AddAsync(pageDb);

    }

    private static SettingDb FromDto(Setting setting, SettingGroupDb groupDb)
    {
        var settingType = setting.GetType();

        if (setting is ISetting<SettingData> typedSetting)
        {
            return new SettingDb(setting.Id, setting.Title, typedSetting.Data, groupDb);
        }

        throw new ArgumentException($"Unknown type of setting {settingType}");
    }

    private static PageDb FromDto(Page page, PageDb? parent = null)
    {
        var pageData = page is IPage<PageData> pageWithData ? pageWithData.Data : PageData.Empty;

        var pageDb = new PageDb
        (
            page.Url,
            page.Route,
            page.Order,
            page.PublishStatus,
            page.Title,
            page.OpenGraph,
            page.Bones,
            pageData,
            parent: parent
        );

        page.Children
            .Select(x => FromDto(x, pageDb))
            .ToList()
            .ForEach(pageDb.Childern.Add);

        return pageDb;
    }
}
