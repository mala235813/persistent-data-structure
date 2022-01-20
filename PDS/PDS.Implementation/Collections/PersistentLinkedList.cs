using System;
using System.Diagnostics;

namespace PDS.Implementation.Collections
{
    public class PersistentLinkedList<T>
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

        public T Front() => _root.Front.FindNode(_root).Value;
        public T Back() => _root.Back.FindNode(_root).Value;

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

        public PersistentLinkedList<T> Remove(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            var indFatNode = TraverseRight(index);
            var delNode = indFatNode.FindNode(_root);

            if (delNode.LeftNode is null && delNode.RightNode is null)
            {
                var newV = new VersionNode<T>(_versionStorage.CurrentVersion, ListFatNode<T>.Empty,
                    ListFatNode<T>.Empty, _root);
                return new PersistentLinkedList<T>(_versionStorage, newV, Count - 1);
            }

            var newVersion = new VersionNode<T>(_versionStorage.CurrentVersion, _root.Front, _root.Back, _root);
            if (delNode.RightNode is null)
            {
                var newLeft = delNode.LeftNode.UpdateRight(null, newVersion);
                if (indFatNode == _root.Back)
                {
                    newVersion.Back = newLeft;
                }

                return new PersistentLinkedList<T>(_versionStorage, newVersion, Count - 1);
            }

            if (delNode.LeftNode is null)
            {
                var newRight = delNode.RightNode.UpdateLeft(null, newVersion);
                if (indFatNode == _root.Front)
                {
                    newVersion.Front = newRight;
                }

                return new PersistentLinkedList<T>(_versionStorage, newVersion, Count - 1);
            }

            if (!delNode.RightNode.IsFull)
            {
                var newLeft = delNode.LeftNode.UpdateRight(delNode.RightNode, newVersion);
                var newRight = delNode.RightNode.UpdateLeft(delNode.LeftNode, newVersion);
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

        public T Get(int index)
        {
            return TraverseRight(index).FindNode(_root).Value;
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
                newNode.LeftNode = foundNode.LeftNode.UpdateRight(newFatNode, newVersion);
            }
            else
            {
                newVersion.Front = newFatNode;
            }

            if (newNode.RightNode != null)
            {
                newNode.RightNode = foundNode.RightNode.UpdateLeft(newFatNode, newVersion);
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
    }
}