using CandyKingdom.MarcyCms.Data;

using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms;

public interface IMarcyCmsDbContext
{
    public DbSet<ViewDb> Views { get; } // todo: use or remove
    public DbSet<PageDb> Pages { get; }
    public DbSet<SettingGroupDb> SettingGroups { get; }
    public DbSet<SettingDb> SettingRecords { get; }
}
