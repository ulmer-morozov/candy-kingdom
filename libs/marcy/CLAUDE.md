# libs/marcy — CLAUDE.md

Core .NET domain library (`CandyKingdom.Marcy`). Source of truth for all data contracts shared with Angular. See the root `CLAUDE.md` for full project context.

## Adding a new bone type

Bone types are defined in consumer apps/libs, not in this library. This library defines the `Bone` abstract base only. However, if adding a new shared base type:

1. Create a C# record extending `Bone`:

```csharp
public record MyBone : Bone
{
    public const string BoneType = "my-bone";
    public override string Type => BoneType;
    public required LocalizedString Title { get; init; }
}
```

2. Register the `[JsonDerivedType]` attribute on the `Bone` base class or in the consumer's `JsonTypeInfoResolver`.
3. Run `dotnet run --project apps/marcy-gen` to regenerate TypeScript interfaces.

## ImmutableList2

All collection properties in domain records must use `ImmutableList2<T>`, not `ImmutableList<T>`, because standard `ImmutableList<T>` breaks C# `record` value equality:

```csharp
public record MyRecord
{
    public required ImmutableList2<Bone> Bones { get; init; }
}

// Creating
var record = new MyRecord { Bones = ImmutableList2.Create(bone1, bone2) };

// Adding
var updated = record with { Bones = record.Bones.Add(newBone) };
```

`ImmutableDictionary2<TKey, TValue>` works the same way for dictionaries.

## LocalizedString

```csharp
// Create
LocalizedString title = LocalizedStringHelpers.En("Hello");

// Access
string english = title.En();                          // shorthand for ["en"]
string? value = title[MarcyConstants.EnCode];

// Merge two localized strings (second overwrites first, null-safe)
LocalizedString merged = a.Combine(b);
```

## JSON polymorphism

`PixMedia`, `Bone`, `PageData`, and `SettingData` use `[JsonPolymorphic]` with a `$type` discriminator. When adding a new derived type, register it:

```csharp
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ExistingType), "existing")]
[JsonDerivedType(typeof(NewType), "new-type")]    // add here
public abstract record BaseType { }
```

Or register it in the consumer's `DefaultJsonTypeInfoResolver` if the base is in this library and the derived type is in a consumer app.

## ResultOrError

Use for operations that can fail without throwing:

```csharp
ResultOrError result = SomeOperation();
if (result.IsError) return result;

ResultOrError<MyData> typed = GetSomething();
if (typed.IsError) return typed.ToBase();
MyData data = typed.Value;
```

## SmartJsonProducer

Serializes an object graph while running image/video conversion inline. Use this for build-time content preparation — it is not a runtime serializer for API responses:

```csharp
string json = await smartJsonProducer.ProduceAsync(myPage, serializerOptions);
```

## After changing public models

Run the TypeGen tool to keep TypeScript interfaces in sync:

```bash
dotnet run --project apps/marcy-gen
```
