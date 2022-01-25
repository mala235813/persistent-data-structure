namespace PDS.UndoRedo
{
    public interface IUndoRedo<TSelf>
    {
        TSelf Redo();
        bool TryRedo(out TSelf collection);
        bool CanRedo { get; }
        
        TSelf Undo();
        bool TryUndo(out TSelf collection);
        bool CanUndo { get; }
    }
}