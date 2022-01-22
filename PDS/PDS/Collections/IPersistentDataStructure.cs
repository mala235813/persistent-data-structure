using System.Collections.Generic;

namespace PDS.Collections
{
    public interface IPersistentDataStructure<T, out TSelf> : IReadOnlyCollection<T>
    {
        TSelf Add(T value);
        
        TSelf AddRange(IEnumerable<T> items);
        
        TSelf Clear();
        
//        static TSelf Empty { get; } 
    }
}