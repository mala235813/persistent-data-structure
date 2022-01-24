using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using PDS.Collections;
using PDS.Implementation.Collections;
using PDS.UndoRedo;

namespace PDS.Implementation.UndoRedo
{
    public class UndoRedoList<T> : IUndoRedoList<T>
    {
        private readonly IPersistentList<T> _persistentList;
        private readonly IPersistentStack<IPersistentList<T>> _undoStack;
        private readonly IPersistentStack<IPersistentList<T>> _redoStack;

        public UndoRedoList()
        {
            //TODO
            _persistentList = null;
            _undoStack = PersistentStack<IPersistentList<T>>.Empty;
            _redoStack = PersistentStack<IPersistentList<T>>.Empty;
        }

        private UndoRedoList(IPersistentList<T> persistentList, IPersistentStack<IPersistentList<T>> undoStack,
            IPersistentStack<IPersistentList<T>> redoStack)
        {
            _persistentList = persistentList;
            _undoStack = undoStack;
            _redoStack = redoStack;
        }

        public IEnumerator<T> GetEnumerator() => _persistentList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _persistentList).GetEnumerator();

        public int Count => _persistentList.Count;

        IPersistentList<T> IPersistentDataStructure<T, IPersistentList<T>>.Add(T value)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.Add(value), u, PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> AddRange(IEnumerable<T> items)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.AddRange(items), u, PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> Clear()
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.Clear(), u, PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> AddRange(IReadOnlyCollection<T> items)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.AddRange(items), u, PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> Insert(int index, T item)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.Insert(index, item), u,
                PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.Remove(value, equalityComparer), u,
                PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> RemoveAll(Predicate<T> match)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.RemoveAll(match), u, PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> RemoveAt(int index)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.RemoveAt(index), u, PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.RemoveRange(items, equalityComparer), u,
                PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> RemoveRange(int index, int count)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.RemoveRange(index, count), u,
                PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.Replace(oldValue, newValue, equalityComparer), u,
                PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> SetItem(int index, T value)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.SetItem(index, value), u,
                PersistentStack<IPersistentList<T>>.Empty);
        }

        public IUndoRedoList<T> Add(T item)
        {
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(_persistentList.Add(item), u, PersistentStack<IPersistentList<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>>.AddRange(
                IEnumerable<T> items) => AddRange(items);

        IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>>.AddRange(
                IReadOnlyCollection<T> items) => AddRange(items);

        IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>>.Clear() => Clear();

        IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>>.Add(T value) => Add(value);

        IPersistentList<T> IPersistentList<T>.AddRange(IEnumerable<T> items) => AddRange(items);

        IPersistentList<T> IPersistentList<T>.Clear() => Clear();

        IPersistentList<T> IPersistentList<T>.AddRange(IReadOnlyCollection<T> items) => AddRange(items);

        int IPersistentList<T>.IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) =>
            _persistentList.IndexOf(item, index, count, equalityComparer);

        IPersistentList<T> IPersistentList<T>.Insert(int index, T item) => Insert(index, item);

        int IPersistentList<T>.LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) =>
            _persistentList.LastIndexOf(item, index, count, equalityComparer);

        IPersistentList<T> IPersistentList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer) =>
            Remove(value, equalityComparer);

        IPersistentList<T> IPersistentList<T>.RemoveAll(Predicate<T> match) => RemoveAll(match);

        IPersistentList<T> IPersistentList<T>.RemoveAt(int index) => RemoveAt(index);

        IPersistentList<T> IPersistentList<T>.
            RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) =>
            RemoveRange(items, equalityComparer);

        IPersistentList<T> IPersistentList<T>.RemoveRange(int index, int count) => RemoveRange(index, count);

        IPersistentList<T> IPersistentList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) =>
            Replace(oldValue, newValue, equalityComparer);

        IPersistentList<T> IPersistentList<T>.SetItem(int index, T value) => SetItem(index, value);

        IPersistentList<T> IPersistentList<T>.Add(T item) => Add(item);

        IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items) => AddRange(items);

        IImmutableList<T> IImmutableList<T>.Clear() => Clear();

        int IImmutableList<T>.IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) =>
            _persistentList.IndexOf(item, index, count, equalityComparer);

        IImmutableList<T> IImmutableList<T>.Insert(int index, T element) => Insert(index, element);

        public IImmutableList<T> InsertRange(int index, IEnumerable<T> items)
        {
            // TODO: add method to interface
            // var u = _undoStack.Push(_persistentList);
            // return new UndoRedoList<T>(_persistentList.InsertRange(index, items), u,
            //     PersistentStack<IPersistentList<T>>.Empty);
            return _persistentList.InsertRange(index, items);
        }

        int IImmutableList<T>.LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) =>
            _persistentList.LastIndexOf(item, index, count, equalityComparer);

        IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer) =>
            Remove(value, equalityComparer);

        IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) => RemoveAll(match);

        IImmutableList<T> IImmutableList<T>.RemoveAt(int index) => RemoveAt(index);

        IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) =>
            RemoveRange(items, equalityComparer);

        IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count) => RemoveRange(index, count);

        IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) =>
            Replace(oldValue, newValue, equalityComparer);

        IImmutableList<T> IImmutableList<T>.SetItem(int index, T value) => SetItem(index, value);

        IImmutableList<T> IImmutableList<T>.Add(T value) => Add(value);

        IPersistentList<T> IPersistentDataStructure<T, IPersistentList<T>>.AddRange(IEnumerable<T> items) =>
            AddRange(items);

        IPersistentList<T> IPersistentDataStructure<T, IPersistentList<T>>.AddRange(IReadOnlyCollection<T> items) =>
            AddRange(items);

        IPersistentList<T> IPersistentDataStructure<T, IPersistentList<T>>.Clear() => Clear();

        public bool IsEmpty => _persistentList.IsEmpty;

        public T this[int index] => _persistentList[index];

        public IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>> Redo()
        {
            if (_redoStack.IsEmpty)
            {
                throw new InvalidOperationException("Redo stack is empty");
            }

            var lastVersion = _redoStack.Peek();
            var r = _redoStack.Pop();
            var u = _undoStack.Push(_persistentList);
            return new UndoRedoList<T>(lastVersion, u, r);
        }

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

        public bool CanRedo => !_redoStack.IsEmpty;

        public IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>> Undo()
        {
            if (_undoStack.IsEmpty)
            {
                throw new InvalidOperationException("Undo stack is empty");
            }

            var lastVersion = _undoStack.Peek();
            var u = _undoStack.Pop();
            var r = _redoStack.Push(_persistentList);
            return new UndoRedoList<T>(lastVersion, u, r);
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