using System.Collections.Generic;
using System.Collections.Immutable;

namespace PDS.Collections
{
    public interface IPersistentDictionary<TKey, TValue>
        : IPersistentDataStructure<KeyValuePair<TKey, TValue>, IPersistentDictionary<TKey, TValue>>,
            IImmutableDictionary<TKey, TValue>
    {
    }
}