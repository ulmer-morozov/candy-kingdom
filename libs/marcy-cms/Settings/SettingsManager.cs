using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms.Settings;

public sealed class SettingsManager<TDbContext> : ISettingsManager
where TDbContext : DbContext, IMarcyCmsDbContext
{
    private readonly IDbContextFactory<TDbContext> _contextFactory;

    public SettingsManager(IDbContextFactory<TDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public Task<ResultOrError> UpdateAsync(IEnumerable<Setting> settings, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResultOrError<ImmutableList2<SettingGroup>>> GetGroupsAsync(IEnumerable<Guid>? ids = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResultOrError> StoreAsync(SettingGroup group, bool deep = false, bool @override = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
