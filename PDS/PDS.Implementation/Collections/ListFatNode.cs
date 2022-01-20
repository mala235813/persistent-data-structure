using System;
using System.Collections.Generic;
using System.Linq;

namespace PDS.Implementation.Collections
{
    internal class ListFatNode<T>
    {
        private const int MaxSize = 2;

        public static ListFatNode<T> Empty => new();

        public List<ListNode<T>> Nodes { get; } = new();

        public void Add(ListNode<T> n)
        {
            Nodes.Add(n);
        }

        public ListNode<T> FindNode(VersionNode<T> versionNode)
        {
            var it = versionNode;
            while (it != null)
            {
                var node = Nodes.FirstOrDefault(n => n.Version == versionNode.Version);
                if (node is null)
                {
                    it = it.Parent;
                }
                else
                {
                    return node;
                }
            }

            throw new InvalidOperationException("Unreachable version");
        }

        public bool IsFull => Nodes.Count == MaxSize;

        public bool IsEmpty => Nodes.Count == 0;

        public ListFatNode<T> UpdateRight(ListFatNode<T>? rightNode, VersionNode<T> versionNode)
        {
            var foundNode = FindNode(versionNode);
            var newNode = new ListNode<T>(versionNode.Version, foundNode.Value, foundNode.LeftNode, rightNode);

            if (IsFull)
            {
                var fat = new ListFatNode<T>();
                fat.Add(newNode);
                if (foundNode.LeftNode != null)
                {
                    newNode.LeftNode = foundNode.LeftNode.UpdateRight(fat, versionNode);
                }
                else
                {
                    versionNode.Front = fat;
                }

                return fat;
            }

            Add(newNode);
            return this;
        }

        public ListFatNode<T> UpdateLeft(ListFatNode<T>? leftNode, VersionNode<T> versionNode)
        {
            var foundNode = FindNode(versionNode);
            var newNode = new ListNode<T>(versionNode.Version, foundNode.Value, leftNode, foundNode.RightNode);

            if (IsFull)
            {
                var fat = new ListFatNode<T>();
                fat.Add(newNode);
                if (foundNode.RightNode != null)
                {
                    newNode.RightNode = foundNode.RightNode.UpdateLeft(fat, versionNode);
                }
                else
                {
                    versionNode.Back = fat;
                }

                return fat;
            }

            Add(newNode);
            return this;
        }
    }
}