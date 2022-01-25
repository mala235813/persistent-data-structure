using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PDS.Implementation.Collections
{
    public class Node<T> : IEnumerable<T>
    {
        public List<Node<T>> Child { get; } = new();
        public List<T> Value { get; } = new();

        public Node()
        {
        }

        public Node(Node<T> other)
        {
            if (other.Child.Count != 0)
            {
                Child = new List<Node<T>>(other.Child);
            }

            if (other.Value.Count != 0)
            {
                Value = new List<T>(other.Value);
            }
        }

        public Node(Node<T> other, int maxIndex)
        {
            if (other.Child.Count != 0)
            {
                Child = new List<Node<T>>(maxIndex);
                for (var i = 0; i <= maxIndex; i++)
                    Child[i] = other.Child[i];
            }

            if (other.Value.Count != 0)
            {
                Value = new List<T>();
                for (var i = 0; i <= maxIndex; i++)
                    Value[i] = other.Value[i];
            }
        }


        public bool IsEmpty => Child.Count == 0 && Value.Count == 0;
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