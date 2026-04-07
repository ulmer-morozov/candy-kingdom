# marcy-cms-sample

.NET 10 reference implementation for `CandyKingdom.MarcyCms`. Demonstrates how to wire up the CMS backend with SQLite, ASP.NET Core Identity, custom bone types, and TypeScript contract generation.

## Purpose

This app serves two roles:

1. **Backend API** — serves `bonnie-cms-sample` at runtime (pages, views, settings, media upload)
2. **Contract generator** — regenerates TypeScript interfaces for `bonnie-cms-sample` on every startup

## Running

```bash
dotnet run --project apps/marcy-cms-sample
```

On startup, the app:

1. Regenerates TypeScript files in `apps/bonnie-cms-sample/src/app/generated/` via `MarcyCmsSampleGenerationSpec`
2. Applies EF Core migrations to the SQLite database
3. Seeds initial pages, views, and settings if the database is empty
4. Starts the ASP.NET Core web server

Swagger UI is available at `/swagger` in development.

## Architecture

### Custom bone types

The sample defines three bone types beyond the base library:

| Type | C# record | TypeScript interface | Description |
| --- | --- | --- | --- |
| `"text"` | `TextBone` | `TextBone` | Localized title + body text |
| `"media"` | `MediaBone` | `MediaBone` | Image or video with caption and link |
| `"page-list"` | `PageListBone` | `PageListBone` | List of child pages (fetched via `IHaveDataRouteWithData`) |
| `"vimeo"` | `VimeoBone` | `VimeoBone` | Vimeo embed |

Custom bones extend `Bone`, declare `const string BoneType`, and are registered in `BoneTypeInfoResolver` for JSON polymorphism.

### Custom page data

`ProjectPageData` extends `PageData` with a `SpecialTitle: LocalizedString` property — demonstrates per-page-type typed data on `Page<ProjectPageData>`.

### Initial content

`PageFactory` subclasses under `Content/` define the page tree declaratively:

- `HomePage` — root page with a face view
- `AboutPage` — about page
- `ProjectsPage` — container for project sub-pages
- `ProjectA`, `ProjectB` — sample project pages using `ProjectPageData`

`FaceView` defines the navigation skeleton (`code: "face"`).

### Database

EF Core with SQLite. The `DbContext` is `CmsSampleDbContext`, which extends `IdentityDbContext<ApplicationUser>` and implements `IMarcyCmsDbContext`.

`InitialDataFiller` seeds the database on first run.

### TypeScript generation

`MarcyCmsSampleGenerationSpec` is a TypeGen `GenerationSpec` that:

- Exports the sample's custom bone types and style constants to TypeScript
- Adds `$type` discriminator properties matching C# JSON polymorphism configuration
- Imports base types from `@candy-kingdom/bonnie` rather than re-generating them

Output goes to `apps/bonnie-cms-sample/src/app/generated/` on every server start.

## Key files

| File | Description |
| --- | --- |
| `Program.cs` | Entry point — generation, DI setup, middleware pipeline |
| `MarcyCmsSampleGenerationSpec.cs` | TypeGen spec for sample-specific types |
| `Bones/` | Custom bone record types |
| `Content/` | Page tree factory classes and view definition |
| `Core/ProjectPageData.cs` | Custom typed page data |
| `Data/CmsSampleDbContext.cs` | EF Core context |
| `Data/InitialDataFiller.cs` | Database seeder |
| `Serialization/BoneTypeInfoResolver.cs` | JSON polymorphism registration |
| `Serialization/CmsJsonSerializationOptions.cs` | Application-wide `JsonSerializerOptions` |
