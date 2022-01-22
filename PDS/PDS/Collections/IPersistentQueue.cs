using System.Collections.Immutable;

namespace PDS.Collections
{
    public interface IPersistentQueue<T> : IPersistentDataStructure<T, IPersistentQueue<T>>, IImmutableQueue<T>
    {
        new IPersistentQueue<T> Clear();

        new IPersistentQueue<T> Enqueue(T value);

        new IPersistentQueue<T> Dequeue();
    }
}