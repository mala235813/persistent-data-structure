using System.Collections.Immutable;

namespace PDS.Collections
{
    public interface IPersistentStack<T> : IPersistentDataStructure<T, IPersistentStack<T>>, IImmutableStack<T>
    {
        IPersistentStack<T> Pop();
        IPersistentStack<T> Push(T value);
        IPersistentStack<T> Clear();
    }
}