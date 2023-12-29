// By ryanholden8
// https://stackoverflow.com/questions/63813872/record-types-with-collection-properties-collections-with-value-semantics

using System.Collections.Immutable;
using System.Collections;

namespace CandyKingdom.Marcy.Immutables;

using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonConverterForImmutableList2Factory))]
public sealed class ImmutableList2<T> : IEquatable<ImmutableList2<T>>, IEnumerable, IEnumerable<T>
{
    private readonly ImmutableList<T> _list;

    public ImmutableList2(ImmutableList<T> list) => _list = list;

        #region ImmutableList Implementation

    public T this[int index] => _list[index];

    public int Count => _list.Count;

    public ImmutableList2<T> Add(T value) => _list.Add(value).WithDeepEquality();

    public ImmutableList2<T> AddRange(IEnumerable<T> items) =>
        _list.AddRange(items).WithDeepEquality();

    public ImmutableList2<T> Clear() => _list.Clear().WithDeepEquality();

    public ImmutableList<T>.Enumerator GetEnumerator() => _list.GetEnumerator();

    public int IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) =>
        _list.IndexOf(item, index, count, equalityComparer);

    public ImmutableList2<T> Insert(int index, T element) =>
        _list.Insert(index, element).WithDeepEquality();

    public ImmutableList2<T> InsertRange(int index, IEnumerable<T> items) =>
        _list.InsertRange(index, items).WithDeepEquality();

    public int LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) =>
        _list.LastIndexOf(item, index, count, equalityComparer);

    public ImmutableList2<T> Remove(T value, IEqualityComparer<T> equalityComparer) =>
        _list.Remove(value, equalityComparer).WithDeepEquality();

    public ImmutableList2<T> RemoveAll(Predicate<T> match) =>
        _list.RemoveAll(match).WithDeepEquality();

    public ImmutableList2<T> RemoveAt(int index) => _list.RemoveAt(index).WithDeepEquality();

    public ImmutableList2<T> RemoveRange(
        IEnumerable<T> items,
        IEqualityComparer<T> equalityComparer
    ) => _list.RemoveRange(items, equalityComparer).WithDeepEquality();

    public ImmutableList2<T> RemoveRange(int index, int count) =>
        _list.RemoveRange(index, count).WithDeepEquality();

    public ImmutableList2<T> Replace(
        T oldValue,
        T newValue,
        IEqualityComparer<T> equalityComparer
    ) => _list.Replace(oldValue, newValue, equalityComparer).WithDeepEquality();

    public ImmutableList2<T> SetItem(int index, T value) =>
        _list.SetItem(index, value).WithDeepEquality();

    public bool IsEmpty => _list.IsEmpty;

    public static ImmutableList2<T> Empty = new(ImmutableList<T>.Empty);

        #endregion

        #region IEnumerable

    IEnumerator IEnumerable.GetEnumerator() => (_list as IEnumerable).GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (_list as IEnumerable<T>).GetEnumerator();

        #endregion

        #region IEquatable

    public bool Equals(ImmutableList2<T> other) => _list.SequenceEqual(other);

    public override bool Equals(object obj) => obj is ImmutableList2<T> other && Equals(other);

    public static bool operator ==(ImmutableList2<T>? left, ImmutableList2<T>? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(ImmutableList2<T>? left, ImmutableList2<T>? right) =>
        !(left == right);

    public override int GetHashCode()
    {
        unchecked
        {
            return _list.Aggregate(19, (h, i) => h * 19 + i!.GetHashCode());
        }
    }

        #endregion
}
