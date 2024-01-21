using System.Text.Json;

using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.Marcy.Skeleton;
using CandyKingdom.MarcyCms.Data;
using CandyKingdom.MarcyCms.Settings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CandyKingdom.MarcyCms;

public static class MarcyDbContextExtensions
{
    public static void HasJsonConversion(this PropertyBuilder<LocalizedString> propertyBuilder)
    {
        propertyBuilder.HasJsonConversion(LocalizedStringHelpers.DefaultSerializerOptions, LocalizedString.Empty);
    }

    public static void HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, JsonSerializerOptions? serializerOptions = null)
        where T : class, new()
    {
        propertyBuilder.HasConversion
        (
            v => JsonSerializer.Serialize(v, serializerOptions),
            v => JsonSerializer.Deserialize<T>(v, serializerOptions) ?? new T()
        );
    }


    public static void HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, JsonSerializerOptions? serializerOptions, T defaultValue)
    {
        propertyBuilder.HasConversion
        (
            v => JsonSerializer.Serialize(v, serializerOptions),
            v => JsonSerializer.Deserialize<T>(v, serializerOptions) ?? defaultValue
        );
    }

    public static void HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, JsonSerializerOptions? serializerOptions, Func<T> factory)
    {
        propertyBuilder.HasConversion
        (
            v => JsonSerializer.Serialize(v, serializerOptions),
            v => JsonSerializer.Deserialize<T>(v, serializerOptions) ?? factory()
        );
    }

    public static void SetupForMarcyCms(this ModelBuilder builder,
    JsonSerializerOptions boneSerializerOptions,
    JsonSerializerOptions settingSerializerOptions,
    JsonSerializerOptions pageDataSerializerOptions)
    {
        builder
              .Entity<PageDb>()
              .HasKey(x => x.Url);

        builder
           .Entity<PageDb>()
           .Property(x => x.OpenGraph)
           .HasJsonConversion();

        builder
            .Entity<PageDb>()
            .Property(x => x.Title)
            .HasJsonConversion();

        builder
            .Entity<PageDb>()
            .Property(x => x.Bones)
            .HasJsonConversion(boneSerializerOptions, ImmutableList2<Bone>.Empty);

        builder
            .Entity<PageDb>()
            .Property(x => x.Data)
            .HasConversion
            (
                v => v == PageData.Empty ? "" : JsonSerializer.Serialize(v, pageDataSerializerOptions),
                v => string.IsNullOrEmpty(v) ? PageData.Empty : JsonSerializer.Deserialize<PageData>(v, pageDataSerializerOptions) ?? PageData.Empty
            );

        builder
            .Entity<SettingDb>()
            .Property(x => x.Data)
            .HasJsonConversion(settingSerializerOptions, SettingData.Empty);

        builder
            .Entity<SettingGroupDb>()
            .HasMany(x => x.Records)
            .WithOne(x => x.Group);
    }
}
