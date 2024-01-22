using System.Collections.Immutable;
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

public sealed class CmsSampleDbContext : IdentityDbContext<ApplicationUser>, IMarcyCmsDbContext
{
    public DbSet<SettingGroupDb> SettingGroups => Set<SettingGroupDb>();
    public DbSet<SettingDb> SettingRecords => Set<SettingDb>();
    public DbSet<PageDb> Pages => Set<PageDb>();

    public CmsSampleDbContext(DbContextOptions<CmsSampleDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var boneSerializerOptions = CreateDefaultOptions();
        boneSerializerOptions.TypeInfoResolver = new BoneTypeInfoResolver(); // needed for inheritance in serialization

        var settingSerializerOptions = CreateDefaultOptions();
        settingSerializerOptions.TypeInfoResolver = new SettingDataTypeInfoResolver();

        var pageDataSerializerOptions = CreateDefaultOptions();
        pageDataSerializerOptions.TypeInfoResolver = new PageDataInfoResolver();

        var defaultSerializationOptions = CreateDefaultOptions();

        builder.SetupForMarcyCms
        (
            boneSerializerOptions: boneSerializerOptions,
            settingSerializerOptions: settingSerializerOptions,
            pageDataSerializerOptions: pageDataSerializerOptions,
            defaultSerializerOptions: defaultSerializationOptions
        );
    }

    private static JsonSerializerOptions CreateDefaultOptions()
    {
        ImmutableList<JsonConverter> defaultConverters = [
            new JsonStringEnumConverter(),
            new JsonConverterForLocalizedObject<string>(),
            new JsonConverterForLocalizedString()
        ];

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic), // multi language serialization
            TypeInfoResolver = new BoneTypeInfoResolver(), // needed for inheritance in serialization
        };

        defaultConverters.ForEach(options.Converters.Add);

        return options;
    }
}
