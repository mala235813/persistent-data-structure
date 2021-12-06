using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using PDS.Collections;

namespace PDS.Implementation.Collections
{
    public class FatNodeLinkedList<T> : IPersistentLinkedList<T>
    {
        private bool _isEmpty;
        private int _count;

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _count;
        
        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.Add(T value)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<T> AddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.Clear()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<T> Insert(int index, T element)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<T> RemoveAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<T> RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<T> RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        public IImmutableList<T> SetItem(int index, T value)
        {
            throw new NotImplementedException();
        }

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.AddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.Add(T value)
        {
            throw new NotImplementedException();
        }

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.Clear()
        {
            throw new NotImplementedException();
        }

        T IImmutableStack<T>.Peek()
        {
            throw new NotImplementedException();
        }

        public IImmutableStack<T> Pop()
        {
            throw new NotImplementedException();
        }

        public IImmutableStack<T> Push(T value)
        {
            throw new NotImplementedException();
        }

        bool IImmutableStack<T>.IsEmpty => _isEmpty1;

        public IImmutableQueue<T> Dequeue()
        {
            throw new NotImplementedException();
        }

        public IImmutableQueue<T> Enqueue(T value)
        {
            throw new NotImplementedException();
        }

        IImmutableStack<T> IImmutableStack<T>.Clear()
        {
            throw new NotImplementedException();
        }

        T IImmutableQueue<T>.Peek()
        {
            throw new NotImplementedException();
        }

        bool IImmutableQueue<T>.IsEmpty => _isEmpty;

        IImmutableQueue<T> IImmutableQueue<T>.Clear()
        {
            throw new NotImplementedException();
        }

        public T this[int index] => throw new NotImplementedException();
    }
}