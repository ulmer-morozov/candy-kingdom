// By ryanholden8
// https://stackoverflow.com/questions/63813872/record-types-with-collection-properties-collections-with-value-semantics

using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace CandyKingdom.Marcy.Immutables;
[JsonConverter(typeof(JsonConverterForImmutableDictionary2Factory))]
public sealed class ImmutableDictionary2<TKey, TValue>
  : IEquatable<ImmutableDictionary2<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable,
    IReadOnlyCollection<KeyValuePair<TKey, TValue>>
  where TKey : notnull
{
    private readonly ImmutableDictionary<TKey, TValue> _dictionary;

    public ImmutableDictionary2(ImmutableDictionary<TKey, TValue> dictionary)
    {
        _dictionary = dictionary;
    }

    #region ImmutableArray Implementation

    public TValue this[TKey index] => _dictionary[index];

    public int Count => _dictionary.Count;

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public IEnumerable<TKey> Keys => _dictionary.Keys;

    public ImmutableDictionary2<TKey, TValue> Add(TKey key, TValue value)
    {
        return _dictionary.Add(key, value).WithDeepEquality();
    }

    public ImmutableDictionary2<TKey, TValue> AddRange(
      IEnumerable<KeyValuePair<TKey, TValue>> pairs
    )
    {
        return _dictionary.AddRange(pairs).WithDeepEquality();
    }

    public ImmutableDictionary2<TKey, TValue> Clear()
    {
        return _dictionary.Clear().WithDeepEquality();
    }

    public ImmutableDictionary<TKey, TValue>.Enumerator GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    public ImmutableDictionary2<TKey, TValue> Remove(TKey key)
    {
        return _dictionary.Remove(key).WithDeepEquality();
    }

    public ImmutableDictionary2<TKey, TValue> RemoveRange(IEnumerable<TKey> keys)
    {
        return _dictionary.RemoveRange(keys).WithDeepEquality();
    }

    public ImmutableDictionary2<TKey, TValue> SetItem(TKey key, TValue value)
    {
        return _dictionary.SetItem(key, value).WithDeepEquality();
    }

    public ImmutableDictionary<TKey, TValue> AsImmutableDictionary()
    {
        return _dictionary;
    }

    public bool IsEmpty => _dictionary.IsEmpty;

    public static ImmutableDictionary2<TKey, TValue> Empty =
      new(ImmutableDictionary<TKey, TValue>.Empty);

    #endregion

    #region IEnumerable

    IEnumerator IEnumerable.GetEnumerator()
    {
        return (_dictionary as IEnumerable).GetEnumerator();
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
        return (_dictionary as IEnumerable<KeyValuePair<TKey, TValue>>).GetEnumerator();
    }

    #endregion

    #region IEquatable

    public bool Equals(ImmutableDictionary2<TKey, TValue> other)
    {
        return _dictionary.SequenceEqual(other);
    }

    public override bool Equals(object obj)
    {
        return obj is ImmutableDictionary2<TKey, TValue> other && Equals(other);
    }

    public static bool operator ==(
      ImmutableDictionary2<TKey, TValue>? left,
      ImmutableDictionary2<TKey, TValue>? right
    ) => left is null ? right is null : right is not null && left.Equals(right);

    public static bool operator !=(
      ImmutableDictionary2<TKey, TValue>? left,
      ImmutableDictionary2<TKey, TValue>? right
    ) => !(left == right);

    public override int GetHashCode()
    {
        unchecked
        {
            return _dictionary.Aggregate(
              19,
              (h, i) => (h * 19) + i.Key.GetHashCode() + (i.Value?.GetHashCode() ?? 0)
            );
        }
    }

    #endregion
}
