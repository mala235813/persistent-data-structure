using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using PDS.Collections;
using PDS.Implementation.Collections;
using PDS.UndoRedo;

namespace PDS.Implementation.UndoRedo
{
    public class UndoRedoStack<T> : IUndoRedoStack<T>
    {
        private readonly IPersistentStack<T> _persistentStack;
        private readonly IPersistentStack<IPersistentStack<T>> _undoStack;
        private readonly IPersistentStack<IPersistentStack<T>> _redoStack;

        public UndoRedoStack()
        {
            _persistentStack = PersistentStack<T>.Empty;
            _undoStack = PersistentStack<IPersistentStack<T>>.Empty;
            _redoStack = PersistentStack<IPersistentStack<T>>.Empty;
        }

        private UndoRedoStack(IPersistentStack<T> persistentStack, IPersistentStack<IPersistentStack<T>> undoStack,
            IPersistentStack<IPersistentStack<T>> redoStack)
        {
            _persistentStack = persistentStack;
            _undoStack = undoStack;
            _redoStack = redoStack;
        }

        public IEnumerator<T> GetEnumerator() => _persistentStack.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _persistentStack.Count;

        public IPersistentStack<T> Add(T value)
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Add(value), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoStack<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.AddRange(
                IEnumerable<T> items)
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.AddRange(items), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoStack<T>> 
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.AddRange(IReadOnlyCollection<T> items)
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.AddRange(items), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.AddRange(IReadOnlyCollection<T> items)
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.AddRange(items), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IUndoRedoStack<T> IUndoRedoStack<T>.Clear()
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Clear(), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IUndoRedoStack<T> IUndoRedoStack<T>.Push(T value)
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Push(value), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoStack<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.Clear()
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Clear(), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IUndoRedoDataStructure<T, IUndoRedoStack<T>>
            IPersistentDataStructure<T, IUndoRedoDataStructure<T, IUndoRedoStack<T>>>.Add(T value)
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Add(value), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.AddRange(IEnumerable<T> items)
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.AddRange(items), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IPersistentStack<T> IPersistentStack<T>.Clear()
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Clear(), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IUndoRedoStack<T> IUndoRedoStack<T>.Pop()
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Pop(), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IPersistentStack<T> IPersistentStack<T>.Push(T value)
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Push(value), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.Clear()
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Clear(), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        public T Peek() => _persistentStack.Peek();

        IPersistentStack<T> IPersistentStack<T>.Pop()
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Pop(), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IImmutableStack<T> IImmutableStack<T>.Pop()
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Pop(), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        IImmutableStack<T> IImmutableStack<T>.Push(T value)
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Push(value), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        public bool IsEmpty => _persistentStack.IsEmpty;

        IImmutableStack<T> IImmutableStack<T>.Clear()
        {
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(_persistentStack.Clear(), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        public IUndoRedoDataStructure<T, IUndoRedoStack<T>> Undo()
        {
            if (_undoStack.IsEmpty)
            {
                throw new InvalidOperationException("Undo stack is empty");
            }

            var lastVersion = _undoStack.Peek();
            var u = _undoStack.Pop();
            var r = _redoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(lastVersion, u, r);
        }

        public bool TryUndo(out IUndoRedoDataStructure<T, IUndoRedoStack<T>> collection)
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

        public IUndoRedoDataStructure<T, IUndoRedoStack<T>> Redo()
        {
            if (_redoStack.IsEmpty)
            {
                throw new InvalidOperationException("Redo stack is empty");
            }

            var lastVersion = _redoStack.Peek();
            var r = _redoStack.Pop();
            var u = _undoStack.Push(_persistentStack);
            return new UndoRedoStack<T>(lastVersion, u, r);
        }

        public bool TryRedo(out IUndoRedoDataStructure<T, IUndoRedoStack<T>> collection)
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
    }
}