using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using PDS.Collections;

namespace PDS.Implementation.Collections
{
    public class PersistentLinkedList<T> : IPersistentLinkedList<T>
    {
        private sealed class PersistentVersionStorage
        {
            public int CurrentVersion { get; private set; }

            public int NextVersion()
            {
                return ++CurrentVersion;
            }
        }

        private readonly PersistentVersionStorage _versionStorage;
        private readonly VersionNode<T> _root;

        public static PersistentLinkedList<T> Empty { get; } = new();
        
        public PersistentLinkedList()
        {
            _versionStorage = new PersistentVersionStorage();
            _root = new VersionNode<T>(_versionStorage.NextVersion(), ListFatNode<T>.Empty, ListFatNode<T>.Empty);
            Count = 0;
        }

        private PersistentLinkedList(PersistentVersionStorage versionStorage, VersionNode<T> root, int count)
        {
            _versionStorage = versionStorage;
            _root = root;
            Count = count;
        }

        public int Count { get; }

        private ListFatNode<T> TraverseRight(int index)
        {
            var it = _root.Front;
            for (var i = 0; i < index; i++)
            {
                it = it.FindNode(_root).RightNode;
                Debug.Assert(it != null, "Inner node is null");
            }

            return it;
        }

        public PersistentLinkedList<T> RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            var indFatNode = TraverseRight(index);
            return RemoveByNode(indFatNode);
        }

        private PersistentLinkedList<T> RemoveByNode(ListFatNode<T> fatNode)
        {
            var delNode = fatNode.FindNode(_root);
            if (delNode.LeftNode is null && delNode.RightNode is null)
            {
                var newV = new VersionNode<T>(_versionStorage.CurrentVersion, ListFatNode<T>.Empty,
                    ListFatNode<T>.Empty, _root);
                return new PersistentLinkedList<T>(_versionStorage, newV, Count - 1);
            }

            var newVersion = new VersionNode<T>(_versionStorage.CurrentVersion, _root.Front, _root.Back, _root);
            if (delNode.RightNode is null)
            {
                var newLeft = delNode.LeftNode!.UpdateRight(null, newVersion);
                if (fatNode == _root.Back)
                {
                    newVersion.Back = newLeft;
                }

                return new PersistentLinkedList<T>(_versionStorage, newVersion, Count - 1);
            }

            if (delNode.LeftNode is null)
            {
                var newRight = delNode.RightNode.UpdateLeft(null, newVersion);
                if (fatNode == _root.Front)
                {
                    newVersion.Front = newRight;
                }

                return new PersistentLinkedList<T>(_versionStorage, newVersion, Count - 1);
            }

            if (!delNode.RightNode.IsFull)
            {
                var newLeft = delNode.LeftNode.UpdateRight(delNode.RightNode, newVersion);
                var newRight = delNode.RightNode.UpdateLeft(delNode.LeftNode, newVersion);
                Debug.Assert(newLeft == newRight, "newLeft == newRight");
                return new PersistentLinkedList<T>(_versionStorage, newVersion, Count - 1);
            }

            var fakeLeft = new ListFatNode<T>();
            var newRightNode = delNode.RightNode.UpdateLeft(fakeLeft, newVersion);
            var newLeftNode = delNode.LeftNode.UpdateRight(newRightNode, newVersion);
            newRightNode.Nodes[0].LeftNode = newLeftNode;

            return new PersistentLinkedList<T>(_versionStorage, newVersion, Count - 1);
        }

        public PersistentLinkedList<T> Insert(int index, T value)
        {
            if (index < 0 || index > Count)
            {
                throw new IndexOutOfRangeException();
            }

            if (index == Count)
            {
                return PushBack(value);
            }

            var newNode = new ListNode<T>(_versionStorage.NextVersion(), value);
            var newFatNode = new ListFatNode<T>(newNode);

            var front = _root.Front;
            if (front.IsEmpty)
            {
                return new PersistentLinkedList<T>(_versionStorage,
                    new VersionNode<T>(_versionStorage.CurrentVersion, newFatNode, newFatNode, _root), Count + 1);
            }

            var newVersion = new VersionNode<T>(_versionStorage.CurrentVersion, _root.Front, _root.Back, _root);
            if (index == 0)
            {
                newNode.RightNode = front.UpdateLeft(newFatNode, newVersion);
                newVersion.Front = newFatNode;
                return new PersistentLinkedList<T>(_versionStorage, newVersion, Count + 1);
            }

            var leftInsNode = TraverseRight(index - 1);
            newNode.LeftNode = leftInsNode.UpdateRight(newFatNode, newVersion);
            var rightIt = leftInsNode.FindNode(_root).RightNode;

            if (rightIt is null)
            {
                newVersion.Back = newFatNode;
            }
            else
            {
                newNode.RightNode = rightIt.UpdateLeft(newFatNode, newVersion);
            }

            return new PersistentLinkedList<T>(_versionStorage, newVersion, Count + 1);
        }

        public PersistentLinkedList<T> Set(int index, T value)
        {
            if (index < 0 || index > Count)
            {
                throw new IndexOutOfRangeException();
            }

            var setFatNode = TraverseRight(index);
            var foundNode = setFatNode.FindNode(_root);
            var newNode = new ListNode<T>(_versionStorage.NextVersion(), value, foundNode.LeftNode,
                foundNode.RightNode);

            var newVersion = new VersionNode<T>(_versionStorage.CurrentVersion, _root.Front, _root.Back, _root);

            if (!setFatNode.IsFull)
            {
                setFatNode.Add(newNode);
                return new PersistentLinkedList<T>(_versionStorage, newVersion, Count);
            }

            var newFatNode = new ListFatNode<T>(newNode);

            if (newNode.LeftNode != null)
            {
                newNode.LeftNode = foundNode.LeftNode!.UpdateRight(newFatNode, newVersion);
            }
            else
            {
                newVersion.Front = newFatNode;
            }

            if (newNode.RightNode != null)
            {
                newNode.RightNode = foundNode.RightNode!.UpdateLeft(newFatNode, newVersion);
            }
            else
            {
                newVersion.Back = newFatNode;
            }

            return new PersistentLinkedList<T>(_versionStorage, newVersion, Count);
        }

        public PersistentLinkedList<T> PushBack(T value)
        {
            if (Count == 0)
            {
                return InitRoot(value);
            }

            var newNode = new ListNode<T>(_versionStorage.NextVersion(), value);
            var newFatNode = new ListFatNode<T>(newNode);

            var newVersion = new VersionNode<T>(_versionStorage.CurrentVersion, _root.Front, newFatNode, _root);
            newNode.LeftNode = _root.Back.UpdateRight(newFatNode, newVersion);

            return new PersistentLinkedList<T>(_versionStorage, newVersion, Count + 1);
        }

        public PersistentLinkedList<T> PushFront(T value)
        {
            if (Count == 0)
            {
                return InitRoot(value);
            }

            var newNode = new ListNode<T>(_versionStorage.NextVersion(), value);
            var newFatNode = new ListFatNode<T>(newNode);

            var newVersion = new VersionNode<T>(_versionStorage.CurrentVersion, newFatNode, _root.Back, _root);
            newNode.RightNode = _root.Front.UpdateLeft(newFatNode, newVersion);

            return new PersistentLinkedList<T>(_versionStorage, newVersion, Count + 1);
        }

        private PersistentLinkedList<T> InitRoot(T value)
        {
            var newNode = new ListNode<T>(_versionStorage.NextVersion(), value);
            var newFatNode = new ListFatNode<T>(newNode);

            var newVersion = new VersionNode<T>(_versionStorage.CurrentVersion, newFatNode, newFatNode, _root);
            return new PersistentLinkedList<T>(_versionStorage, newVersion, Count + 1);
        }

        #region IPersistentLinkedList

        public T First => _root.Front.FindNode(_root).Value;

        public T Last => _root.Back.FindNode(_root).Value;

        public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || index + count > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var i = index;
            foreach (var it in this.Skip(index).Take(count))
            {
                if (Equals(it, item))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.Insert(int index, T item)
        {
            return Insert(index, item);
        }

        public IPersistentLinkedList<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        public IPersistentLinkedList<T> RemoveAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        IPersistentLinkedList<T> IPersistentLinkedList<T>.RemoveAt(int index) => RemoveAt(index);

        public IPersistentLinkedList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        public IPersistentLinkedList<T> RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public IPersistentLinkedList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            throw new NotImplementedException();
        }

        public IPersistentLinkedList<T> SetItem(int index, T value)
        {
            return Set(index, value);
        }

        public IPersistentLinkedList<T> AddFirst(T item)
        {
            return PushFront(item);
        }

        public IPersistentLinkedList<T> AddLast(T item)
        {
            return PushBack(item);
        }

        public IPersistentLinkedList<T> RemoveFirst()
        {
            return RemoveByNode(_root.Front);
        }

        public IPersistentLinkedList<T> RemoveLast()
        {
            return RemoveByNode(_root.Back);
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public T Get(int index)
        {
            return TraverseRight(index).FindNode(_root).Value;
        }

        public IPersistentLinkedList<T> Add(T item)
        {
            return PushBack(item);
        }

        public IPersistentLinkedList<T> AddRange(IEnumerable<T> items)
        {
            using var enumerator = items.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return this;
            }

            var count = 0;
            VersionNode<T> newVersion;
            if (Count == 0)
            {
                count++;
                var newNode = new ListNode<T>(_versionStorage.NextVersion(), enumerator.Current);
                var newFatNode = new ListFatNode<T>(newNode);

                newVersion = new VersionNode<T>(_versionStorage.CurrentVersion, newFatNode, newFatNode, _root);
            }
            else
            {
                newVersion = new VersionNode<T>(_versionStorage.NextVersion(), _root.Front, _root.Back, _root);
                enumerator.Reset();
            }

            while (enumerator.MoveNext())
            {
                count++;
                var newNode = new ListNode<T>(_versionStorage.CurrentVersion, enumerator.Current);
                var newFatNode = new ListFatNode<T>(newNode);

                newVersion.Back = newFatNode;
                newNode.LeftNode = _root.Back.UpdateRight(newFatNode, newVersion);
            }

            return new PersistentLinkedList<T>(_versionStorage, newVersion, count);
        }

        public IPersistentLinkedList<T> AddRange(IReadOnlyCollection<T> items)
        {
            return AddRange(items.AsEnumerable());
        }

        #endregion

        #region IPersistentDataStructure

        public bool IsEmpty => Count == 0;

        public IEnumerator<T> GetEnumerator()
        {
            var it = _root.Front;
            for (var i = 0; i < Count; i++)
            {
                var listNode = it.FindNode(_root);
                yield return listNode.Value;
                it = listNode.RightNode;
                Debug.Assert(it != null, "Inner node is null");
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.Add(T value) => Add(value);

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.AddRange(IEnumerable<T> items) =>
            AddRange(items);

        IPersistentLinkedList<T> IPersistentDataStructure<T, IPersistentLinkedList<T>>.AddRange(
            IReadOnlyCollection<T> items) => AddRange(items);

        public IPersistentLinkedList<T> Clear() => new PersistentLinkedList<T>();

        #endregion

        #region IPersistentStack

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.AddRange(IEnumerable<T> items) =>
            AddRange(items);

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.AddRange(IReadOnlyCollection<T> items) =>
            AddRange(items);

        IPersistentStack<T> IPersistentStack<T>.Clear() => Clear();

        IPersistentStack<T> IPersistentStack<T>.Push(T value) => PushBack(value);

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.Clear() => Clear();

        T IImmutableStack<T>.Peek() => Last;

        IPersistentStack<T> IPersistentStack<T>.Pop() => RemoveLast();

        IImmutableStack<T> IImmutableStack<T>.Pop() => RemoveLast();

        IImmutableStack<T> IImmutableStack<T>.Push(T value) => PushBack(value);

        bool IImmutableStack<T>.IsEmpty => IsEmpty;

        IPersistentStack<T> IPersistentDataStructure<T, IPersistentStack<T>>.Add(T value) => Add(value);

        #endregion

        #region IPersistentQueue

        public IImmutableQueue<T> Dequeue() => RemoveFirst();

        public IImmutableQueue<T> Enqueue(T value) => PushBack(value);

        IImmutableStack<T> IImmutableStack<T>.Clear() => Clear();

        T IImmutableQueue<T>.Peek() => First;

        bool IImmutableQueue<T>.IsEmpty => IsEmpty;

        IImmutableQueue<T> IImmutableQueue<T>.Clear() => Clear();

        #endregion
    }
}