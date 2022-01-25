namespace PDS.UndoRedo
{
    /// <summary>
    /// Common interface for all persistent collections, that allows version storing and traversing
    /// </summary>
    /// <typeparam name="TSelf">Type of persistent collection implementation</typeparam>
    public interface IUndoRedo<TSelf> where TSelf: IUndoRedo<TSelf>
    {
        /// <summary>
        /// Redo last undo operation or throw exception
        /// </summary>
        /// <returns>Instance of persistent collection before last undo</returns>
        TSelf Redo();

        /// <summary>
        /// Try to redo last undo operation
        /// </summary>
        /// <param name="collection">Instance of persistent collection before last undo, or self if false</param>
        /// <returns>True, if redo happened successfully</returns>
        bool TryRedo(out TSelf collection);
        
        /// <summary>
        /// Check if redo is possible
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// Undo last modifying operation or throw exception
        /// </summary>
        /// <returns>Instance of persistent collection before last modifying operation</returns>
        TSelf Undo();
        
        /// <summary>
        /// Tre to undo last modifying operation
        /// </summary>
        /// <param name="collection">Instance of persistent collection before last modifying operation, or self if false</param>
        /// <returns>True, if undo happened successfully</returns>
        bool TryUndo(out TSelf collection);
        
        /// <summary>
        /// Check if undo is possible
        /// </summary>
        bool CanUndo { get; }
    }
}