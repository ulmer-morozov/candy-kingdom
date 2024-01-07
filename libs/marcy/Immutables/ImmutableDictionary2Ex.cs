// By ryanholden8
// https://stackoverflow.com/questions/63813872/record-types-with-collection-properties-collections-with-value-semantics

using System.Collections.Immutable;

namespace CandyKingdom.Marcy.Immutables;

public static class ImmutableDictionary2Ex
{
    public static ImmutableDictionary2<TKey, TValue> WithDeepEquality<TKey, TValue>(
      this ImmutableDictionary<TKey, TValue> dictionary
    )
      where TKey : notnull => new(dictionary);

    public static ImmutableDictionary2<TKey, TValue> ToImmutableDictionary2<TKey, TValue>(
      this IEnumerable<KeyValuePair<TKey, TValue>> list
    )
      where TKey : notnull => new(list.ToImmutableDictionary());
}
