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
    public static void HasJsonConversion(this PropertyBuilder<LocalizedString> propertyBuilder)
    {
        propertyBuilder.HasConversion
        (
            v => LocalizedStringHelpers.ToJson(v, null),
            v => LocalizedStringHelpers.FromJson(v, null)
        );
    }

    public static void SetupForMarcyCms(this ModelBuilder builder, JsonSerializerOptions boneSerializerOptions)
    {
        // full query <--> JSON mapping support
        builder
           .Entity<PageDb>()
           .OwnsOne
           (
               page => page.OG,
               builder => builder.ToJson()
           );

        builder
            .Entity<PageDb>()
            .Property(x => x.Title)
            .HasJsonConversion();

        builder
            .Entity<PageDb>()
            .Property(x => x.Bones)
            .HasConversion
            (
                v => JsonSerializer.Serialize(v, boneSerializerOptions),
                v => JsonSerializer.Deserialize<ImmutableList2<Bone>>(v, boneSerializerOptions) ?? ImmutableList2<Bone>.Empty
            );

        builder
            .Entity<SettingGroupDb>()
            .HasMany(x => x.Records)
            .WithOne(x => x.Group);

    }
}
