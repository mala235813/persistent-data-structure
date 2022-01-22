using PDS.Collections;

namespace PDS.Transactional
{
    public interface ITransactionalDataStructure<T, out TSelf> : ITransactional<ITransactionalDataStructure<T, TSelf>>,
        IPersistentDataStructure<T, ITransactionalDataStructure<T, TSelf>>
    {
    }
}