using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using CandyKingdom.Marcy;
using CandyKingdom.MarcyCms.Data;
using CandyKingdom.MarcyCms.Sample.Data;
using CandyKingdom.MarcyCms.Sample.Serialization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms.Sample.Content;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IMarcyCmsDbContext
{
    public DbSet<SettingGroupDb> SettingGroups => Set<SettingGroupDb>();
    public DbSet<SettingDb> SettingRecords => Set<SettingDb>();
    public DbSet<PageDb> Pages => Set<PageDb>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var boneSerializationOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            TypeInfoResolver = new BoneTypeInfoResolver() // needed for inheritance in serialization
        };

        boneSerializationOptions.Converters.Add(new JsonStringEnumConverter());
        boneSerializationOptions.Converters.Add(new JsonConverterForLocalizedObjectFactory());
        boneSerializationOptions.Converters.Add(new JsonConverterForLocalizedString());


        builder.SetupForMarcyCms(boneSerializationOptions);
    }
}
