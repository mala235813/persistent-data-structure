using System;
using System.Collections.Generic;
using PDS.Collections;

namespace PDS.UndoRedo
{
    public interface IUndoRedoLinkedList<T> : IPersistentLinkedList<T>,
        IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>, IUndoRedoStack<T>
    {
        new IUndoRedoLinkedList<T> AddFirst(T item);
        
        new IUndoRedoLinkedList<T> AddLast(T item);

        new IUndoRedoLinkedList<T> RemoveFirst();
        
        new IUndoRedoLinkedList<T> RemoveLast();

        new IUndoRedoLinkedList<T> Add(T item);
        
        new IUndoRedoLinkedList<T> AddRange(IEnumerable<T> items);
        
        new IUndoRedoLinkedList<T> AddRange(IReadOnlyCollection<T> items);

        new IUndoRedoLinkedList<T> Insert(int index, T item);

        new IUndoRedoLinkedList<T> Remove(T value, IEqualityComparer<T>? equalityComparer);

        new IUndoRedoLinkedList<T> RemoveAll(Predicate<T> match);

        new IUndoRedoLinkedList<T> RemoveAt(int index);

        new IUndoRedoLinkedList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer);

        new IUndoRedoLinkedList<T> RemoveRange(int index, int count);

        new IUndoRedoLinkedList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer);

        new IUndoRedoLinkedList<T> SetItem(int index, T value);

        new IUndoRedoLinkedList<T> Clear();
    }
}