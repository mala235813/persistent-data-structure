using System.Collections.Generic;

namespace PDS.Collections
{
    public interface IPersistentArray<T> : IPersistentDataStructure<T, IPersistentArray<T>>
    {
        int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer);

        IPersistentArray<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer);

        IPersistentArray<T> SetItem(int index, T value);
        
        T this[int index] { get; }
    }
}