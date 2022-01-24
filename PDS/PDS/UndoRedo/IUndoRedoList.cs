using System;
using System.Collections.Generic;
using PDS.Collections;

namespace PDS.UndoRedo
{
    public interface IUndoRedoList<T> : IPersistentList<T>,
        IUndoRedoDataStructure<T, IUndoRedoLinkedList<T>>
    {
        new IUndoRedoList<T> Add(T item);
        
        new IUndoRedoList<T> AddRange(IEnumerable<T> items);
        
        new IUndoRedoList<T> AddRange(IReadOnlyCollection<T> items);
        
        new IUndoRedoList<T> Insert(int index, T item);
        
        new IUndoRedoList<T> Remove(T value, IEqualityComparer<T>? equalityComparer);

        new IUndoRedoList<T> RemoveAll(Predicate<T> match);

        new IUndoRedoList<T> RemoveAt(int index);

        new IUndoRedoList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer);

        new IUndoRedoList<T> RemoveRange(int index, int count);

        new IUndoRedoList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer);

        new IUndoRedoList<T> SetItem(int index, T value);

        new IUndoRedoList<T> Clear();
    }
}