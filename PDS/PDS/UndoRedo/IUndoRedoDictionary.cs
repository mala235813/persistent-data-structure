using System;
using System.Collections.Generic;
using PDS.Collections;

namespace PDS.UndoRedo
{
    public interface IUndoRedoDictionary<TKey, TValue> : IPersistentDictionary<TKey, TValue>,
        IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>>
    {
        new IUndoRedoDictionary<TKey, TValue> Clear();

        new IUndoRedoDictionary<TKey, TValue> Add(TKey key, TValue value);
        
        new IUndoRedoDictionary<TKey, TValue> AddOrUpdate(TKey key, TValue value);
        
        new IUndoRedoDictionary<TKey, TValue> Update(TKey key, Func<TKey, TValue, TValue> valueFactory);
        
        bool TryAdd(TKey key, TValue value, out IUndoRedoDictionary<TKey, TValue> newVersion);

        new IUndoRedoDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs);

        new IUndoRedoDictionary<TKey, TValue> SetItem(TKey key, TValue value);

        new IUndoRedoDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items);

        new IUndoRedoDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys);

        new IUndoRedoDictionary<TKey, TValue> Remove(TKey key);
        
        bool TryRemove(TKey key, out IUndoRedoDictionary<TKey, TValue> newVersion);
    }
}