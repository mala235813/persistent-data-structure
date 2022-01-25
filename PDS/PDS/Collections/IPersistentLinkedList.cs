using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PDS.Collections
{
    /// <summary>
    /// Persistent doubly linked list
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public interface IPersistentLinkedList<T> : IPersistentDataStructure<T, IPersistentLinkedList<T>>,
        IImmutableQueue<T>, IPersistentStack<T>
    {
        /// <summary>
        /// First item in list
        /// </summary>
        T First { get; }
        
        /// <summary>
        /// Last item in list
        /// </summary>
        T Last { get; }

        /// <summary>
        /// Add item to the front of the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> AddFirst(T item);
        
        /// <summary>
        /// Add item to the end of the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> AddLast(T item);

        /// <summary>
        /// Remove first item from the list 
        /// </summary>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> RemoveFirst();
        
        /// <summary>
        /// Remove last item from the list 
        /// </summary>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> RemoveLast();

        /// <summary>
        /// Check if list contains given item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True, if item was found, otherwise false</returns>
        bool Contains(T item);
        
        new bool IsEmpty { get; }

        /// <summary>
        /// Get item by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Item with given index</returns>
        T Get(int index);

        /// <summary>
        /// Add item to the end of the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IPersistentLinkedList<T> Add(T item);
        
        /// <summary>
        /// Add range of items to the end of the list
        /// </summary>
        /// <param name="items"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IPersistentLinkedList<T> AddRange(IEnumerable<T> items);
        
        /// <summary>
        /// Add range of items to the end of the list
        /// </summary>
        /// <param name="items"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IPersistentLinkedList<T> AddRange(IReadOnlyCollection<T> items);

        /// <summary>
        /// Locate given item in the list
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>Index of item, or -1 if item was not found</returns>
        int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer);

        /// <summary>
        /// Insert item in given position
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> Insert(int index, T item);

        /// <summary>
        /// Remove first occurence of given item
        /// </summary>
        /// <param name="value"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> Remove(T value, IEqualityComparer<T>? equalityComparer);

        /// <summary>
        /// Remove all items that match given predicate
        /// </summary>
        /// <param name="match"></param>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> RemoveAll(Predicate<T> match);

        /// <summary>
        /// Remove item from given position
        /// </summary>
        /// <param name="index"></param>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> RemoveAt(int index);

        /// <summary>
        /// Remove range of items
        /// </summary>
        /// <param name="items"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer);

        /// <summary>
        /// Remove count items starting from given index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> RemoveRange(int index, int count);

        /// <summary>
        /// Replace oldValue with newValue in list
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer);

        /// <summary>
        /// Set value of item in given position
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns>New instance of persistent linked list</returns>
        IPersistentLinkedList<T> SetItem(int index, T value);

        new IPersistentLinkedList<T> Clear();
    }
}