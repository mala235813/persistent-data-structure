using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using PDS.Collections;

namespace PDS.Implementation.Collections
{
    public class PersistentStack<T> : IPersistentStack<T>
    {
        private readonly T _value;
        private readonly PersistentStack<T>? _next;

        public int Count { get; }

        public bool IsEmpty => Count == 0;

        public static PersistentStack<T> Empty { get; } = new();

        private PersistentStack(T value, PersistentStack<T>? next, int count)
        {
            _value = value;
            _next = next;
            Count = count;
        }

        public PersistentStack() : this(default, null, 0)
        {
        }
        
        public IPersistentStack<T> Add(T value)
        {
            return Push(value);
        }

        public IPersistentStack<T> AddRange(IEnumerable<T> items)
        {
            IPersistentStack<T> stack = this;
            return items.Aggregate(stack, (current, item) => current.Push(item));
        }

        public IPersistentStack<T> AddRange(IReadOnlyCollection<T> items) => AddRange(items.AsEnumerable());

        public IPersistentStack<T> Clear()
        {
            return Empty;
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Empty stack");
            }

            return _value;
        }

        public IPersistentStack<T> Pop()
        {
            if (IsEmpty || _next is null)
            {
                throw new InvalidOperationException("Empty stack");
            }

            return _next;
        }

        public IPersistentStack<T> Push(T value)
        {
            return new PersistentStack<T>(value, this, Count + 1);
        }
        
        IImmutableStack<T> IImmutableStack<T>.Pop()
        {
            return Pop();
        }

        IImmutableStack<T> IImmutableStack<T>.Push(T value)
        {
            return Push(value);
        }
        
        IImmutableStack<T> IImmutableStack<T>.Clear()
        {
            return Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var stackNode = this;
            while (stackNode != null && !stackNode.IsEmpty)
            {
                yield return stackNode._value;
                stackNode = stackNode._next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}