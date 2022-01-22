namespace PDS.Transactional
{
    public interface ITransactional<out TSelf>
    {
        TSelf Undo();

        TSelf Redo();
    }
}