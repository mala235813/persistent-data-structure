using PDS.Collections;

namespace PDS.UndoRedo
{
    public interface IUndoRedoStack<T> : IPersistentStack<T>,
        IUndoRedoDataStructure<T, IUndoRedoStack<T>>
    {
        new IUndoRedoStack<T> Pop();
        new IUndoRedoStack<T> Push(T value);
        new IUndoRedoStack<T> Clear();
        new bool IsEmpty { get; }
    }
}