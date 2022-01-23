using System.Collections.Generic;
using System.Collections.Immutable;

namespace PDS.Collections
{
    public interface IPersistentSet<T> : IPersistentDataStructure<T, IPersistentSet<T>>, IImmutableSet<T>, IReadOnlySet<T>
    {
        /// <summary>Adds the specified element to this persistent set.</summary>
        /// <param name="value">The element to add.</param>
        /// <returns>A new set with the element added, or this set if the element is already in the set.</returns>
        new IPersistentSet<T> Add(T value);

        /// <summary>Retrieves an empty persistent set that has the same sorting and ordering semantics as this instance.</summary>
        /// <returns>An empty set that has the same sorting and ordering semantics as this instance.</returns>
        new IPersistentSet<T> Clear();

        /// <summary>Removes the elements in the specified collection from the current persistent set.</summary>
        /// <param name="other">The collection of items to remove from this set.</param>
        /// <returns>A new set with the items removed; or the original set if none of the items were in the set.</returns>
        new IPersistentSet<T> Except(IEnumerable<T> other);

        /// <summary>Creates an persistent set that contains only elements that exist in this set and the specified set.</summary>
        /// <param name="other">The collection to compare to the current <see cref="T:System.Collections.persistent.IPersistentSet`1" />.</param>
        /// <returns>A new persistent set that contains elements that exist in both sets.</returns>
        new IPersistentSet<T> Intersect(IEnumerable<T> other);

        /// <summary>Removes the specified element from this persistent set.</summary>
        /// <param name="value">The element to remove.</param>
        /// <returns>A new set with the specified element removed, or the current set if the element cannot be found in the set.</returns>
        new IPersistentSet<T> Remove(T value);

        /// <summary>Creates an persistent set that contains only elements that are present either in the current set or in the specified collection, but not both.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>A new set that contains the elements that are present only in the current set or in the specified collection, but not both.</returns>
        new IPersistentSet<T> SymmetricExcept(IEnumerable<T> other);

        /// <summary>Creates a new persistent set that contains all elements that are present in either the current set or in the specified collection.</summary>
        /// <param name="other">The collection to add elements from.</param>
        /// <returns>A new persistent set with the items added; or the original set if all the items were already in the set.</returns>
        new IPersistentSet<T> Union(IEnumerable<T> other);
    }
}