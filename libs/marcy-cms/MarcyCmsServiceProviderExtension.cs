using CandyKingdom.Marcy.ImageMin;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Storage;
using CandyKingdom.MarcyCms.Data;
using CandyKingdom.MarcyCms.Settings;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CandyKingdom.MarcyCms;

public sealed class MarcyCmsConfiguration
{
    public required ImageUploaderConfig ImageUploaderConfig { get; set; }
    public required VideoUploaderConfig VideoUploaderConfig { get; set; }
    public required IFileStorage FileStorage { get; set; }
    public required IList<ImageMinVendor> MinificationVendors { get; set; }
}

public static class MarcyCmsServiceCollectionExtension
{
    public static DirectoryInfo DefaultCacheDir { get; } = new("media_cache");
    public const int DefaultImageMinQuality = 90;

    public static IServiceCollection AddMarcyCms<TDbContext>(this IServiceCollection services, Action<MarcyCmsConfiguration>? configurator = null)
        where TDbContext : DbContext, IMarcyCmsDbContext
    {
        var config = new MarcyCmsConfiguration
        {
            ImageUploaderConfig = new ImageUploaderConfig { CacheDir = DefaultCacheDir },
            VideoUploaderConfig = new VideoUploaderConfig { CacheDir = DefaultCacheDir },
            FileStorage = new LocalFileStorage("/files/", Path.Combine(Directory.GetCurrentDirectory(), "files")),
            MinificationVendors = [
                  new ImageMinJpegtran(new ImageMinJpegtranOptions()),
            new ImageMinMozJpeg(new ImageMinMozJpegOptions(quality: DefaultImageMinQuality)),
            new ImageMinWebp(new ImageMinWebpOptions(quality: DefaultImageMinQuality)),
            // new ImageMinGuetzli(new ImageMinGuetzliOptions(quality: DefaultImageMinQuality))
        ]
        };

        configurator?.Invoke(config);

        config.ImageUploaderConfig.CacheDir.Create();
        config.VideoUploaderConfig.CacheDir.Create();

        services.AddSingleton(config.FileStorage);
        services.AddSingleton(config.ImageUploaderConfig);
        services.AddSingleton(config.VideoUploaderConfig);

        var imageMin = new ImageMin(config.MinificationVendors);
        services.AddSingleton<IImageMin>(imageMin);

        services.AddSingleton<ISvgUploader, SvgUploader>();
        services.AddSingleton<IImageUploader, ImageUploader>();
        services.AddSingleton<IVideoUploader, VideoUploader>();

        services.AddSingleton<IImageManager, ImageManager>();
        services.AddSingleton<IVideoManager, VideoManager>();

        services.AddSingleton<IPageManager, PageManager<TDbContext>>();
        services.AddSingleton<IViewManager, ViewManager<TDbContext>>();
        services.AddSingleton<ISettingsManager, SettingsManager<TDbContext>>();

        return services;
    }
}
