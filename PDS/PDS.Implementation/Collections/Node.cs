using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PDS.Implementation.Collections
{
    public class Node<T> : IEnumerable<T>
    {
        public List<Node<T>> Child { get; }
        public List<T> Value { get; }

        public Node()
        {
            Child = new List<Node<T>>();
            Value = new List<T>();
        }

        private Node(Node<T> other)
        {
            Child = new List<Node<T>>(other.Child);
            Value = new List<T>(other.Value);
        }
        
        public int Count => Child.Count == 0 ? Value.Count : Child.Count;

        public Node<T> Clone()
        {
            return new Node<T>(this);
        }

        public IEnumerator<T> GetEnumerator() =>
            Child.Count > 0 ? Child.SelectMany(c => c).GetEnumerator() : Value.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}