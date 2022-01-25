using System.Collections.Immutable;

namespace PDS.Collections
{
    /// <summary>
    /// Persistent queue
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public interface IPersistentQueue<T> : IPersistentDataStructure<T, IPersistentQueue<T>>, IImmutableQueue<T>
    {
        /// <summary>
        /// Clear persistent queue
        /// </summary>
        /// <returns>Empty queue</returns>
        new IPersistentQueue<T> Clear();

        /// <summary>
        /// Add item to the end of the queue
        /// </summary>
        /// <param name="value"></param>
        /// <returns>New instance of persistent queue</returns>
        new IPersistentQueue<T> Enqueue(T value);

        /// <summary>
        /// Remove first item from the queue 
        /// </summary>
        /// <returns>New instance of persistent queue</returns>
        new IPersistentQueue<T> Dequeue();
    }
}