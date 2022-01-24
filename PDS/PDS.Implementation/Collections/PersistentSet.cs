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
        private IPersistentList<List<T>> _buckets;

        public static PersistentSet<T> Empty { get; } = new();

        public PersistentSet(int count, IPersistentList<List<T>> buckets)
        {
            Count = count;
            _buckets = buckets;
        }
        
        private PersistentSet()
        {
            Count = 0;
            // TODO: persistent list
            _buckets = null;
        }

        public int Count { get; }

        private (int index, List<T> bucket) GetBucket(T value)
        {
            var index = value.GetHashCode() % _buckets.Count;
            return (index, _buckets[index]);
        }

        public T GetByKey(T key)
        {
            var (_, bucket) = GetBucket(key);
            foreach (var k in bucket)
            {
                if (k.Equals(key))
                {
                    return k;
                }
            }

            throw new KeyNotFoundException(key.ToString());
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


        IPersistentSet<T> IPersistentSet<T>.Add(T value) => Set(value);

        public IPersistentSet<T> Clear() => new PersistentSet<T>(0, _buckets.Clear());

        IPersistentSet<T> IPersistentSet<T>.Except(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        IPersistentSet<T> IPersistentSet<T>.Intersect(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        IPersistentSet<T> IPersistentSet<T>.Remove(T value) => Remove(value);

        IPersistentSet<T> IPersistentSet<T>.SymmetricExcept(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        IPersistentSet<T> IPersistentSet<T>.Union(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        IImmutableSet<T> IImmutableSet<T>.Clear() => Clear();

        public bool Contains(T value)
        {
            var (_, bucket) = GetBucket(value);
            return Enumerable.Contains(bucket, value);
        }

        bool IReadOnlySet<T>.IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        bool IReadOnlySet<T>.IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        bool IReadOnlySet<T>.IsSubsetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        bool IReadOnlySet<T>.IsSupersetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        bool IReadOnlySet<T>.Overlaps(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        bool IReadOnlySet<T>.SetEquals(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        IImmutableSet<T> IImmutableSet<T>.Except(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        IImmutableSet<T> IImmutableSet<T>.Intersect(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        bool IImmutableSet<T>.IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        bool IImmutableSet<T>.IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        bool IImmutableSet<T>.IsSubsetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        bool IImmutableSet<T>.IsSupersetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        bool IImmutableSet<T>.Overlaps(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        IImmutableSet<T> IImmutableSet<T>.Remove(T value) => Remove(value);

        bool IImmutableSet<T>.SetEquals(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        IImmutableSet<T> IImmutableSet<T>.SymmetricExcept(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(T equalValue, out T actualValue)
        {
            throw new System.NotImplementedException();
        }

        IImmutableSet<T> IImmutableSet<T>.Union(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
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

            //TODO: _buckets = new PersistentArray<List<T>>(array);
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