using System;
using System.Collections.Generic;
using PDS.Collections;

namespace PDS.UndoRedo
{
    /// <summary>
    /// Persistent doubly linked list with undo-redo mechanic
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public interface IUndoRedoLinkedList<T> : IPersistentLinkedList<T>,
        IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>, IUndoRedoStack<T>
    {
        /// <summary>
        /// Add item to the front of the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> AddFirst(T item);
        
        /// <summary>
        /// Add item to the end of the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> AddLast(T item);

        /// <summary>
        /// Remove first item from the list 
        /// </summary>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> RemoveFirst();
        
        /// <summary>
        /// Remove last item from the list 
        /// </summary>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> RemoveLast();

        /// <summary>
        /// Add item to the end of the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> Add(T item);
        
        /// <summary>
        /// Add range of items to the end of the list
        /// </summary>
        /// <param name="items"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> AddRange(IEnumerable<T> items);
        
        /// <summary>
        /// Add range of items to the end of the list
        /// </summary>
        /// <param name="items"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> AddRange(IReadOnlyCollection<T> items);

        /// <summary>
        /// Insert item in given position
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> Insert(int index, T item);

        /// <summary>
        /// Remove first occurence of given item
        /// </summary>
        /// <param name="value"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> Remove(T value, IEqualityComparer<T>? equalityComparer);

        /// <summary>
        /// Remove all items that match given predicate
        /// </summary>
        /// <param name="match"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> RemoveAll(Predicate<T> match);

        /// <summary>
        /// Remove item from given position
        /// </summary>
        /// <param name="index"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> RemoveAt(int index);

        /// <summary>
        /// Remove range of items
        /// </summary>
        /// <param name="items"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer);

        /// <summary>
        /// Remove count items starting from given index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> RemoveRange(int index, int count);

        /// <summary>
        /// Replace oldValue with newValue in list
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="equalityComparer"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer);

        /// <summary>
        /// Set value of item in given position
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns>New instance of persistent linked list</returns>
        new IUndoRedoLinkedList<T> SetItem(int index, T value);

        /// <summary>
        /// Clear persistent linked list
        /// </summary>
        /// <returns>Empty list</returns>
        new IUndoRedoLinkedList<T> Clear();
    }
}