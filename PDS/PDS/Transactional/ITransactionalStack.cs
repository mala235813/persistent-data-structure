using PDS.Collections;

namespace PDS.Transactional
{
    public interface ITransactionalStack<T> : IPersistentStack<T>,
        ITransactionalDataStructure<T, ITransactionalStack<T>>
    {
        new ITransactionalStack<T> Pop();
        new ITransactionalStack<T> Push(T value);
        new ITransactionalStack<T> Clear();
    }
}