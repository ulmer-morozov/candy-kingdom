# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

CandyKingdom is an Nx monorepo for a headless CMS framework. Angular 21 + Nx 22 on the front end; .NET 10 on the back end.

Two language ecosystems share data contracts via code generation:

- **TypeScript/Angular** — rendering library, CMS admin UI, sample app
- **C#/.NET** — domain models, media processing, REST API, sample server

## Architecture

```text
libs/marcy          ← C# domain models, media tools (source of truth for data contracts)
libs/marcy-cms      ← ASP.NET Core CMS layer (controllers, EF Core, settings)
apps/marcy-gen      ← TypeGen tool: reads libs/marcy + libs/marcy-cms, writes TS interfaces
apps/marcy-cms-sample ← .NET sample app: wires up marcy-cms, regenerates its own TS contracts

libs/bonnie         ← Angular rendering library (@candy-kingdom/bonnie)
libs/bonnie-cms     ← Angular CMS UI library (@candy-kingdom/bonnie-cms)
apps/bonnie-cms-sample ← Angular SSR sample app
```

Path aliases (tsconfig.base.json):

- `@candy-kingdom/bonnie` → `libs/bonnie/src/index.ts`
- `@candy-kingdom/bonnie-cms` → `libs/bonnie-cms/src/index.ts`

---

## Code Generation Flow

**Never edit files inside any `generated/` folder.** They are produced by TypeGen from C# source and will be overwritten.

1. C# models live in `libs/marcy` and `libs/marcy-cms`.
2. `apps/marcy-gen` reads those models and writes `.ts` interface files to:
   - `libs/bonnie/src/lib/generated/`
   - `libs/bonnie-cms/src/lib/generated/`
3. `apps/marcy-cms-sample` runs its own `MarcyCmsSampleGenerationSpec` at startup and writes sample-app-specific types to `apps/bonnie-cms-sample/src/app/generated/`.

Generated files begin with: `/** This is a TypeGen auto-generated file. ... */`

To regenerate: `dotnet run --project apps/marcy-gen` (or start `marcy-cms-sample`).

---

## libs/bonnie (`@candy-kingdom/bonnie`)

Angular rendering library. Contains components, directives, pipes, and TypeScript interfaces for rendering CMS content pages delivered from the API.

### bonnie — source layout

- `src/index.ts` — public API barrel
- `src/lib/generated/` — auto-generated TS interfaces mirroring C# marcy models
- `src/lib/skeleton/` — dynamic skeleton rendering system
- `src/lib/marcy-image/` — `MarcyImageComponent` (`<bon-image>`), `ImageSrcDirective` (`[imgsrc]`)
- `src/lib/marcy-video/` — `MarcyVideoComponent` (`<bon-video>`), `VideoSrcDirective` (`[vidsrc]`)
- `src/lib/marcy-media/` — `MarcyMediaComponent` (`<bon-media>`) — dispatches to image or video
- `src/lib/localization/` — `LocalizeServiceBase`, `LocalizePipe`, `LocalizeObjectPipe`, `LocalizeUrlPipe`
- `src/lib/core/` — `DeviceServiceBase`, `DeviceService`, `IntersectionComponent`, `SrcBaseDirective`
- `src/variables.scss` — shared SCSS variables

### bonnie — core types (generated from libs/marcy)

- `Bone` — base content block: `style`, `mediaQuery`, `enabled`, `type`
- `Page<T extends PageData>` / `PageBase` — `id`, `url`, `route`, `title: LocalizedString`, `bones: Bone[]`, `children: PageBase[]`
- `View` — named skeleton (nav layouts etc.), identified by `code: string`
- `Image extends PixMedia` — `sources: ImageSource[]`, `type: 'image'`
- `Video extends PixMedia` — `sources: VideoSource[]`, `type: 'video'`
- `LocalizedString extends LocalizedObject<string>` — `{ [locale: string]: string }`
- `IHaveDataRoute` / `IHaveDataRouteWithData<T>` — bones that require child-page data fetched separately

### bonnie — skeleton pattern

`SkeletonComponent` takes `[map]: BoneMap` and `[bones]: Bone[]`. It dynamically instantiates the correct Angular component for each bone type via `viewContainerRef.createComponent()`. `BoneMap extends Map<string, Type<any>>` with a `getRequired()` helper.

### bonnie — media components

- `<bon-media [src]="...">` — unified component that renders `<bon-image>` or `<bon-video>` based on `src.type`
- `<bon-image>` requires `[imgsrc]` directive on a parent providing an `Image` signal
- `<bon-video>` requires `[vidsrc]` directive providing a `Video` signal
- `SrcBaseDirective` manages media query watchers and pixel ratio reactively with signals + RxJS
- All media components are SSR-compatible (guard `typeof window === 'undefined'`)

### bonnie — services

- `DeviceServiceBase` (abstract) — `isSSR: boolean`, `devicePixelRatio: number`; `DeviceService` is the browser impl
- `LocalizeServiceBase` (abstract) — `locale: Signal<string>`, `getLocalized<T>()`, `getLocalizedText()`, `isLocalUrl()`

---

## libs/bonnie-cms (`@candy-kingdom/bonnie-cms`)

Angular CMS admin UI library. Provides all components, services, base classes, and directives for building a content management interface on top of the bonnie rendering layer.

### bonnie-cms — source layout

- `src/index.ts` — public API barrel
- `src/lib/generated/` — auto-generated TS interfaces for CMS-specific server types
- `src/lib/services/data.service.ts` — `DataService`: fetches pages, views, settings; resolves `IHaveDataRoute` bones
- `src/lib/services/admin-data.service.ts` — `AdminDataService`: reads/writes pages and settings via admin API
- `src/lib/services/API_BASE_URL.ts` — `API_BASE_URL` injection token
- `src/lib/skeleton-editor/` — `SkeletonEditorComponent`, `BoneEditorContainerComponent`, `BoneEditorMap`, `IBoneEditor`, `IBoneTemplate`, `ContentPreset`
- `src/lib/bone-editors/bone-editor-base.component.ts` — `BoneEditorBaseComponent<TBone>`: abstract base for all bone editors
- `src/lib/core-components/editable.directive.ts` — `EditableDirective<T>`: implements `ControlValueAccessor`, manages edit/save/cancel lifecycle
- `src/lib/core-components/editable-group.ts` — `EditableGroupComponent`: coordinates multiple `EditableDirective` instances
- `src/lib/core-components/form-base.component.ts` — `FormBaseComponent<TData>`: abstract base for CMS form components
- `src/lib/forms/` — `TextFormComponent`, `TranslationFormComponent`, `SeoFormComponent`, `OneImageFormComponent`, `SvgFormComponent`, `FileFormComponent`
- `src/lib/translation-input/` — `TranslationInputComponent`: inline `<input>` for localized strings
- `src/lib/translation-textarea/` — `TranslationTextareaComponent`: `<textarea>` for localized strings
- `src/lib/admin-controls/` — `AdminControlsComponent`: toolbar for switching locale and device preview
- `src/lib/media-uploader/` — `MediaUploaderComponent`: uploads images or videos
- `src/lib/file-uploader/` — `FileUploaderComponent`: uploads generic files
- `src/lib/styles/` — `admin-styles.scss`, `admin-variables.scss`, `variables.scss`

### bonnie-cms — core types (generated from libs/marcy-cms)

- `Setting<T extends SettingData>` / `SettingGroup` — CMS settings
- `SettingData` subtypes: `TextSettingData`, `LocalizedTextSettingData`, `OneImageSettingData`, `SvgSettingData`, `FileSettingData`

### bonnie-cms — bone editor pattern

- Extend `BoneEditorBaseComponent<TBone>` and implement `IBoneEditor<TBone>`
- `bone` property is a `WritableSignal<TBone>` — access properties as `bone().property` in templates and `this.bone().property` in TS
- Implement `getPresets(): ContentPreset<TBone>[]` for style variant switching
- `BoneEditorMap` (`ReadonlyMap<string, Type<IBoneEditor>>`) maps bone type strings to editor components

### bonnie-cms — EditableDirective

The fundamental CMS data binding primitive. Implements Angular `ControlValueAccessor`. Tracks original vs. current value, exposes `isDirty`, and drives the edit/save/cancel lifecycle. Forms extend `FormBaseComponent` and use `hostDirectives: [EditableDirective]`.

### bonnie-cms — services

- `DataService` — `getPage(route)`, `getView(code)`, `getSettings(ids[])`; automatically resolves `dataRoute` bones
- `AdminDataService` — `getSettingGroups()`, `getPage(url)`, `storePage(page)`, `updateSettings(settings[])`

---

## libs/marcy (C#)

Core .NET domain library. Defines all domain models (pages, bones, media), media processing tools (image/video conversion), file storage abstractions, and JSON serialization infrastructure. **Source of truth for all data contracts.**

### marcy — source layout

- `Bone.cs` — abstract base record: `Style`, `MediaQuery`, `Enabled`, `abstract string Type`
- `Page.cs` — `Page<T>` with `Id`, `Url`, `Route`, `Title: LocalizedString`, `Bones`, `Children`; `Page<T>` adds `Data: T`
- `View.cs` — named skeleton: `Id`, `Code`, `Bones`
- `PixMedia.cs` — abstract record with `[JsonPolymorphic]` for `Image`/`Video` union
- `Image.cs`, `Video.cs` — concrete media records with typed `Sources`
- `LocalizedString.cs` / `LocalizedObject.cs` — `LocalizedObject<T>` is a dictionary `{ [locale] → T }`; `LocalizedString` adds `.En()`, `.Combine()` helpers
- `Skeleton/IHaveDataRoute.cs` / `IHaveDataRouteWithData.cs` — interfaces for bones with deferred child data
- `ImageTools/` — `IImageManager`/`ImageManager` (NetVips), `IVideoManager`/`VideoManager` (xFFmpeg.NET), `IImageUploader`/`ImageUploader`, `IVideoUploader`/`VideoUploader`, `ISvgUploader`/`SvgUploader`
- `ImageMin/` — image minification pipeline with vendor wrappers (jpegtran, mozjpeg, webp, guetzli)
- `Storage/IFileStorage.cs` — `Store(stream, name, mimeType)` abstraction; impls: `LocalFileStorage`, `S3FileStorage`
- `Serialization/SmartJsonProducer.cs` — serializes objects while running image/video conversion inline via custom `JsonConverter`s
- `Immutables/` — `ImmutableList2<T>` and `ImmutableDictionary2<T,V>`: value-equality wrappers with custom JSON converters (required for C# record value semantics)
- `ResultOrError.cs` — result monad: `ResultOrError` / `ResultOrError<T>`
- `PageFactory.cs`, `SkeletonFactory.cs`, `ViewFactory.cs`, `WebsiteRoot.cs` — builder classes for constructing initial page trees in code

### marcy — key patterns

- All collection properties in domain records use `ImmutableList2<T>` for value equality.
- JSON polymorphism uses `[JsonPolymorphic]` / `[JsonDerivedType]` with `$type` discriminator.
- `SmartJsonProducer` enables a build-time workflow: serialize a data object and automatically convert any `Image`/`Video` fields to produce all responsive sizes.

---

## libs/marcy-cms (C#)

ASP.NET Core CMS layer. Provides REST API controllers, EF Core data access, settings management, sitemap building, and the DI registration entry point. Host applications reference this library and supply their own `DbContext`.

### marcy-cms — source layout

- `MarcyCmsServiceProviderExtension.cs` — `AddMarcyCms<TDbContext>()` extension: registers all services (`IPageManager`, `IViewManager`, `ISettingsManager`, all upload/processing services)
- `MarcyCmsConfiguration` — config object: `ImageUploaderConfig`, `VideoUploaderConfig`, `IFileStorage`, `MinificationVendors`
- `IPageManager.cs` / `PageManager.cs` — `GetAsync(url)`, `GetChildrenAsync(url)`, `StoreAsync(page)`
- `IViewManager.cs` / `ViewManager.cs` — `GetAsync(code)`
- `Settings/ISettingsManager.cs` / `SettingsManager.cs` — `GetSettingsAsync(ids)`, `UpdateAsync(settings)`, `GetGroupsAsync()`
- `Data/IMarcyCmsDbContext.cs` — interface consumer `DbContext` must implement: `Views`, `Pages`, `SettingGroups`, `SettingRecords`
- `Data/MarcyDbContextExtensions.cs` — `SetupForMarcyCms(serializerOptions)`: configures EF Core JSON column serialization for `Bone[]`, `LocalizedString`, etc.
- `Controllers/PageController.cs` — `GET /api/pages?url=...`, `GET /api/pages/children?url=...` (public)
- `Controllers/AdminPageController.cs` — `GET /api/admin/pages/{url}`, `POST /api/admin/pages` (`[Authorize]`)
- `Controllers/ViewController.cs` — `GET /api/views/{code}` (public)
- `Controllers/SettingsController.cs` — `GET /api/settings?ids=...` (public)
- `Controllers/AdminSettingsController.cs` — `GET/POST /api/admin/settings` (`[Authorize]`)
- `Controllers/AdminUploadImageController.cs` — `POST /api/admin/upload/image` (Raw, Complex, single, SVG variants)
- `Controllers/AdminUploadVideoController.cs` — `POST /api/admin/upload/video`
- `Controllers/AdminUploadFileController.cs` — `POST /api/admin/upload/file`
- `SitemapBuilder.cs` — builds XML sitemap from a `WebsiteRoot` page tree

### marcy-cms — key patterns

- Consumer apps call `services.AddMarcyCms<MyDbContext>()` and implement `IMarcyCmsDbContext`.
- `Bone[]` and related types are stored as JSON columns; `builder.SetupForMarcyCms()` configures EF Core for this.
- Host app must register controllers from the MarcyCms assembly: `AddApplicationPart(typeof(MarcyCmsServiceCollectionExtension).Assembly)`.
- All admin endpoints require `[Authorize]`; identity integration is left to the host app.

---

## apps/marcy-cms-sample (.NET)

Back-end sample app. Wires up `libs/marcy-cms` with SQLite, ASP.NET Core Identity, custom bone types, and TypeScript contract generation.

### marcy-cms-sample — source layout

- `Program.cs` — entry point: calls `SpecGenerator.GenerateTsFiles<MarcyCmsSampleGenerationSpec>()` to regenerate Angular `generated/` files at startup, then configures Identity, EF Core (SQLite), `AddMarcyCms`, controllers, static files, Swagger
- `MarcyCmsSampleGenerationSpec.cs` — TypeGen `GenerationSpec` for sample-app-specific types (custom imports from `@candy-kingdom/bonnie`, `$type` discriminators)
- `Bones/TextBone.cs` — `record TextBone : Bone` (`Title`, `Text: LocalizedString`, `Type = "text"`)
- `Bones/MediaBone.cs` — `record MediaBone : Bone` (`Media: PixMedia`, `Title`, `Text`, `Alt`, `Link`, `Type = "media"`)
- `Bones/PageListBone.cs` — `record PageListBone : Bone, IHaveDataRouteWithData<ImmutableList2<Page>>` (`Type = "page-list"`)
- `Bones/VimeoBone.cs` — Vimeo embed bone
- `Content/` — `PageFactory` subclasses defining initial page tree (`HomePage`, `AboutPage`, `ProjectsPage`, etc.) and `FaceView`
- `Core/ProjectPageData.cs` — custom `PageData` subclass with `SpecialTitle: LocalizedString`
- `Data/CmsSampleDbContext.cs` — EF Core `IdentityDbContext<ApplicationUser>` implementing `IMarcyCmsDbContext`
- `Data/InitialDataFiller.cs` — seeds SQLite DB on first run
- `Serialization/BoneTypeInfoResolver.cs` — `DefaultJsonTypeInfoResolver` adding polymorphism for `Bone`, `PageData`, `SettingData`
- `Serialization/CmsJsonSerializationOptions.cs` — configures `JsonSerializerOptions` for the application

### marcy-cms-sample — key patterns

- TypeScript files are regenerated every server startup. Changes to C# models immediately flow to the Angular app on next server start.
- Custom bone types extend `Bone`, declare `const string BoneType`, and must be registered in `BoneTypeInfoResolver`.
- `PageFactory` subclasses use `AddBones(...)` and `AddChild<T>()` to build the page tree declaratively.

---

## apps/bonnie-cms-sample (Angular)

Angular 21 SSR sample app. Demonstrates both the public-facing site (SSR) and the CMS admin UI, backed by `marcy-cms-sample`.

### bonnie-cms-sample — source layout

- `src/app/app.routes.ts` — routes: `signin`, `register`, `admin/**` (lazy + auth guard), `**` (face catch-all resolving page, faceView, settings)
- `src/app/app.config.ts` — `provideZonelessChangeDetection()`, `provideHttpClient()`, `provideClientHydration()`, `AuthInterceptor`, `AuthGuard`, `AuthService`
- `src/app/FaceBoneMap.ts` — `BoneMap`: `"text"` → `TextBoneComponent`, `"media"` → `MediaBoneComponent`, `"page-list"` → `PageListBoneComponent`
- `src/app/AdminBoneMap.ts` — `BoneEditorMap`: `"text"` → `TextBoneEditorComponent`, etc.
- `src/app/generated/` — auto-generated by `marcy-cms-sample` at startup (never edit manually)
- `src/app/face/face.component.ts` — public page renderer: uses `SkeletonComponent` + `FaceBoneMap`; sets `<title>` and OG meta
- `src/app/admin/admin.component.ts` — admin shell, checks auth
- `src/app/admin-pages/admin-pages.component.ts` — page editor: uses `SkeletonEditorComponent` + `AdminBoneMap`; calls `AdminDataService.storePage()`
- `src/app/admin-settings/admin-settings.component.ts` — settings editor
- `src/app/bone-components/` — `TextBoneComponent`, `MediaBoneComponent`, `PageListBoneComponent` (public rendering)
- `src/app/bone-editors/` — `TextBoneEditorComponent`, `MediaBoneEditorComponent`, `PageListBoneEditorComponent` (CMS editing)
- `src/app/core/` — bone factory functions (`emptyTextBone()`, etc.), `MediaUploadMap`
- `src/app/service.ts` — `AuthService`: sign in/out via ASP.NET Identity cookie endpoints
- `src/app/guard.ts` — `AuthGuard`: redirects unauthenticated users from admin routes
- `src/app/interceptor.ts` — `AuthInterceptor`: handles 401 responses
- `src/app/router-localize.service.ts` — `RouterLocalizeService extends LocalizeServiceBase`: derives locale from URL path prefix
- `proxy.conf.json` — dev proxy to ASP.NET Core backend

### bonnie-cms-sample — bone editor pattern

1. Extend `BoneEditorBaseComponent<TextBone>`
2. Implement `getPresets()` returning `ContentPreset` objects (one per style variant via e.g. `TextBoneStyle`)
3. The host directive `[boncEditable]` (driven by `SkeletonEditorComponent`) handles save/cancel
4. Emit `saved`, `removed`, `editing` outputs consumed by `SkeletonEditorComponent`

---

## apps/marcy-gen (.NET)

Standalone TypeGen code-generation tool. Reads C# models from `libs/marcy` and `libs/marcy-cms` via reflection and writes TypeScript interface files.

### marcy-gen — source layout

- `Program.cs` — calls `GenerateTsFiles<MarcyGenerationSpec>(bonnieDir)` and `GenerateTsFiles<MarcyCmsGenerationSpec>(bonnieCmsDir)`
- `MarcyGenerationSpec.cs` — TypeGen spec for `libs/marcy` models (Bone, Page, View, Image, Video, media types, localization types, etc.)
- `MarcyCmsGenerationSpec.cs` — TypeGen spec for `libs/marcy-cms` settings models (SettingGroup, Setting, SettingData subtypes)
- `JsonMemberNameConverter.cs` — camelCases JSON member names
- `TypeNameConverter.cs` — adjusts C# type names to TypeScript conventions
- `FileNameConverter.cs` — maps C# names to kebab-case TypeScript file names

### marcy-gen — key patterns

- Spec uses fluent builder: `.Member(...).Type(...)` to override inferred types, `.Ignore()` to exclude members, `.MemberName(...)` to rename (e.g. add `$type` discriminator)
- Cross-module import refs specified inline: `.Type("LocalizedString", "@candy-kingdom/bonnie")`

---

## Angular Patterns

- **Signals**: Uses Angular signals (`input()`, `output()`, `model()`, `signal()`, `computed()`) instead of decorators. Templates call signals with `()` to unwrap values.
- **Control flow**: `@if`/`@for` instead of `*ngIf`/`*ngFor`.
- **Bone editors**: Extend `BoneEditorBaseComponent<TBone>` and implement `IBoneEditor`. Access bone properties as `bone().property` in templates and `this.bone().property` in TS.
- **BoneEditorMap**: Maps bone type strings to editor component types; uses `IBoneEditor<any>` for variance compatibility.
- **Zoneless**: `bonnie-cms-sample` uses `provideZonelessChangeDetection()`.
- **SSR**: `bonnie-cms-sample` uses `@angular/ssr` with Express; all media components guard for SSR.

---

## Build Commands

```bash
npx nx run-many -t build          # Build all projects
npx nx build bonnie               # Build bonnie library
npx nx build bonnie-cms           # Build bonnie-cms library
npx nx build bonnie-cms-sample    # Build sample app (depends on both libs)
npx nx serve bonnie-cms-sample    # Dev server
npx nx lint <project>             # Lint a project
dotnet run --project apps/marcy-gen  # Regenerate TS interfaces from C# models
```

## Versioning

```bash
npm run version-bump              # Patch version bump + push tags
```
