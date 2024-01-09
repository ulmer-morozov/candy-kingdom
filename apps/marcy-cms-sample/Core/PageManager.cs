using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms.Sample.Core;

public sealed class PageManager<TDbContext> : IPageManager
where TDbContext : DbContext, IMarcyCmsDbContext
{
    private readonly IDbContextFactory<TDbContext> _contextFactory;

    public PageManager(IDbContextFactory<TDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // For generated pages we could override this method
    public virtual async Task<ResultOrError<Page>> Get(string routeName, GetPageParams parameters)
    {
        await using var context = _contextFactory.CreateDbContext();

        var page = await context.Pages
            .Include(x => x.Title)
            .Include(x => x.OgTitle)
            .Include(x => x.OgImage)
            .Include(x => x.OgDescription)
            .Where(x => x.PublishStatus == PublishStatus.Published)
            .SingleOrDefaultAsync(x => x.RouteName == routeName);

        if (page == null)
            throw new Exception($"Страница с именем {routeName} не найдена. Возможно это страница проекта или категории и она должна генерироваться, а не браться из базы");

        parameters ??= new GetPageParams();

        var project = await context.Projects
            .SingleOrDefaultAsync(x => x.Page.Id == page.Id);

        if (project == null)
            throw new Exception($"Проект для страницы с именем {routeName} не найден.");

        var nextProject = await context.Projects
            .Include(x => x.Title)
            //
            .Include(x => x.Category)
            .ThenInclude(x => x.Title)
            //
            .Include(x => x.Page)
            .Where(x => x.Page.PublishStatus == PublishStatus.Published)
            .Where(x => x.Order < project.Order)
            .OrderByDescending(x => x.Order)
            .FirstOrDefaultAsync();

        nextProject = nextProject ?? await context.Projects
            .Include(x => x.Title)
            //
            .Include(x => x.Category)
            .ThenInclude(x => x.Title)
            //
            .Include(x => x.Page)
            .Where(x => x.Page.PublishStatus == PublishStatus.Published)
            .OrderByDescending(x => x.Order)
            .FirstOrDefaultAsync();

        Page dto;

    }

    public Task<ResultOrError> Store(Page page, bool ignoreBones = false)
    {
                if (page == null)
            return ResultOrError.Fail($"Не удалось сохранить страницу. {nameof(Page)} не может быть null");

        await using var context = _contextFactory.CreateDbContext();

        var pageDb = await context.Pages
            .Include(x => x.Title)
            .Include(x => x.OgTitle)
            .Include(x => x.OgImage)
            .Include(x => x.OgDescription)
            .SingleOrDefaultAsync(x => x.Id == page.id);

        if (pageDb == null)
            return ResultOrError.Fail($"Страница с Id {page.id} не найдена. Возможно это страница проекта или категории и она должна генерироваться, а не браться из базы");

        pageDb.Copy(page, ignoreBones);

        await context.SaveChangesAsync();

        return ResultOrError.Success();
    }

    private static Page ToDto(PageDb pageDb, bool includeDisabled, params Bone[] additionalBones)
    {
        var bones = Filter(pageDb.Bones.Concat(additionalBones), includeDisabled: includeDisabled);

        return new Page(
            id: pageDb.Id,
            publishStatus: pageDb.PublishStatus,
            routeName: pageDb.RouteName,
            title: pageDb.Title.ToLocalizedString(),
            colorTheme: pageDb.ColorTheme,
            openGraph: new OpenGraph(
                ogImage: pageDb.OgImage.ToLocalizedString(),
                ogTitle: pageDb.OgTitle.ToLocalizedString(),
                ogDescription: pageDb.OgDescription.ToLocalizedString()
            ),
            content: bones
        );
    }

    private static ImagePack ToDto(ImagePackDb packDb)
    {
        if (packDb == null)
            throw new ArgumentNullException(nameof(packDb));

        if (packDb.Alt == null)
            throw new Exception($"{nameof(packDb.Alt)} не может быть null. Ошибка преобразования в DTO {nameof(ImagePackDb)}.");

        if (packDb.Mobile == null)
            throw new Exception($"{nameof(packDb.Mobile)} не может быть null. Ошибка преобразования в DTO {nameof(ImagePackDb)}.");

        if (packDb.Tablet == null)
            throw new Exception($"{nameof(packDb.Tablet)} не может быть null. Ошибка преобразования в DTO {nameof(ImagePackDb)}.");

        if (packDb.Desktop == null)
            throw new Exception($"{nameof(packDb.Desktop)} не может быть null. Ошибка преобразования в DTO {nameof(ImagePackDb)}.");

        var imagePack = new ImagePack(
            mobile: packDb.Mobile.ToDto(),
            tablet: packDb.Tablet.ToDto(),
            desktop: packDb.Desktop.ToDto(),
            alt: packDb.Alt.ToLocalizedString()
        );

        return imagePack;
    }

}
