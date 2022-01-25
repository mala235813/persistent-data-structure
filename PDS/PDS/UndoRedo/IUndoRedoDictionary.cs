using System;
using System.Collections.Generic;
using PDS.Collections;

namespace PDS.UndoRedo
{
    /// <summary>
    /// Persistent dictionary with undo-redo mechanic
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    public interface IUndoRedoDictionary<TKey, TValue> : IPersistentDictionary<TKey, TValue>,
        IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>>
    {
        /// <summary>
        /// Clear persistent dictionary
        /// </summary>
        /// <returns>Empty dictionary</returns>
        new IUndoRedoDictionary<TKey, TValue> Clear();

        /// <summary>
        /// Associate given key with given value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IUndoRedoDictionary<TKey, TValue> Add(TKey key, TValue value);
        
        /// <summary>
        /// Add or update given key with given value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IUndoRedoDictionary<TKey, TValue> AddOrUpdate(TKey key, TValue value);
        
        /// <summary>
        /// Update key using value factory
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueFactory">Function that provides value depending on key and old value</param>
        /// <returns>New instance of persistent dictionary</returns>
        new IUndoRedoDictionary<TKey, TValue> Update(TKey key, Func<TKey, TValue, TValue> valueFactory);
        
        /// <summary>
        /// Try associate given key with given value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="newVersion">New instance of persistent dictionary, or same instance if false</param>
        /// <returns>True, if new value was added successfully</returns>
        bool TryAdd(TKey key, TValue value, out IUndoRedoDictionary<TKey, TValue> newVersion);

        /// <summary>
        /// Add range of key value pairs to dictionary
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IUndoRedoDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs);

        /// <summary>
        /// Set value for given key, same as AddOrUpdate
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IUndoRedoDictionary<TKey, TValue> SetItem(TKey key, TValue value);

        /// <summary>
        /// Add range of key value pairs to dictionary
        /// </summary>
        /// <param name="items"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IUndoRedoDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items);

        /// <summary>
        /// Remove range of keys from dictionary
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IUndoRedoDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys);

        /// <summary>
        /// Remove given key from dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns>New instance of persistent dictionary</returns>
        new IUndoRedoDictionary<TKey, TValue> Remove(TKey key);
          
        /// <summary>
        /// Try remove given key from dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newVersion">New instance of persistent dictionary, or same instance if false</param>
        /// <returns>True, if new value was removed successfully</returns>
        bool TryRemove(TKey key, out IUndoRedoDictionary<TKey, TValue> newVersion);
    }
}