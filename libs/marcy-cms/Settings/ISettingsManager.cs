using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.MarcyCms.Settings;

public interface ISettingsManager
{
    public Task<ResultOrError> UpdateAsync(IEnumerable<Setting> settings, CancellationToken cancellationToken = default);

    public Task<ResultOrError<ImmutableList2<SettingGroup>>> GetGroupsAsync(IEnumerable<Guid>? ids = null, CancellationToken cancellationToken = default);

    public Task<ResultOrError> StoreAsync(SettingGroup group, bool deep = false, bool @override = true, CancellationToken cancellationToken = default);
}
