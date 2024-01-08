using CandyKingdom.MarcyCms.Data;
using CandyKingdom.MarcyCms.Sample.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms.Sample.Content;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IMarcyCmsDbContext
{
    public DbSet<SettingGroupDb> SettingGroups => Set<SettingGroupDb>();
    public DbSet<SettingRecordDb> SettingRecords => Set<SettingRecordDb>();


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.SetupForMarcyCms();
    }
}
