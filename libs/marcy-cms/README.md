# CandyKingdom.MarcyCms

ASP.NET Core CMS layer for CandyKingdom. Provides REST API controllers, EF Core data access, settings management, and the DI registration entry point. Host applications reference this library and supply their own `DbContext`.

## Installation

```bash
dotnet add package CandyKingdom.MarcyCms
```

Depends on `CandyKingdom.Marcy`.

## Getting started

### 1. Implement IMarcyCmsDbContext

Your `DbContext` must implement `IMarcyCmsDbContext`:

```csharp
public class MyDbContext : DbContext, IMarcyCmsDbContext
{
    public DbSet<ViewDb> Views { get; set; } = null!;
    public DbSet<PageDb> Pages { get; set; } = null!;
    public DbSet<SettingGroupDb> SettingGroups { get; set; } = null!;
    public DbSet<SettingDb> SettingRecords { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.SetupForMarcyCms(MyJsonSerializerOptions.Options);
    }
}
```

`SetupForMarcyCms` configures EF Core to store `Bone[]`, `LocalizedString`, and related types as JSON columns.

### 2. Register services

```csharp
builder.Services.AddMarcyCms<MyDbContext>(new MarcyCmsConfiguration
{
    FileStorage = new LocalFileStorage("/var/www/media"),
    ImageUploaderConfig = new ImageUploaderConfig { ... },
    VideoUploaderConfig = new VideoUploaderConfig { ... },
    MinificationVendors = new MinificationVendors { ... },
});
```

### 3. Register controllers

```csharp
builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(MarcyCmsServiceCollectionExtension).Assembly);
```

## REST API

### Public endpoints

| Method | Path | Description |
| --- | --- | --- |
| `GET` | `/api/pages?url={url}` | Get a page by URL |
| `GET` | `/api/pages/children?url={url}` | Get child pages |
| `GET` | `/api/views/{code}` | Get a named view |
| `GET` | `/api/settings?ids={id1}&ids={id2}` | Get settings by ID |

### Admin endpoints (require `[Authorize]`)

| Method | Path | Description |
| --- | --- | --- |
| `GET` | `/api/admin/pages/{url}` | Get a page for editing |
| `POST` | `/api/admin/pages` | Save a page |
| `GET` | `/api/admin/settings` | Get all setting groups |
| `POST` | `/api/admin/settings` | Update settings |
| `POST` | `/api/admin/upload/image` | Upload an image (multiple variants) |
| `POST` | `/api/admin/upload/video` | Upload a video |
| `POST` | `/api/admin/upload/file` | Upload a generic file |

Identity integration is left to the host application.

## Services

All services are registered by `AddMarcyCms` and available via DI:

- `IPageManager` — `GetAsync(url)`, `GetChildrenAsync(url)`, `StoreAsync(page)`
- `IViewManager` — `GetAsync(code)`
- `ISettingsManager` — `GetSettingsAsync(ids)`, `UpdateAsync(settings)`, `GetGroupsAsync()`
- `IImageUploader`, `IVideoUploader`, `ISvgUploader` — media upload pipelines
- `IImageManager`, `IVideoManager` — media conversion
- `IImageMin` — image minification
- `IFileStorage` — file storage (provided by host via `MarcyCmsConfiguration`)

## Settings

Settings are typed records that can be read from the public API and updated from the admin UI.

Built-in `SettingData` subtypes:

| Type | Description |
| --- | --- |
| `TextSettingData` | Plain text value |
| `LocalizedTextSettingData` | Localized text value |
| `OneImageSettingData` | Single image with upload config |
| `SvgSettingData` | SVG file |
| `FileSettingData` | Generic file |

## Sitemap

```csharp
var sitemap = await sitemapBuilder.BuildAsync(websiteRoot, baseUrl);
```

## TypeScript contract generation

The TypeScript interfaces in `@candy-kingdom/bonnie-cms` are generated from this library's models by `apps/marcy-gen`. After changing any public model, run:

```bash
dotnet run --project apps/marcy-gen
```
