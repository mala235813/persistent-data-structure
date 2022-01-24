using System.Collections.Generic;

namespace PDS.Collections
{
    public interface IPersistentDataStructure<T, out TSelf> : IReadOnlyCollection<T>
        where TSelf : IPersistentDataStructure<T, TSelf>
    {
        TSelf Add(T value);

        TSelf AddRange(IEnumerable<T> items);

        TSelf AddRange(IReadOnlyCollection<T> items);

        TSelf Clear();

        bool IsEmpty { get; }
    }
}