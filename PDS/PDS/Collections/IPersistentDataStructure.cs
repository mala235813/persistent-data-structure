using System.Collections.Generic;

namespace PDS.Collections
{
    /// <summary>
    /// Common interface for all persistent collections
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <typeparam name="TSelf">Type of persistent collection implementation</typeparam>
    public interface IPersistentDataStructure<T, out TSelf> : IReadOnlyCollection<T>
    {
        /// <summary>
        /// Add value to persistent collection
        /// </summary>
        /// <param name="value">Value to add</param>
        /// <returns>New instance of persistent collection</returns>
        TSelf Add(T value);

        /// <summary>
        /// Add range of items to persistent collection
        /// </summary>
        /// <param name="items">Items to add</param>
        /// <returns>New instance of persistent collection</returns>
        TSelf AddRange(IEnumerable<T> items);
        
        /// <summary>
        /// Add range of items to persistent collection
        /// </summary>
        /// <param name="items">Items to add</param>
        /// <returns>New instance of persistent collection</returns>
        TSelf AddRange(IReadOnlyCollection<T> items);
        
        /// <summary>
        /// Clear persistent collection
        /// </summary>
        /// <returns>Empty instance of persistent collection</returns>
        TSelf Clear();
        
        /// <summary>
        /// Check if collection is empty
        /// </summary>
        bool IsEmpty { get; }
    }
}