using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using PDS.Collections;
using PDS.Implementation.Collections;
using PDS.UndoRedo;

namespace PDS.Implementation.UndoRedo
{
    public class UndoRedoLinkedList<T> : IUndoRedoLinkedList<T>
    {
        private readonly IPersistentLinkedList<T> _persistentLinkedList;
        private readonly IPersistentStack<IPersistentLinkedList<T>> _undoStack;
        private readonly IPersistentStack<IPersistentLinkedList<T>> _redoStack;

        public UndoRedoLinkedList()
        {
            _persistentLinkedList = PersistentLinkedList<T>.Empty;
            _undoStack = PersistentStack<IPersistentLinkedList<T>>.Empty;
            _redoStack = PersistentStack<IPersistentLinkedList<T>>.Empty;
        }
        
        private UndoRedoLinkedList(IPersistentLinkedList<T> persistentLinkedList, IPersistentStack<IPersistentLinkedList<T>> undoStack,
            IPersistentStack<IPersistentLinkedList<T>> redoStack)
        {
            _persistentLinkedList = persistentLinkedList;
            _undoStack = undoStack;
            _redoStack = redoStack;
        }

        public IEnumerator<T> GetEnumerator() => _persistentLinkedList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _persistentLinkedList.Count;
        public T First => _persistentLinkedList.First;
        public T Last => _persistentLinkedList.Last;
        IPersistentLinkedList<T> IPersistentLinkedList<T>.AddFirst(T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddFirst(item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> AddLast(T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddLast(item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> RemoveFirst()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveFirst(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> RemoveLast()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveLast(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> Add(T item) => AddLast(item);

        IUndoRedoDataStructure<T, IUndoRedoStack<T>> IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.Add(T value)
        {
            return Add(value);
        }

        IUndoRedoDataStructure<T, IUndoRedoStack<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.AddRange(IEnumerable<T> items) =>
            AddRange(items);

        IUndoRedoDataStructure<T, IUndoRedoStack<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.AddRange(
                IReadOnlyCollection<T> items) => AddRange(items);

        public IUndoRedoLinkedList<T> Clear()
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Clear(), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IUndoRedoStack<T> IUndoRedoStack<T>.Clear() => Clear();

        IUndoRedoStack<T> IUndoRedoStack<T>.Push(T value) => AddLast(value);

        IUndoRedoDataStructure<T, IUndoRedoStack<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.Clear() => Clear();
        
        public IUndoRedoLinkedList<T> AddRange(IEnumerable<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddRange(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> AddRange(IReadOnlyCollection<T> items)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddRange(items), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.Clear() => Clear();

        public bool IsEmpty => _persistentLinkedList.IsEmpty;

        public IUndoRedoLinkedList<T> Insert(int index, T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Insert(index, item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Remove(value, equalityComparer), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> RemoveAll(Predicate<T> match)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveAll(match), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> RemoveAt(int index)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveAt(index), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveRange(items, equalityComparer), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> RemoveRange(int index, int count)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.RemoveRange(index, count), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.Replace(oldValue, newValue, equalityComparer), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> SetItem(int index, T value)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.SetItem(index, value), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        public IUndoRedoLinkedList<T> AddFirst(T item)
        {
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(_persistentLinkedList.AddFirst(item), u, PersistentStack<IPersistentLinkedList<T>>.Empty);
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.AddLast(T item) => AddLast(item);

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveFirst() => RemoveFirst();

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveLast() => RemoveLast();

        public bool Contains(T item) => _persistentLinkedList.Contains(item);

        public T Get(int index) => _persistentLinkedList.Get(index);
        IPersistentLinkedList<T> IPersistentLinkedList<T>.Add(T item) => Add(item);

        IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>>.AddRange(
                IEnumerable<T> items) => AddRange(items);

        IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>>.AddRange(
                IReadOnlyCollection<T> items) => AddRange(items);

        IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>>.Clear() => Clear();

        bool IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>>.IsEmpty => _persistentLinkedList.IsEmpty;

        IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>>.Add(T value) =>
            AddLast(value);

        IPersistentLinkedList<T> IPersistentLinkedList<T>.AddRange(IEnumerable<T> items) => AddRange(items);

        IPersistentLinkedList<T> IPersistentLinkedList<T>.AddRange(IReadOnlyCollection<T> items) => AddRange(items);

        public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) =>
            _persistentLinkedList.IndexOf(item, index, count, equalityComparer);

        IPersistentLinkedList<T> IPersistentLinkedList<T>.Insert(int index, T item) => Insert(index, item);

        IPersistentLinkedList<T> IPersistentLinkedList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer) =>
            Remove(value, equalityComparer);

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveAll(Predicate<T> match) => RemoveAll(match);
        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveAt(int index) => RemoveAt(index);

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveRange(IEnumerable<T> items,
            IEqualityComparer<T>? equalityComparer) => RemoveRange(items, equalityComparer);

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveRange(int index, int count) =>
            RemoveRange(index, count);

        IPersistentLinkedList<T> IPersistentLinkedList<T>.Replace(T oldValue, T newValue,
            IEqualityComparer<T>? equalityComparer) => Replace(oldValue, newValue, equalityComparer);

        IPersistentLinkedList<T> IPersistentLinkedList<T>.SetItem(int index, T value) => SetItem(index, value);

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.Add(T value) => Add(value);

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.AddRange(IEnumerable<T> items) =>
            AddRange(items);

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.AddRange(IReadOnlyCollection<T> items) =>
            AddRange(items);

        IPersistentStack<T> IPersistentStack<T>.Clear() => Clear();

        bool IPersistentStack<T>.IsEmpty => _persistentLinkedList.IsEmpty;

        IUndoRedoStack<T> IUndoRedoStack<T>.Pop() => RemoveLast();

        IPersistentStack<T> IPersistentStack<T>.Push(T value) => AddLast(value);

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.Clear() => Clear();

        T IImmutableStack<T>.Peek() => _persistentLinkedList.Last;

        IPersistentStack<T> IPersistentStack<T>.Pop() => RemoveLast();

        IImmutableStack<T> IImmutableStack<T>.Pop() => RemoveLast();

        IImmutableStack<T> IImmutableStack<T>.Push(T value) => AddLast(value);

        bool IImmutableStack<T>.IsEmpty => _persistentLinkedList.IsEmpty;

        bool IPersistentDataStructure<T, IPersistentStack<T>>.IsEmpty => _persistentLinkedList.IsEmpty;

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.Add(T value) => AddLast(value);

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.AddRange(IEnumerable<T> items) =>
            AddRange(items);

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.AddRange(
            IReadOnlyCollection<T> items) => AddRange(items);

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.Clear() => Clear();

        public IImmutableQueue<T> Dequeue() => RemoveFirst();

        public IImmutableQueue<T> Enqueue(T value) => AddLast(value);

        IImmutableStack<T> IImmutableStack<T>.Clear() => Clear();

        T IImmutableQueue<T>.Peek() => _persistentLinkedList.First;

        bool IImmutableQueue<T>.IsEmpty => _persistentLinkedList.IsEmpty;

        IImmutableQueue<T> IImmutableQueue<T>.Clear() => Clear();

        bool IPersistentDataStructure<T, IPersistentLinkedList<T>>.IsEmpty => _persistentLinkedList.IsEmpty;

        public IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>> Redo()
        {
            if (_redoStack.IsEmpty)
            {
                throw new InvalidOperationException("Redo stack is empty");
            }

            var lastVersion = _redoStack.Peek();
            var r = _redoStack.Pop();
            var u = _undoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(lastVersion, u, r);
        }

        public bool TryRedo(out IUndoRedoDataStructure<T, IUndoRedoStack<T>> collection) => ((IUndoRedoLinkedList<T>)this).TryRedo(out collection);

        public bool TryRedo(out IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>> collection)
        {
            if (_redoStack.IsEmpty)
            {
                collection = this;
                return false;
            }

            collection = Redo();
            return true;
        }

        IUndoRedoDataStructure<T, IUndoRedoStack<T>> IUndoRedo<IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.Redo() => (IUndoRedoDataStructure<T, IUndoRedoStack<T>>)Redo();

        public bool CanRedo => !_redoStack.IsEmpty;
        IUndoRedoDataStructure<T, IUndoRedoStack<T>> IUndoRedo<IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.Undo() => (IUndoRedoDataStructure<T, IUndoRedoStack<T>>)Undo();

        public bool TryUndo(out IUndoRedoDataStructure<T, IUndoRedoStack<T>> collection) => ((IUndoRedoLinkedList<T>)this).TryUndo(out collection);

        public IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>> Undo()
        {
            if (_undoStack.IsEmpty)
            {
                throw new InvalidOperationException("Undo stack is empty");
            }

            var lastVersion = _undoStack.Peek();
            var u = _undoStack.Pop();
            var r = _redoStack.Push(_persistentLinkedList);
            return new UndoRedoLinkedList<T>(lastVersion, u, r);
        }

        public bool TryUndo(out IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>> collection)
        {
            if (_undoStack.IsEmpty)
            {
                collection = this;
                return false;
            }

            collection = Undo();
            return true;
        }

        public bool CanUndo => !_undoStack.IsEmpty;
    }
}