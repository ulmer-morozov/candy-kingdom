# libs/marcy-cms — CLAUDE.md

ASP.NET Core CMS layer (`CandyKingdom.MarcyCms`). Provides the REST API, EF Core data access, and settings system consumed by Angular apps via `@candy-kingdom/bonnie-cms`. See the root `CLAUDE.md` for full project context.

## Host app setup (reference)

Consumer apps must:

1. Implement `IMarcyCmsDbContext` on their `DbContext`.
2. Call `builder.SetupForMarcyCms(serializerOptions)` in `OnModelCreating`.
3. Call `services.AddMarcyCms<TDbContext>(config)` in `Program.cs`.
4. Call `AddApplicationPart(typeof(MarcyCmsServiceCollectionExtension).Assembly)` to register controllers.

## Adding a new public controller

1. Create the controller class in `Controllers/`.
2. Inherit from `ControllerBase` and add `[ApiController]` / `[Route]`.
3. Inject services via constructor.
4. No explicit registration needed — host apps pick it up via `AddApplicationPart`.

Admin endpoints must be decorated with `[Authorize]`.

## Adding a new SettingData subtype

1. Create a record extending `SettingData` in `Settings/`:

```csharp
public record MySettingData : SettingData
{
    public const string SettingDataType = "my-setting";
    public override string Type => SettingDataType;
    public override SettingData Empty => new MySettingData { ... };
    public required string Value { get; init; }
}
```

2. Register the `[JsonDerivedType]` on `SettingData` (or in the consumer's resolver).
3. Run `dotnet run --project apps/marcy-gen` to generate the matching TypeScript interface.

## EF Core JSON columns

`Bone[]`, `LocalizedString`, and related types are stored as JSON columns. The configuration is applied by `MarcyDbContextExtensions.SetupForMarcyCms()`. When adding a new JSON column:

- Ensure the type has a registered `JsonConverter` or is covered by the existing polymorphic serialization options.
- Pass the same `JsonSerializerOptions` instance to both `SetupForMarcyCms` and the ASP.NET Core serializer so the DB and API serialization match.

## IPageManager / ISettingsManager

Prefer injecting the interfaces, not the concrete implementations. The host app may wrap or replace them.

```csharp
public class MyController(IPageManager pages, ISettingsManager settings) : ControllerBase
```

## After changing public models

Run the TypeGen tool:

```bash
dotnet run --project apps/marcy-gen
```
