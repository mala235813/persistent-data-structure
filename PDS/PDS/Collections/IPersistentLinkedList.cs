using System.Collections.Generic;
using System.Collections.Immutable;

namespace PDS.Collections
{
    public interface IPersistentLinkedList<T> : IPersistentDataStructure<T, IPersistentLinkedList<T>>,
        IImmutableQueue<T>, IImmutableStack<T>, ICollection<T>
    {
    }
}