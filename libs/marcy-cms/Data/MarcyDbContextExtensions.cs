using System.Text.Json;

using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.Marcy.Skeleton;
using CandyKingdom.MarcyCms.Settings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CandyKingdom.MarcyCms.Data;

public static class MarcyDbContextExtensions
{
    public static void HasJsonConversion(this PropertyBuilder<LocalizedString> propertyBuilder, JsonSerializerOptions serializerOptions)
    {
        propertyBuilder.HasJsonConversion(serializerOptions, LocalizedString.Empty);
    }

    public static void HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, JsonSerializerOptions serializerOptions)
        where T : class, new()
    {
        propertyBuilder.HasConversion
        (
            v => JsonSerializer.Serialize(v, serializerOptions),
            v => JsonSerializer.Deserialize<T>(v, serializerOptions) ?? new T()
        );
    }

    public static void HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, JsonSerializerOptions serializerOptions, T defaultValue)
    {
        propertyBuilder.HasConversion
        (
            v => JsonSerializer.Serialize(v, serializerOptions),
            v => JsonSerializer.Deserialize<T>(v, serializerOptions) ?? defaultValue
        );
    }

    public static void HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, JsonSerializerOptions serializerOptions, Func<T> factory)
    {
        propertyBuilder.HasConversion
        (
            v => JsonSerializer.Serialize(v, serializerOptions),
            v => JsonSerializer.Deserialize<T>(v, serializerOptions) ?? factory()
        );
    }

    public static void SetupForMarcyCms(this ModelBuilder builder, JsonSerializerOptions serializerOptions)
    {
        builder
            .Entity<PageDb>()
            .HasIndex(x => x.Url)
            .IsUnique();

        builder
           .Entity<PageDb>()
           .Property(x => x.OpenGraph)
           .HasConversion
           (
                v => v == OpenGraphData.Empty ? "" : JsonSerializer.Serialize(v, serializerOptions),
                v => string.IsNullOrEmpty(v) ? OpenGraphData.Empty : JsonSerializer.Deserialize<OpenGraphData>(v, serializerOptions) ?? OpenGraphData.Empty
           );

        builder
            .Entity<PageDb>()
            .Property(x => x.Title)
            .HasJsonConversion(serializerOptions);

        builder
            .Entity<PageDb>()
            .Property(x => x.Bones)
            .HasJsonConversion(serializerOptions, ImmutableList2<Bone>.Empty);

        builder
            .Entity<PageDb>()
            .Property(x => x.Data)
            .HasConversion
            (
                v => v == PageData.Empty ? "" : JsonSerializer.Serialize(v, serializerOptions),
                v => string.IsNullOrEmpty(v) ? PageData.Empty : JsonSerializer.Deserialize<PageData>(v, serializerOptions) ?? PageData.Empty
            );

        builder
            .Entity<ViewDb>()
            .HasIndex(x => x.Code)
            .IsUnique();

        builder
            .Entity<ViewDb>()
            .Property(x => x.Bones)
            .HasJsonConversion(serializerOptions, ImmutableList2<Bone>.Empty);

        builder
            .Entity<SettingDb>()
            .Property(x => x.Data)
            .HasJsonConversion(serializerOptions, SettingData.Empty);

        builder
            .Entity<SettingGroupDb>()
            .HasMany(x => x.Records)
            .WithOne(x => x.Group);

    }
}
