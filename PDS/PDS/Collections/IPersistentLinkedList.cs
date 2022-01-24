using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PDS.Collections
{
    public interface IPersistentLinkedList<T> : IPersistentDataStructure<T, IPersistentLinkedList<T>>,
        IImmutableQueue<T>, IPersistentStack<T>
    {
        T First { get; }
        
        T Last { get; }

        IPersistentLinkedList<T> AddFirst(T item);
        
        IPersistentLinkedList<T> AddLast(T item);

        IPersistentLinkedList<T> RemoveFirst();
        
        IPersistentLinkedList<T> RemoveLast();

        bool Contains(T item);
        
        new bool IsEmpty { get; }

        T Get(int index);

        new IPersistentLinkedList<T> Add(T item);
        
        new IPersistentLinkedList<T> AddRange(IEnumerable<T> items);
        
        new IPersistentLinkedList<T> AddRange(IReadOnlyCollection<T> items);

        int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer);

        IPersistentLinkedList<T> Insert(int index, T item);

        IPersistentLinkedList<T> Remove(T value, IEqualityComparer<T>? equalityComparer);

        IPersistentLinkedList<T> RemoveAll(Predicate<T> match);

        IPersistentLinkedList<T> RemoveAt(int index);

        IPersistentLinkedList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer);

        IPersistentLinkedList<T> RemoveRange(int index, int count);

        IPersistentLinkedList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer);

        IPersistentLinkedList<T> SetItem(int index, T value);

        new IPersistentLinkedList<T> Clear();
    }
}