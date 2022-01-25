using PDS.Collections;

namespace PDS.UndoRedo
{
    /// <summary>
    /// Persistent stack with undo-redo mechanic
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public interface IUndoRedoStack<T> : IPersistentStack<T>,
        IUndoRedoDataStructure<T, IUndoRedoStack<T>>
    {
        /// <summary>
        /// Remove item from the top of the stack
        /// </summary>
        /// <returns>New instance of persistent stack</returns>
        new IUndoRedoStack<T> Pop();
        
        /// <summary>
        /// Add item to the top of the stack
        /// </summary>
        /// <param name="value"></param>
        /// <returns>New instance of persistent stack</returns>
        new IUndoRedoStack<T> Push(T value);
        
        /// <summary>
        /// Clear persistent stack
        /// </summary>
        /// <returns>Empty stack</returns>
        new IUndoRedoStack<T> Clear();
        
        /// <summary>
        /// Check if stack is empty
        /// </summary>
        new bool IsEmpty { get; }
    }
}