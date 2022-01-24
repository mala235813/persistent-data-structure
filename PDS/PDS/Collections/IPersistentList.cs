using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PDS.Collections
{
    /// <summary>
    /// Persistent array list with constant time random access
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public interface IPersistentList<T> : IPersistentDataStructure<T, IPersistentList<T>>, IImmutableList<T>
    {
        /// <summary>
        /// Add item to the end of the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns>New instance of persistent list</returns>
        new IPersistentList<T> Add(T item);
        
        /// <summary>
        /// Add range of items to the end of the list
        /// </summary>
        /// <param name="items"></param> 
        /// <returns>New instance of persistent list</returns>
        new IPersistentList<T> AddRange(IEnumerable<T> items);

        /// <summary>
        /// Add range of items to the end of the list
        /// </summary>
        /// <param name="items"></param> 
        /// <returns>New instance of persistent list</returns>
        new IPersistentList<T> AddRange(IReadOnlyCollection<T> items);

        /// <summary>
        /// Locate given item in the list
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>Index of item, or -1 if item was not found</returns>
        new int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer);
        
        /// <summary>
        /// Insert item in given position
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns>New instance of persistent list</returns>
        new IPersistentList<T> Insert(int index, T item);

        /// <summary>
        /// Locate given item from the end of the list
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>Last index of item, or -1 if item was not found</returns>
        new int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer);

        /// <summary>
        /// Remove first occurence of given item
        /// </summary>
        /// <param name="value"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>New instance of persistent list</returns>
        new IPersistentList<T> Remove(T value, IEqualityComparer<T>? equalityComparer);

        /// <summary>
        /// Remove all items that match given predicate
        /// </summary>
        /// <param name="match"></param>
        /// <returns>New instance of persistent list</returns>
        new IPersistentList<T> RemoveAll(Predicate<T> match);

        /// <summary>
        /// Remove item from given position
        /// </summary>
        /// <param name="index"></param>
        /// <returns>New instance of persistent list</returns>
        new IPersistentList<T> RemoveAt(int index);

        /// <summary>
        /// Remove range of items
        /// </summary>
        /// <param name="items"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>New instance of persistent list</returns>
        new IPersistentList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer);

        
        /// <summary>
        /// Remove count items starting from given index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns>New instance of persistent list</returns>
        new IPersistentList<T> RemoveRange(int index, int count);

        /// <summary>
        /// Replace oldValue with newValue in list
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>New instance of persistent list</returns>
        new IPersistentList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer);

        /// <summary>
        /// Set value of item in given position
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns>New instance of persistent list</returns>
        new IPersistentList<T> SetItem(int index, T value);

        /// <summary>
        /// Clear persistent list
        /// </summary>
        /// <returns>Empty list</returns>
        new IPersistentList<T> Clear();
    }
}