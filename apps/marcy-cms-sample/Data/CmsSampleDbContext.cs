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

        var serializerOptions = CmsJsonSerializationOptions.New();

        builder.SetupForMarcyCms(serializerOptions);
    }
}
