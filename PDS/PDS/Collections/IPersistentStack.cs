using System.Collections.Immutable;

namespace PDS.Collections
{
    /// <summary>
    /// Persistent queue
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public interface IPersistentStack<T> : IPersistentDataStructure<T, IPersistentStack<T>>, IImmutableStack<T>
    {
        /// <summary>
        /// Remove item from the top of the stack
        /// </summary>
        /// <returns>New instance of persistent stack</returns>
        new IPersistentStack<T> Pop();

        /// <summary>
        /// Add item to the top of the stack
        /// </summary>
        /// <param name="value"></param>
        /// <returns>New instance of persistent stack</returns>
        new IPersistentStack<T> Push(T value);
        
        /// <summary>
        /// Clear persistent stack
        /// </summary>
        /// <returns>Empty stack</returns>
        new IPersistentStack<T> Clear();
        
        /// <summary>
        /// Check if stack is empty
        /// </summary>
        new bool IsEmpty { get; }
    }
}