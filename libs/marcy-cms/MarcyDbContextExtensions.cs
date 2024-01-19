using System.Text.Json;

using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Skeleton;
using CandyKingdom.MarcyCms.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CandyKingdom.MarcyCms;

public static class MarcyDbContextExtensions
{
    public static void HasJsonConversion(this PropertyBuilder<LocalizedString> propertyBuilder) => propertyBuilder.HasJsonConversion(LocalizedStringHelpers.DefaultSerializerOptions, LocalizedString.Empty);

    public static void HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, JsonSerializerOptions serializerOptions, T defaultValue) => propertyBuilder.HasConversion
        (
            v => JsonSerializer.Serialize(v, serializerOptions),
            v => JsonSerializer.Deserialize<T>(v, serializerOptions) ?? defaultValue
        );

    public static void HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, JsonSerializerOptions serializerOptions, Func<T> factory) => propertyBuilder.HasConversion
        (
            v => JsonSerializer.Serialize(v, serializerOptions),
            v => JsonSerializer.Deserialize<T>(v, serializerOptions) ?? factory()
        );

    public static void SetupForMarcyCms(this ModelBuilder builder, JsonSerializerOptions boneSerializerOptions)
    {
        builder
          .Entity<PageDb>()
          .HasKey(x => x.Url);

        // full query <--> JSON mapping support
        builder
           .Entity<PageDb>()
           .OwnsOne
           (
               page => page.OpenGraph,
               builder => builder.ToJson()
           );

        builder
            .Entity<PageDb>()
            .Property(x => x.Title)
            .HasJsonConversion();

        builder
            .Entity<PageDb>()
            .Property(x => x.Bones)
            .HasJsonConversion(boneSerializerOptions, ImmutableList2<Bone>.Empty);

        builder
            .Entity<SettingGroupDb>()
            .HasMany(x => x.Records)
            .WithOne(x => x.Group);
    }
}
