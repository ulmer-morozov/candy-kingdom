using Autofac;

using CandyKingdom.Marcy;
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
                        new Setting<SvgSettingData>
                        {
                            Id = CmsSampleSettingIds.Logo,
                            Title = "Logo",
                            Data = new SvgSettingData
                            {
                                Src=new FileSrc<ImageMeta> { Meta = ImageMeta.Empty, Url = "", MimeType = "" }
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
                        new Setting<FileSettingData>{
                            Id=CmsSampleSettingIds.SomeFile,
                            Title="Some json File",
                            Data = new(){
                                AllowedMimeTypes = new ImmutableList2<string>(["application/json"])
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

        await CreateIfNotExist
        (
            context,
            new FaceView()
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

    private static async Task CreateIfNotExist(CmsSampleDbContext context, params ViewFactory[] viewFactories)
    {
        var existingCodes = await context.Views
                .Where(x => viewFactories.Select(y => y.Code).Contains(x.Code))
                .Select(x => x.Code)
                .ToListAsync();

        var viewDbs = viewFactories
                        .Where(x => !existingCodes.Contains(x.Code))
                        .Select(x => x.Create())
                        .Select
                        (
                            x => new ViewDb
                            (
                                id: Guid.NewGuid(),
                                code: x.Code,
                                bones: x.Bones
                            )
                        )
                        .ToList();

        await context.Views.AddRangeAsync(viewDbs);
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
            Guid.NewGuid(),
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
            .ForEach(pageDb.Children.Add);

        return pageDb;
    }
}
