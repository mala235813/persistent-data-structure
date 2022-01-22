using System.Collections.Immutable;

namespace PDS.Collections
{
    public interface IPersistentStack<T> : IPersistentDataStructure<T, IPersistentStack<T>>, IImmutableStack<T>
    {
        new IPersistentStack<T> Pop();

        new IPersistentStack<T> Push(T value);
        
        new IPersistentStack<T> Clear();
        
        new bool IsEmpty { get; }
    }
}