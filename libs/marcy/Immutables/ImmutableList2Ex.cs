// By ryanholden8
// https://stackoverflow.com/questions/63813872/record-types-with-collection-properties-collections-with-value-semantics

using System.Collections.Immutable;

namespace CandyKingdom.Marcy.Immutables;
public static class ImmutableList2Ex
{
    public static ImmutableList2<T> WithDeepEquality<T>(this ImmutableList<T> list)
    {
        return new(list);
    }

    public static ImmutableList2<T> ToImmutableList2<T>(this IEnumerable<T> list)
    {
        return new(list.ToImmutableList());
    }
}
