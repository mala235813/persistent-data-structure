using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using PDS.Collections;

namespace PDS.Implementation.Collections
{
    public class PersistentList<T> : IPersistentList<T>
    {
        private const int BranchingFactor = 32;
        private const int BitMask = BranchingFactor - 1;
        private static readonly int Shift = CountBits(BitMask);

        private readonly int _shift;
        private readonly Node<T> _root;
        private readonly Node<T> _tail;
        private readonly int _tailOffset;

        public PersistentList()
        {
            Count = 0;
            _shift = 5;
            _root = new Node<T>();
            _tail = new Node<T>();
            _tailOffset = 0;
        }

        private PersistentList(int count, int shift, Node<T> root, Node<T> tail)
        {
            Count = count;
            _shift = shift;
            _root = root;
            _tail = tail;
            _tailOffset = count - _tail.Count;
        }

        public int Count { get; }

        public T Get(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    $"Index out of range: {index} should be greater then 0 and less then {Count}");
            }

            return GetNode(index).Value[index & BitMask];
        }

        public PersistentList<T> Set(int index, T value)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(
                    $"Index out of range: {index} should be greater then 0 and less then {Count}");
            }

            if (index == Count)
            {
                return Add(value);
            }

            if (index >= _tailOffset)
            {
                var newTail = _tail.Clone();
                newTail.Value[index & BitMask] = value;

                return new PersistentList<T>(Count, _shift, _root, newTail);
            }

            return new PersistentList<T>(Count, _shift, Set(_shift, _root, index, value), _tail);
        }

        public PersistentList<T> Add(T value)
        {
            Node<T> newTail;
            if (_tail.Value.Count < BranchingFactor)
            {
                newTail = _tail.Clone();
                newTail.Value.Add(value);
                return new PersistentList<T>(Count + 1, _shift, _root, newTail);
            }

            var (newRoot, newShift) = CreateNewRoot();
            newTail = new Node<T>();
            newTail.Value.Add(value);

            return new PersistentList<T>(Count + 1, newShift, newRoot, newTail);
        }

        public PersistentList<T> AddRange(IEnumerable<T> items)
        {
            if (items is not IReadOnlyCollection<T> collection)
            {
                collection = items.ToArray();
            }

            return AddRange(collection);
        }

        public PersistentList<T> AddRange(IReadOnlyCollection<T> items)
        {
            return items.Count == 0 ? this : new PersistentList<T>(this, items);
        }

        private PersistentList(PersistentList<T> oldVersion, IReadOnlyCollection<T> items)
        {
            using var enumerator = items.GetEnumerator();
            enumerator.MoveNext();

            var newList = oldVersion.Add(enumerator.Current);

            Count = newList.Count;
            _shift = newList._shift;
            _root = newList._root;
            _tail = newList._tail;
            _tailOffset = newList._tailOffset;

            var offset = 0;
            var count = items.Count - 1;
            while (offset < count)
            {
                var maxDelta = BranchingFactor - _tail.Count;
                var delta = Math.Min(maxDelta, count - offset);
                for (var i = 0; i < maxDelta; i++)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }

                    _tail.Value.Add(enumerator.Current);
                }

                Count += delta;
                offset += delta;

                if (_tail.Count == BranchingFactor)
                {
                    (_root, _shift) = CreateNewRoot();
                    _tail = new Node<T>();
                }
            }

            _tailOffset = Count - _tail.Count;
        }

        public PersistentList<T> Remove(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    $"Index out of range: {index} should be greater then 0 and less then {Count}");
            }

            if (index >= _tailOffset)
            {
                var newTail = _tail.Clone();
                newTail.Value.RemoveAt(index & BitMask);

                return new PersistentList<T>(Count, _shift, _root, newTail);
            }

            return new PersistentList<T>(Count, _shift, Remove(_shift, _root, index), _tail);
        }


        private (Node<T> newRoot, int newShift) CreateNewRoot()
        {
            var newShift = _shift;
            Node<T> newRoot;
            if ((Count >> Shift) > (1 << _shift))
            {
                newRoot = new Node<T>();
                newRoot.Child.Add(_root);
                newRoot.Child.Add(NewPath(_shift, _tail));

                newShift += Shift;
            }
            else
            {
                newRoot = PushTail(_shift, _root, _tail);
            }

            return (newRoot, newShift);
        }

        private Node<T> PushTail(int level, Node<T> parent, Node<T> tail)
        {
            var newNode = parent.Clone();
            if (level == Shift)
            {
                newNode.Child.Add(tail);
                return newNode;
            }

            var subIndex = ((Count - 1) >> level) & BitMask;
            if (parent.Count > subIndex)
            {
                newNode.Child[subIndex] = PushTail(level - Shift, parent.Child[subIndex], tail);
            }
            else
            {
                newNode.Child.Add(NewPath(level - Shift, tail));
            }

            return newNode;
        }

        private Node<T> NewPath(int level, Node<T> node)
        {
            if (level == 0)
            {
                return node;
            }

            var newBranch = new Node<T>();
            newBranch.Child.Add(NewPath(level - Shift, node));
            return newBranch;
        }

        private Node<T> GetNode(int index)
        {
            if (index >= _tailOffset)
            {
                return _tail;
            }

            var node = _root;
            for (var level = _shift; level > 0; level -= Shift)
            {
                node = node.Child[(index >> level) & BitMask];
            }

            return node;
        }

        private static Node<T> Set(int level, Node<T> node, int index, T value)
        {
            var newNode = node.Clone();
            if (level == 0)
            {
                newNode.Value[index & BitMask] = value;
            }
            else
            {
                var subIndex = (index >> level) & BitMask;
                newNode.Child[subIndex] = Set(level - Shift, node.Child[subIndex], index, value);
            }

            return newNode;
        }

        private static Node<T> Remove(int level, Node<T> node, int index)
        {
            var newNode = node.Clone();
            if (level == 0)
            {
                newNode.Value.RemoveAt(index & BitMask);
            }
            else
            {
                var subIndex = (index >> level) & BitMask;
                newNode.Child[subIndex] = Remove(level - Shift, node.Child[subIndex], index);
            }

            return newNode;
        }

        private static int CountBits(uint value)
        {
            var count = 0;
            while (value != 0)
            {
                count++;
                value &= value - 1;
            }

            return count;
        }

        IPersistentList<T> IPersistentList<T>.AddRange(IEnumerable<T> items) => AddRange(items);

        IPersistentList<T> IPersistentList<T>.Clear() => new PersistentList<T>();

        IPersistentList<T> IPersistentList<T>.AddRange(IReadOnlyCollection<T> items) => AddRange(items);

        public IPersistentList<T> Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public IPersistentList<T> InsertRange(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public IPersistentList<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        public IPersistentList<T> RemoveAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        IPersistentList<T> IPersistentList<T>.RemoveAt(int index) => Remove(index);

        public IPersistentList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        public IPersistentList<T> RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public IPersistentList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        public IPersistentList<T> SetItem(int index, T value) => Set(index, value);

        IPersistentList<T> IPersistentList<T>.Add(T item) => Add(item);

        IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items) => AddRange(items);

        IImmutableList<T> IImmutableList<T>.Clear() => new PersistentList<T>();

        public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.Insert(int index, T element) => Insert(index, element);

        IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items) => InsertRange(index, items);

        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer) =>
            Remove(value, equalityComparer);

        IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) => RemoveAll(match);

        IImmutableList<T> IImmutableList<T>.RemoveAt(int index) => Remove(index);

        IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) =>
            RemoveRange(items, equalityComparer);

        IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count) => RemoveRange(index, count);

        IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) =>
            Replace(oldValue, newValue, equalityComparer);

        IImmutableList<T> IImmutableList<T>.SetItem(int index, T value) => Set(index, value);

        IImmutableList<T> IImmutableList<T>.Add(T value) => Add(value);

        IPersistentList<T> IPersistentDataStructure<T, IPersistentList<T>>.AddRange(IEnumerable<T> items) =>
            AddRange(items);

        IPersistentList<T> IPersistentDataStructure<T, IPersistentList<T>>.AddRange(IReadOnlyCollection<T> items) =>
            AddRange(items);

        IPersistentList<T> IPersistentDataStructure<T, IPersistentList<T>>.Clear() => new PersistentList<T>();

        public bool IsEmpty => Count == 0;

        IPersistentList<T> IPersistentDataStructure<T, IPersistentList<T>>.Add(T value) => Add(value);

        public IEnumerator<T> GetEnumerator() => _root.AsEnumerable().Concat(_tail.AsEnumerable()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T this[int index] => Get(index);
    }
}