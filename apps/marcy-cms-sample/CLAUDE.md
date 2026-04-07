# apps/marcy-cms-sample — CLAUDE.md

.NET reference implementation for `CandyKingdom.MarcyCms`. Backs the Angular sample app at runtime and regenerates its TypeScript contracts on startup. See the root `CLAUDE.md` for full context.

## Running

```bash
dotnet run --project apps/marcy-cms-sample
```

On startup this app:

1. Regenerates `apps/bonnie-cms-sample/src/app/generated/` via `MarcyCmsSampleGenerationSpec`.
2. Applies EF Core migrations.
3. Seeds the database if empty (`InitialDataFiller`).
4. Starts the web server (Swagger at `/swagger`).

## Adding a new bone type

This is a 4-step process:

**Step 1 — Define the C# record** in `Bones/`:

```csharp
public record MyBone : Bone
{
    public const string BoneType = "my-bone";
    public override string Type => BoneType;
    public required LocalizedString Title { get; init; }
}
```

**Step 2 — Register JSON polymorphism** in `Serialization/BoneTypeInfoResolver.cs`:

```csharp
options.AddDerivedType(typeof(Bone), typeof(MyBone), MyBone.BoneType);
```

**Step 3 — Register TypeScript generation** in `MarcyCmsSampleGenerationSpec.cs`:

```csharp
spec.AddInterface<MyBone>()
    .Member(nameof(MyBone.Type)).MemberName("$type");
// Add style constants if applicable:
spec.AddInterface<MyBoneStyle>();
```

**Step 4 — Start the server** to regenerate TypeScript, then register the bone in the Angular app (`FaceBoneMap` and `AdminBoneMap`).

## Adding a style enum or constants class

Style constants are plain C# classes with `const string` fields (not enums). Register them in the generation spec:

```csharp
spec.AddInterface<MyBoneStyle>();
```

## Adding a page to the initial content tree

Create a `PageFactory` subclass in `Content/`:

```csharp
public class MyPage : PageFactory
{
    protected override void Configure()
    {
        SetUrl("/my-page");
        SetTitle(LocalizedStringHelpers.En("My Page"));
        AddBones(new TextBone { ... });
    }
}
```

Register it as a child in the parent factory:

```csharp
// In ParentPage.Configure():
AddChild<MyPage>();
```

Add it to `InitialDataFiller` if it needs to be seeded.

## TypeScript generation spec

`MarcyCmsSampleGenerationSpec` writes to `apps/bonnie-cms-sample/src/app/generated/`. It runs automatically on server start — no manual regeneration step needed during development.

The generated folder in the Angular app is owned by this server. **Never edit `apps/bonnie-cms-sample/src/app/generated/` manually.**

## Database

SQLite file: `marcy-cms-sample.db` (in the project directory, gitignored).

To reset: delete the `.db` file and restart the server. `InitialDataFiller` will reseed.

## JSON serialization

`CmsJsonSerializationOptions.Options` is the single `JsonSerializerOptions` instance for the whole app. It is passed to EF Core (via `SetupForMarcyCms`) and to ASP.NET Core's serializer. Always use this instance — never create separate `JsonSerializerOptions`.
