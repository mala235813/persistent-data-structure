using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using PDS.Collections;
using PDS.Implementation.Collections;
using PDS.Transactional;

namespace PDS.Implementation.Transactional
{
    public class TransactionalStack<T> : ITransactionalStack<T>
    {
        private readonly IPersistentStack<T> _persistentStack;
        private readonly IPersistentStack<IPersistentStack<T>> _undoStack;
        private readonly IPersistentStack<IPersistentStack<T>> _redoStack;

        public TransactionalStack()
        {
            _persistentStack = PersistentStack<T>.Empty;
            _undoStack = PersistentStack<IPersistentStack<T>>.Empty;
            _redoStack = PersistentStack<IPersistentStack<T>>.Empty;
        }

        private TransactionalStack(IPersistentStack<T> persistentStack, IPersistentStack<IPersistentStack<T>> undoStack,
            IPersistentStack<IPersistentStack<T>> redoStack)
        {
            _persistentStack = persistentStack;
            _undoStack = undoStack;
            _redoStack = redoStack;
        }

        public IEnumerator<T> GetEnumerator() => _persistentStack.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _persistentStack.Count;

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.Add(T value)
        {
            throw new System.NotImplementedException();
        }

        ITransactionalDataStructure<T, ITransactionalStack<T>>
            IPersistentDataStructure<T, ITransactionalDataStructure<T, ITransactionalStack<T>>>.AddRange(
                IEnumerable<T> items)
        {
            throw new System.NotImplementedException();
        }

        ITransactionalDataStructure<T, ITransactionalStack<T>> IPersistentDataStructure<T, ITransactionalDataStructure<T, ITransactionalStack<T>>>.AddRange(IReadOnlyCollection<T> items)
        {
            throw new NotImplementedException();
        }

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.AddRange(IReadOnlyCollection<T> items)
        {
            throw new NotImplementedException();
        }

        ITransactionalStack<T> ITransactionalStack<T>.Clear()
        {
            var u = _undoStack.Push(_persistentStack);
            return new TransactionalStack<T>(_persistentStack.Clear(), u, PersistentStack<IPersistentStack<T>>.Empty);
        }

        ITransactionalStack<T> ITransactionalStack<T>.Push(T value)
        {
            throw new System.NotImplementedException();
        }

        ITransactionalDataStructure<T, ITransactionalStack<T>>
            IPersistentDataStructure<T, ITransactionalDataStructure<T, ITransactionalStack<T>>>.Clear()
        {
            throw new System.NotImplementedException();
        }

        ITransactionalDataStructure<T, ITransactionalStack<T>>
            IPersistentDataStructure<T, ITransactionalDataStructure<T, ITransactionalStack<T>>>.Add(T value)
        {
            throw new System.NotImplementedException();
        }

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.AddRange(IEnumerable<T> items)
        {
            throw new System.NotImplementedException();
        }

        IPersistentStack<T> IPersistentStack<T>.Clear()
        {
            throw new System.NotImplementedException();
        }

        ITransactionalStack<T> ITransactionalStack<T>.Pop()
        {
            throw new System.NotImplementedException();
        }

        IPersistentStack<T> IPersistentStack<T>.Push(T value)
        {
            throw new System.NotImplementedException();
        }

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.Clear()
        {
            throw new System.NotImplementedException();
        }

        public T Peek()
        {
            throw new System.NotImplementedException();
        }

        IPersistentStack<T> IPersistentStack<T>.Pop()
        {
            throw new System.NotImplementedException();
        }

        IImmutableStack<T> IImmutableStack<T>.Pop()
        {
            throw new System.NotImplementedException();
        }

        IImmutableStack<T> IImmutableStack<T>.Push(T value)
        {
            throw new System.NotImplementedException();
        }

        public bool IsEmpty => _persistentStack.IsEmpty;

        IImmutableStack<T> IImmutableStack<T>.Clear()
        {
            throw new System.NotImplementedException();
        }

        public ITransactionalDataStructure<T, ITransactionalStack<T>> Undo()
        {
            if (_undoStack.IsEmpty)
            {
                throw new InvalidOperationException("Undo stack is empty");
            }

            var lastVersion = _undoStack.Peek();
            var u = _undoStack.Pop();
            var r = _redoStack.Push(_persistentStack);
            return new TransactionalStack<T>(lastVersion, u, r);
        }

        public ITransactionalDataStructure<T, ITransactionalStack<T>> Redo()
        {
            if (_redoStack.IsEmpty)
            {
                throw new InvalidOperationException("Redo stack is empty");
            }

            var lastVersion = _redoStack.Peek();
            var r = _redoStack.Pop();
            var u = _undoStack.Push(_persistentStack);
            return new TransactionalStack<T>(lastVersion, u, r);
        }
    }
}