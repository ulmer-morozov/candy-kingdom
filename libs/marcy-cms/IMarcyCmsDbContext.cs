using CandyKingdom.MarcyCms.Data;
using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms;

public interface IMarcyCmsDbContext
{
    // public DbSet<PageDb> Pages { get; }
    public DbSet<SettingGroupDb> SettingGroups { get; }
    public DbSet<SettingRecordDb> SettingRecords { get; }
}
