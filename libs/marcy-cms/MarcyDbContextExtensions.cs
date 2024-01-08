using CandyKingdom.MarcyCms.Data;
using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms;

public static class MarcyDbContextExtensions
{
    public static void SetupForMarcyCms(this ModelBuilder builder)
    {
        // builder
        //     .Entity<PageDb>()
        //     .Property(x => x.ColorTheme)
        //     .HasConversion(
        //         v => JsonConvert.SerializeObject(v, ApplicationJsonSerializer.Settings),
        //         v =>
        //             JsonConvert.DeserializeObject<ColorTheme>(
        //                 v,
        //                 ApplicationJsonSerializer.Settings
        //             )
        //     );

        // builder
        //     .Entity<PageDb>()
        //     .Property(x => x.Bones)
        //     .HasConversion(
        //         v => JsonConvert.SerializeObject(v, ApplicationJsonSerializer.Settings),
        //         v =>
        //             JsonConvert.DeserializeObject<ImmutableList<Bone>>(
        //                 v,
        //                 ApplicationJsonSerializer.Settings
        //             )
        //     );
    }

    // private ProjectPreview DeserializeProjectSettings(string v)
    // {
    //     if (string.IsNullOrWhiteSpace(v))
    //         return ProjectPreview.EmptyWithImage;

    //     var result = JsonConvert.DeserializeObject<ProjectPreview>(
    //         v,
    //         ApplicationJsonSerializer.Settings
    //     );
    //     return result;
    // }
}
