using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using PDS.Collections;

namespace PDS.Implementation.Collections
{
    public class PersistentDictionary<TKey, TValue> : IPersistentDictionary<TKey, TValue> where TKey : notnull
    {
        private IPersistentList<List<KeyValuePair<TKey, TValue>>> _buckets;

        public PersistentDictionary(int count, IPersistentList<List<KeyValuePair<TKey, TValue>>> buckets)
        {
            Count = count;
            _buckets = buckets;
        }

        private PersistentDictionary()
        {
            Count = 0;
            //TODO: List realisation 
            _buckets = null;
        }   

        public int Count { get; }
        
        public static PersistentDictionary<TKey, TValue> Empty => new();

        private (int index, List<KeyValuePair<TKey, TValue>> bucket) GetBucket(TKey key)
        {
            var index = key.GetHashCode() % _buckets.Count;
            return (index, _buckets[index]);
        }

        public TValue GetByKey(TKey key)
        {
            var (_, bucket) = GetBucket(key);
            foreach (var (k, v) in bucket)
            {
                if (k.Equals(key))
                {
                    return v;
                }
            }

            throw new KeyNotFoundException(key.ToString());
        }

        public bool Contains(TKey key)
        {
            var (_, bucket) = GetBucket(key);
            foreach (var (k, v) in bucket)
            {
                if (k.Equals(key))
                {
                    return true;
                }
            }

            return false;
        }

        public PersistentDictionary<TKey, TValue> Set(TKey key, TValue value)
        {
            if (_buckets.Count < 0.67 * Count)
            {
                Reallocate(2 * _buckets.Count);
            }

            var (index, bucket) = GetBucket(key);
            foreach (var (k, v) in bucket)
            {
                if (k.Equals(key))
                {
                    return Equals(v, value) ? this : Update(bucket, index, key, value);
                }
            }

            var newBucket = new List<KeyValuePair<TKey, TValue>>(bucket) {new(key, value)};
            var newBuckets = _buckets.SetItem(index, newBucket);

            return new PersistentDictionary<TKey, TValue>(Count + 1, newBuckets);
        }

        private PersistentDictionary<TKey, TValue> Update(List<KeyValuePair<TKey, TValue>> bucket, int index, TKey key,
            TValue value)
        {
            var newBucket = bucket
                .Where(kv => !kv.Key.Equals(key))
                .Append(new KeyValuePair<TKey, TValue>(key, value))
                .ToList();
            var newBuckets = _buckets.SetItem(index, newBucket);
            return new PersistentDictionary<TKey, TValue>(Count, newBuckets);
        }
        
        private void Reallocate(int newSize)
        {
            var array = Enumerable.Range(0, newSize).Select(i => new List<KeyValuePair<TKey, TValue>>()).ToArray();
            foreach (var keyValuePair in this)
            {
                var index = keyValuePair.Key.GetHashCode() % newSize;
                array[index].Add(keyValuePair);
            }

            //TODO: _buckets = new PersistentArray<List<KeyValuePair<TKey, TValue>>>(array);
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.RemoveRange(IEnumerable<TKey> keys)
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Remove(TKey key)
        {
            return Remove(key);
        }

        public bool TryRemove(TKey key, out IPersistentDictionary<TKey, TValue> newVersion)
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.SetItem(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Clear()
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public IPersistentDictionary<TKey, TValue> AddOrUpdate(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public IPersistentDictionary<TKey, TValue> Update(TKey key, Func<TKey, TValue, TValue> valueFactory)
        {
            throw new NotImplementedException();
        }

        public bool TryAdd(TKey key, TValue value, out IPersistentDictionary<TKey, TValue> newVersion)
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> pair)
        {
            throw new NotImplementedException();
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Remove(TKey key)
        {
            return Remove(key);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.RemoveRange(IEnumerable<TKey> keys)
        {
            throw new NotImplementedException();
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItem(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            throw new NotImplementedException();
        }

        bool IImmutableDictionary<TKey, TValue>.TryGetKey(TKey equalKey, out TKey actualKey)
        {
            throw new NotImplementedException();
        }

        public PersistentDictionary<TKey, TValue> Remove(TKey key)
        {
            var (index, bucket) = GetBucket(key);
            
            var newBucket = bucket
                .Where(kv => !kv.Key.Equals(key))
                .ToList();

            if (newBucket.Count == bucket.Count)
            {
                throw new ArgumentException($"Key does not exist: {key}");
            }

            var newBuckets = _buckets.SetItem(index, newBucket);
            return new PersistentDictionary<TKey, TValue>(Count - 1, newBuckets);
        }


        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _buckets.SelectMany(bucket => bucket).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IPersistentDictionary<TKey, TValue> Add(KeyValuePair<TKey, TValue> value)
        {
            throw new NotImplementedException();
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            throw new NotImplementedException();
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Clear()
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDataStructure<KeyValuePair<TKey, TValue>, IPersistentDictionary<TKey, TValue>>.AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            throw new NotImplementedException();
        }

        public IPersistentDictionary<TKey, TValue> AddRange(IReadOnlyCollection<KeyValuePair<TKey, TValue>> items)
        {
            throw new NotImplementedException();
        }

        IPersistentDictionary<TKey, TValue> IPersistentDataStructure<KeyValuePair<TKey, TValue>, IPersistentDictionary<TKey, TValue>>.Clear()
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty => Count == 0;
        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        public TValue this[TKey key] => throw new NotImplementedException();

        public IEnumerable<TKey> Keys => this.Select(kvp => kvp.Key);
        public IEnumerable<TValue> Values => this.Select(kvp => kvp.Value);
    }
}