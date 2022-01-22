using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PDS.Collections
{
    public interface IPersistentDictionary<TKey, TValue>
        : IPersistentDataStructure<KeyValuePair<TKey, TValue>, IPersistentDictionary<TKey, TValue>>,
            IImmutableDictionary<TKey, TValue>
    {
        new IPersistentDictionary<TKey, TValue> Clear();

        new IPersistentDictionary<TKey, TValue> Add(TKey key, TValue value);
        
        IPersistentDictionary<TKey, TValue> AddOrUpdate(TKey key, TValue value);
        
        IPersistentDictionary<TKey, TValue> Update(TKey key, Func<TKey, TValue, TValue> valueFactory);
        
        bool TryAdd(TKey key, TValue value, out IPersistentDictionary<TKey, TValue> newVersion);

        new IPersistentDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs);

        new IPersistentDictionary<TKey, TValue> SetItem(TKey key, TValue value);

        new IPersistentDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items);

        new IPersistentDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys);

        new IPersistentDictionary<TKey, TValue> Remove(TKey key);
        
        bool TryRemove(TKey key, out IPersistentDictionary<TKey, TValue> newVersion);
    }
}