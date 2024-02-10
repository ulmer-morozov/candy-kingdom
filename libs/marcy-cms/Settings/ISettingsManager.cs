using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Immutables;

namespace CandyKingdom.MarcyCms.Settings;

public interface ISettingsManager
{
    public Task<ResultOrError<ImmutableList2<Setting>>> GetSettingsAsync(IEnumerable<Guid> settingIds, CancellationToken cancellationToken = default);
    public Task<ResultOrError> UpdateAsync(IEnumerable<Setting> settings, CancellationToken cancellationToken = default);

    public Task<ResultOrError<ImmutableList2<SettingGroup>>> GetGroupsAsync(IEnumerable<Guid>? ids = null, CancellationToken cancellationToken = default);
}
