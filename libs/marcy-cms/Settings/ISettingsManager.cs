using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;
using CandyKingdom.MarcyCms.Settings;

namespace CandyKingdom.MarcyCms.Sample.Core;

public interface ISettingsManager
{
    public Task<ResultOrError> UpdateAsync(IEnumerable<Setting> settings, CancellationToken cancellationToken = default);

    public Task<ResultOrError<ImmutableList2<SettingGroup>>> GetGroupsAsync(IEnumerable<Guid>? ids, CancellationToken cancellationToken = default);

    public Task<ResultOrError> StoreAsync(SettingGroup group, bool deep = false, bool @override = true, CancellationToken cancellationToken = default);
}
