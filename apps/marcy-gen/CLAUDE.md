# apps/marcy-gen — CLAUDE.md

TypeGen code-generation console app. Reads C# models from `libs/marcy` and `libs/marcy-cms` and writes TypeScript interfaces to the Angular libraries. See the root `CLAUDE.md` for full context.

## Running

```bash
dotnet run --project apps/marcy-gen
```

Output goes to:

- `libs/bonnie/src/lib/generated/` — from `MarcyGenerationSpec`
- `libs/bonnie-cms/src/lib/generated/` — from `MarcyCmsGenerationSpec`

**Never edit files in those `generated/` folders.** They are fully overwritten on each run.

## Adding a type to the generation

Open the relevant spec (`MarcyGenerationSpec.cs` or `MarcyCmsGenerationSpec.cs`) and register the new type:

```csharp
// Simplest case — generate as-is
spec.AddInterface<MyNewType>();

// With customizations
spec.AddInterface<MyNewType>()
    .Member(nameof(MyNewType.Type)).MemberName("$type")   // rename
    .Member(nameof(MyNewType.Internal)).Ignore()           // exclude
    .Member(nameof(MyNewType.Media)).Type("PixMediaUnion", "@candy-kingdom/bonnie"); // override type + import
```

Then run `dotnet run --project apps/marcy-gen`.

## Adding an enum

```csharp
spec.AddEnum<MyEnum>();
```

## Cross-library imports

When a generated type in `bonnie-cms` references a type already generated in `bonnie`, override the TypeScript type to import from `@candy-kingdom/bonnie` instead of re-generating it:

```csharp
spec.AddInterface<MyType>()
    .Member(nameof(MyType.LocalizedTitle)).Type("LocalizedString", "@candy-kingdom/bonnie");
```

## Name converters

The three converters in this project control naming for all generated output:

| Converter | Role |
| --- | --- |
| `JsonMemberNameConverter` | Camel-cases property names |
| `TypeNameConverter` | Adjusts C# type names to TS conventions |
| `FileNameConverter` | Maps type names to kebab-case file names |

Edit these only if naming conventions need to change globally.
