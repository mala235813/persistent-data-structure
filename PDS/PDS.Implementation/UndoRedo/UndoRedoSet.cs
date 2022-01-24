using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using PDS.Collections;
using PDS.Implementation.Collections;
using PDS.UndoRedo;

namespace PDS.Implementation.UndoRedo
{
    public class UndoRedoSet<T> : IUndoRedoSet<T>
        where T : notnull
    {
        private readonly IPersistentSet<T> _persistentSet;
        private readonly IPersistentStack<IPersistentSet<T>> _undoStack;
        private readonly IPersistentStack<IPersistentSet<T>> _redoStack;

        public UndoRedoSet()
        {
            _persistentSet = PersistentSet<T>.Empty;
            _undoStack = PersistentStack<IPersistentSet<T>>.Empty;
            _redoStack = PersistentStack<IPersistentSet<T>>.Empty;
        }
        
        private UndoRedoSet(IPersistentSet<T> persistentLinkedList, IPersistentStack<IPersistentSet<T>> undoStack,
            IPersistentStack<IPersistentSet<T>> redoStack)
        {
            _persistentSet = persistentLinkedList;
            _undoStack = undoStack;
            _redoStack = redoStack;
        }



        public IEnumerator<T> GetEnumerator() => _persistentSet.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _persistentSet.Count;
        
        IPersistentSet<T> IPersistentDataStructure<T, IPersistentSet<T>>.Add(T value)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Add(value), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IUndoRedoSet<T> IUndoRedoSet<T>.Clear()
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Clear(), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IUndoRedoSet<T> IUndoRedoSet<T>.Except(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Except(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IUndoRedoSet<T> IUndoRedoSet<T>.Intersect(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Intersect(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IUndoRedoSet<T> IUndoRedoSet<T>.Remove(T value)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Remove(value), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IUndoRedoSet<T> IUndoRedoSet<T>.SymmetricExcept(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.SymmetricExcept(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IUndoRedoSet<T> IUndoRedoSet<T>.Union(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Union(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IUndoRedoSet<T> IUndoRedoSet<T>.Add(T value)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Add(value), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoSet<T>> 
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoSet<T>>>.AddRange(IEnumerable<T> items)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.AddRange(items), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoSet<T>> 
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoSet<T>>>.AddRange(IReadOnlyCollection<T> items)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.AddRange(items), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoSet<T>> 
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoSet<T>>>.Clear()
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Clear(), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IPersistentSet<T> IPersistentSet<T>.Clear()
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Clear(), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IPersistentSet<T> IPersistentSet<T>.Except(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Except(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IPersistentSet<T> IPersistentSet<T>.Intersect(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Intersect(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IPersistentSet<T> IPersistentSet<T>.Remove(T value)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Remove(value), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IPersistentSet<T> IPersistentSet<T>.SymmetricExcept(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.SymmetricExcept(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IPersistentSet<T> IPersistentSet<T>.Union(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Union(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IPersistentSet<T> IPersistentSet<T>.Add(T value)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Add(value), u, PersistentStack<IPersistentSet<T>>.Empty);;
        }

        IImmutableSet<T> IImmutableSet<T>.Clear()
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Clear(), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        bool IImmutableSet<T>.Contains(T value) => ((IImmutableSet<T>)_persistentSet).Contains(value);

        bool IReadOnlySet<T>.IsProperSubsetOf(IEnumerable<T> other) => ((IReadOnlySet<T>)_persistentSet).IsProperSubsetOf(other);

        bool IReadOnlySet<T>.IsProperSupersetOf(IEnumerable<T> other) => ((IReadOnlySet<T>)_persistentSet).IsProperSupersetOf(other);

        bool IReadOnlySet<T>.IsSubsetOf(IEnumerable<T> other) => ((IReadOnlySet<T>)_persistentSet).IsSubsetOf(other);

        bool IReadOnlySet<T>.IsSupersetOf(IEnumerable<T> other) => ((IReadOnlySet<T>)_persistentSet).IsSupersetOf(other);

        bool IReadOnlySet<T>.Overlaps(IEnumerable<T> other) => ((IReadOnlySet<T>)_persistentSet).Overlaps(other);

        bool IReadOnlySet<T>.SetEquals(IEnumerable<T> other) => ((IReadOnlySet<T>)_persistentSet).SetEquals(other);

        IImmutableSet<T> IImmutableSet<T>.Except(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Except(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IImmutableSet<T> IImmutableSet<T>.Intersect(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Intersect(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        bool IReadOnlySet<T>.Contains(T item) => ((IReadOnlySet<T>)_persistentSet).Contains(item);
        
        bool IImmutableSet<T>.IsProperSubsetOf(IEnumerable<T> other) => 
            ((IImmutableSet<T>)_persistentSet).IsProperSubsetOf(other);

        bool IImmutableSet<T>.IsProperSupersetOf(IEnumerable<T> other) => 
            ((IImmutableSet<T>)_persistentSet).IsProperSupersetOf(other);

        bool IImmutableSet<T>.IsSubsetOf(IEnumerable<T> other) => 
            ((IImmutableSet<T>)_persistentSet).IsSubsetOf(other);

        bool IImmutableSet<T>.IsSupersetOf(IEnumerable<T> other) => 
            ((IImmutableSet<T>)_persistentSet).IsSupersetOf(other);

        bool IImmutableSet<T>.Overlaps(IEnumerable<T> other) => 
            ((IImmutableSet<T>)_persistentSet).Overlaps(other);

        IImmutableSet<T> IImmutableSet<T>.Remove(T value)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Remove(value), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        bool IImmutableSet<T>.SetEquals(IEnumerable<T> other) => ((IImmutableSet<T>)_persistentSet).SetEquals(other);

        IImmutableSet<T> IImmutableSet<T>.SymmetricExcept(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.SymmetricExcept(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        public bool TryGetValue(T equalValue, out T actualValue) =>
            _persistentSet.TryGetValue(equalValue, out actualValue);

        IImmutableSet<T> IImmutableSet<T>.Union(IEnumerable<T> other)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Union(other), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoSet<T>> 
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoSet<T>>>.Add(T value)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Add(value), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IPersistentSet<T> IPersistentDataStructure<T, IPersistentSet<T>>.AddRange(IEnumerable<T> items)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.AddRange(items), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IPersistentSet<T> IPersistentDataStructure<T, IPersistentSet<T>>.AddRange(IReadOnlyCollection<T> items)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.AddRange(items), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IImmutableSet<T> IImmutableSet<T>.Add(T value)
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Add(value), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        IPersistentSet<T> IPersistentDataStructure<T, IPersistentSet<T>>.Clear()
        {
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(_persistentSet.Clear(), u, PersistentStack<IPersistentSet<T>>.Empty);
        }

        public bool IsEmpty => _persistentSet.IsEmpty;
        
        public IUndoRedoDataStructure<T, IUndoRedoSet<T>> Redo()
        {
            if (_redoStack.IsEmpty)
            {
                throw new InvalidOperationException("Redo stack is empty");
            }

            var lastVersion = _redoStack.Peek();
            var r = _redoStack.Pop();
            var u = _undoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(lastVersion, u, r);
        }

        public bool TryRedo(out IUndoRedoDataStructure<T, IUndoRedoSet<T>> collection)
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
        
        
        public IUndoRedoDataStructure<T, IUndoRedoSet<T>> Undo()
        {
            if (_undoStack.IsEmpty)
            {
                throw new InvalidOperationException("Undo stack is empty");
            }

            var lastVersion = _undoStack.Peek();
            var u = _undoStack.Pop();
            var r = _redoStack.Push(_persistentSet);
            return new UndoRedoSet<T>(lastVersion, u, r);
        }

        public bool TryUndo(out IUndoRedoDataStructure<T, IUndoRedoSet<T>> collection)
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