# marcy-gen

TypeGen code-generation tool. Reads C# domain models from `libs/marcy` and `libs/marcy-cms` via reflection and writes matching TypeScript interface files into the Angular libraries.

## What it generates

| Source (C#) | Output (TypeScript) |
| --- | --- |
| `libs/marcy` models | `libs/bonnie/src/lib/generated/` |
| `libs/marcy-cms` models | `libs/bonnie-cms/src/lib/generated/` |

## Usage

```bash
dotnet run --project apps/marcy-gen
```

Run this after making any change to public models in `libs/marcy` or `libs/marcy-cms`.

> The sample app (`apps/marcy-cms-sample`) also runs its own generation spec at startup for its custom bone types, writing to `apps/bonnie-cms-sample/src/app/generated/`. That generation is separate from this tool.

## Generated files

All generated files begin with:

```text
/** This is a TypeGen auto-generated file. Any changes made to this file can be lost when this file is regenerated. */
```

**Never edit any file inside a `generated/` folder.** Changes will be overwritten on the next generation run.

## How it works

The tool uses [TypeGen](https://jburzynski.github.io/TypeGen/) to reflect over C# assemblies and produce TypeScript.

Two generation specs are defined:

- `MarcyGenerationSpec` — exports all types from `libs/marcy`: `Bone`, `Page`, `View`, `Image`, `Video`, all media and localization types
- `MarcyCmsGenerationSpec` — exports settings types from `libs/marcy-cms`: `SettingGroup`, `Setting`, all `SettingData` subtypes

### Name converters

| Converter | Behavior |
| --- | --- |
| `JsonMemberNameConverter` | Camel-cases property names to match JSON serialization |
| `TypeNameConverter` | Adjusts C# type names to TypeScript conventions |
| `FileNameConverter` | Maps type names to kebab-case file names |

### Spec customization

The specs use TypeGen's fluent builder to customize output:

```csharp
// Override inferred TypeScript type
spec.Member(nameof(MyType.Prop)).Type("MyTsType", "@candy-kingdom/bonnie");

// Rename a member (e.g. add $type discriminator)
spec.Member(nameof(MyType.Type)).MemberName("$type");

// Exclude a member from output
spec.Member(nameof(MyType.Internal)).Ignore();
```

## Adding a new C# type

1. Add the C# record/class to `libs/marcy` or `libs/marcy-cms`.
2. Register it in the relevant generation spec (`MarcyGenerationSpec` or `MarcyCmsGenerationSpec`).
3. Run `dotnet run --project apps/marcy-gen`.
4. The generated TypeScript file appears in the corresponding `generated/` folder and is automatically exported from the library's `index.ts`.
