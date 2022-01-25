using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using PDS.Collections;

namespace PDS.Implementation.Collections
{
    public class PersistentSet<T> : IPersistentSet<T> where T : notnull
    {
        private const int InitialSize = 32;
        private IPersistentList<List<T>> _buckets;

        public static PersistentSet<T> Empty { get; } = new();

        public PersistentSet(int count, IPersistentList<List<T>> buckets)
        {
            Count = count;
            _buckets = buckets;
        }
        
        public PersistentSet()
        {
            Count = 0;
            var array = Enumerable.Range(0, InitialSize).Select(i => new List<T>()).ToArray();
            _buckets = new PersistentList<List<T>>().AddRange(array);
        }

        public int Count { get; }

        private (int index, List<T> bucket) GetBucket(T value)
        {
            var index = value.GetHashCode() % _buckets.Count;
            return (index, _buckets[index]);
        }

        public PersistentSet<T> Remove(T value)
        {
            var (index, bucket) = GetBucket(value);
            
            var newBucket = bucket
                .Where(v => !v.Equals(value))
                .ToList();

            if (newBucket.Count == bucket.Count)
            {
                throw new ArgumentException($"Value does not exist: {value}");
            }

            var newBuckets = _buckets.SetItem(index, newBucket);
            return new PersistentSet<T>(Count - 1, newBuckets);
        }


        public IPersistentSet<T> Add(T value) => Set(value);

        public IPersistentSet<T> Clear() => new PersistentSet<T>(0, _buckets.Clear());

        public IPersistentSet<T> Except(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public IPersistentSet<T> Intersect(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        IPersistentSet<T> IPersistentSet<T>.Remove(T value) => Remove(value);

        public IPersistentSet<T> SymmetricExcept(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public IPersistentSet<T> Union(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        IImmutableSet<T> IImmutableSet<T>.Clear() => Clear();

        public bool Contains(T value)
        {
            var (_, bucket) = GetBucket(value);
            return Enumerable.Contains(bucket, value);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        IImmutableSet<T> IImmutableSet<T>.Except(IEnumerable<T> other) => Except(other);

        IImmutableSet<T> IImmutableSet<T>.Intersect(IEnumerable<T> other) => Intersect(other);

        bool IImmutableSet<T>.IsProperSubsetOf(IEnumerable<T> other) => IsProperSubsetOf(other);

        bool IImmutableSet<T>.IsProperSupersetOf(IEnumerable<T> other) => IsProperSupersetOf(other);

        bool IImmutableSet<T>.IsSubsetOf(IEnumerable<T> other) => IsSubsetOf(other);

        bool IImmutableSet<T>.IsSupersetOf(IEnumerable<T> other) => IsSupersetOf(other);

        bool IImmutableSet<T>.Overlaps(IEnumerable<T> other) => Overlaps(other);

        IImmutableSet<T> IImmutableSet<T>.Remove(T value) => Remove(value);

        bool IImmutableSet<T>.SetEquals(IEnumerable<T> other) => SetEquals(other);

        IImmutableSet<T> IImmutableSet<T>.SymmetricExcept(IEnumerable<T> other) => SymmetricExcept(other);

        IImmutableSet<T> IImmutableSet<T>.Union(IEnumerable<T> other) => Union(other);
        
        public bool TryGetValue(T equalValue, out T actualValue)
        {
            var (_, bucket) = GetBucket(equalValue);
            foreach (var k in bucket)
            {
                if (k.Equals(equalValue))
                {
                    actualValue = k;
                    return true;
                }
            }

            actualValue = equalValue;
            return false;
        }
        
        public PersistentSet<T> Set(T value)
        {
            if (_buckets.Count < 0.67 * Count)
            {
                Reallocate(2 * _buckets.Count);
            }

            var (index, bucket) = GetBucket(value);
            if (bucket.Any(v => v.Equals(value)))
            {
                return Update(bucket, index, value);
            }

            var newBucket = new List<T>(bucket) {value};
            var newBuckets = _buckets.SetItem(index, newBucket);

            return new PersistentSet<T>(Count + 1, newBuckets);
        }

        private PersistentSet<T> Update(List<T> bucket, int index, T value)
        {
            var newBucket = bucket
                .Where(item => !item.Equals(value))
                .Append(value)
                .ToList();
            var newBuckets = _buckets.SetItem(index, newBucket);
            return new PersistentSet<T>(Count, newBuckets);
        }

        private void Reallocate(int newSize)
        {
            var array = Enumerable.Range(0, newSize).Select(i => new List<T>()).ToArray();
            foreach (var item in this)
            {
                var index = item.GetHashCode() % newSize;
                array[index].Add(item);
            }

            _buckets = new PersistentList<List<T>>().AddRange(array);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _buckets.SelectMany(bucket => bucket).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IPersistentSet<T> IPersistentDataStructure<T, IPersistentSet<T>>.Add(T value) => Set(value);

        public IPersistentSet<T> AddRange(IEnumerable<T> items)
        {
            return items.Aggregate(this, (current, item) => current.Set(item));
        }

        public IPersistentSet<T> AddRange(IReadOnlyCollection<T> items) => AddRange(items.AsEnumerable());

        IImmutableSet<T> IImmutableSet<T>.Add(T value) => Set(value);

        IPersistentSet<T> IPersistentDataStructure<T, IPersistentSet<T>>.Clear() => new PersistentSet<T>(0, _buckets.Clear());

        public bool IsEmpty => Count == 0;
    }
}