# CandyKingdom.Marcy

Core .NET domain library for CandyKingdom CMS. Defines all domain models, media processing tools, file storage abstractions, and JSON serialization infrastructure. This is the **source of truth for all data contracts** shared with the Angular front end.

## Installation

```bash
dotnet add package CandyKingdom.Marcy
```

## What's included

### Domain models

#### Pages and bones

```csharp
// A page with typed data
var page = new Page<MyPageData>(
    Id: Guid.NewGuid(),
    Url: "/about",
    Route: "about",
    Title: LocalizedStringHelpers.En("About Us"),
    Bones: ImmutableList2.Create<Bone>(new TextBone(...)),
    Children: ImmutableList2<PageBase>.Empty
);

// A named skeleton (not a full page — used for navigation etc.)
var view = new View(Id: Guid.NewGuid(), Code: "face", Bones: ...);
```

Bone is an abstract record — concrete types declare `Type` as a constant string:

```csharp
public record TextBone : Bone
{
    public const string BoneType = "text";
    public override string Type => BoneType;
    public required LocalizedString Title { get; init; }
}
```

#### Localization

```csharp
// Create a localized string
LocalizedString title = LocalizedStringHelpers.En("Hello");

// Access a locale
string english = title.En();

// Merge two localized strings (null-safe)
LocalizedString merged = a.Combine(b);
```

`LocalizedObject<T>` is the generic base — a dictionary of `{ locale → value }` with record value semantics.

#### Media

```csharp
// Image with responsive sources
var image = new Image(
    Sources: ImmutableList2.Create(
        new ImageSource(
            SrcSet: ImmutableList2.Create(new FileSrc<ImageMeta>(...)),
            MediaQuery: "(min-width: 1024px)",
            Sizes: ...
        )
    )
);
```

`PixMedia` is an abstract record that JSON-serializes with a `$type` discriminator (`"image"` or `"video"`).

### Media processing

#### Image processing (NetVips)

```csharp
services.AddSingleton<IImageManager, ImageManager>();
services.AddSingleton<IImageUploader, ImageUploader>();
```

`IImageUploader.UploadAsync(stream, setups, fileName)` — converts an image to multiple sizes and formats, applies minification, and stores via `IFileStorage`.

`ImageSetup` describes one conversion target:

```csharp
new ImageSetup(Width: 800, Format: FileFormat.WebP, TransformType: TransformType.Contain)
```

#### Video processing (xFFmpeg.NET)

```csharp
services.AddSingleton<IVideoManager, VideoManager>();
services.AddSingleton<IVideoUploader, VideoUploader>();
```

#### SVG

```csharp
services.AddSingleton<ISvgUploader, SvgUploader>();
```

### File storage

Implement `IFileStorage` or use one of the built-in adapters:

```csharp
// Local disk
services.AddSingleton<IFileStorage>(new LocalFileStorage("/var/www/media"));

// AWS S3
services.AddSingleton<IFileStorage>(new S3FileStorage(bucketName, s3Client));
```

### Smart serialization

`SmartJsonProducer` serializes a data object while running image/video conversion inline. Useful for seeding initial content that contains placeholder images:

```csharp
var json = await smartJsonProducer.ProduceAsync(myPage);
```

### Immutable collections

All collection properties in domain records use `ImmutableList2<T>` — a value-equality wrapper around `ImmutableList<T>` required for C# `record` equality semantics. Custom JSON converters are included.

### Result monad

```csharp
ResultOrError result = DoSomething();
ResultOrError<MyData> resultWithData = DoSomethingWithReturn();
```

## JSON polymorphism

`PixMedia`, `Bone`, `PageData`, and `SettingData` use `[JsonPolymorphic]` with `$type` discriminator. Register derived types:

```csharp
[JsonDerivedType(typeof(TextBone), TextBone.BoneType)]
[JsonDerivedType(typeof(MediaBone), MediaBone.BoneType)]
```

## TypeScript contract generation

The TypeScript interfaces in `@candy-kingdom/bonnie` are generated from this library's models by `apps/marcy-gen`. After changing any public model, run:

```bash
dotnet run --project apps/marcy-gen
```
