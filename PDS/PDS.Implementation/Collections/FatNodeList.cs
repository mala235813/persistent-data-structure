using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using PDS.Collections;

namespace PDS.Implementation.Collections
{
    public class FatNodeList<T> : IPersistentList<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; }
        
        IPersistentList<T> IPersistentDataStructure<T, IPersistentList<T>>.Add(T value)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentList<T>.AddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        int IPersistentList<T>.IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentList<T>.Insert(int index, T element)
        {
            throw new NotImplementedException();
        }

        int IPersistentList<T>.LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentList<T>.RemoveAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentList<T>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentList<T>.RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentList<T>.SetItem(int index, T value)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.Clear()
        {
            throw new NotImplementedException();
        }

        int IImmutableList<T>.IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.Insert(int index, T element)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        int IImmutableList<T>.LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.SetItem(int index, T value)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentDataStructure<T, IPersistentList<T>>.AddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.Add(T value)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentDataStructure<T, IPersistentList<T>>.Clear()
        {
            throw new NotImplementedException();
        }

        public T this[int index] => throw new NotImplementedException();
    }
}