using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms.Data;

public interface IMarcyCmsDbContext
{
    public DbSet<ViewDb> Views { get; }
    public DbSet<PageDb> Pages { get; }
    public DbSet<SettingGroupDb> SettingGroups { get; }
    public DbSet<SettingDb> SettingRecords { get; }
}
