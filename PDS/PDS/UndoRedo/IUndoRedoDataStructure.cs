using PDS.Collections;

namespace PDS.UndoRedo
{
    /// <summary>
    /// Common interface for all persistent collections with undo-redo mechanic
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <typeparam name="TSelf">Type of persistent undo redo collection implementation</typeparam>
    public interface IUndoRedoDataStructure<T, TSelf> : IUndoRedo<IUndoRedoDataStructure<T, TSelf>>,
        IPersistentDataStructure<T, IUndoRedoDataStructure<T, TSelf>> where TSelf : IUndoRedoDataStructure<T, TSelf>
    {
    }
}