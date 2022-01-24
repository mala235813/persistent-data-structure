using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PDS.Collections
{
    /// <summary>
    /// Persistent dictionary
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    public interface IPersistentDictionary<TKey, TValue>
        : IPersistentDataStructure<KeyValuePair<TKey, TValue>, IPersistentDictionary<TKey, TValue>>,
            IImmutableDictionary<TKey, TValue>
    {
        /// <summary>
        /// Clear persistent dictionary
        /// </summary>
        /// <returns>Empty dictionary</returns>
        new IPersistentDictionary<TKey, TValue> Clear();

        /// <summary>
        /// Associate given key with given value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IPersistentDictionary<TKey, TValue> Add(TKey key, TValue value);
        
        /// <summary>
        /// Add or update given key with given value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>New instance of persistent dictionary</returns>
        IPersistentDictionary<TKey, TValue> AddOrUpdate(TKey key, TValue value);
        
        /// <summary>
        /// Update key using value factory
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueFactory">Function that provides value depending on key and old value</param>
        /// <returns>New instance of persistent dictionary</returns>
        IPersistentDictionary<TKey, TValue> Update(TKey key, Func<TKey, TValue, TValue> valueFactory);
        
        /// <summary>
        /// Try associate given key with given value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="newVersion">New instance of persistent dictionary, or same instance if false</param>
        /// <returns>True, if new value was added successfully</returns>
        bool TryAdd(TKey key, TValue value, out IPersistentDictionary<TKey, TValue> newVersion);

        /// <summary>
        /// Add range of key value pairs to dictionary
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IPersistentDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs);

        /// <summary>
        /// Set value for given key, same as AddOrUpdate
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IPersistentDictionary<TKey, TValue> SetItem(TKey key, TValue value);

        /// <summary>
        /// Add range of key value pairs to dictionary
        /// </summary>
        /// <param name="items"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IPersistentDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items);

        /// <summary>
        /// Remove range of keys from dictionary
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IPersistentDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys);

        /// <summary>
        /// Remove given key from dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IPersistentDictionary<TKey, TValue> Remove(TKey key);
        
        /// <summary>
        /// Try remove given key from dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newVersion">New instance of persistent dictionary, or same instance if false</param>
        /// <returns>True, if new value was removed successfully</returns>
        bool TryRemove(TKey key, out IPersistentDictionary<TKey, TValue> newVersion);
    }
}