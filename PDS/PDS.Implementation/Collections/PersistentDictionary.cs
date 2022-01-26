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
        private const int InitialSize = 32;
        private IPersistentList<List<KeyValuePair<TKey, TValue>>> _buckets;

        public PersistentDictionary(int count, IPersistentList<List<KeyValuePair<TKey, TValue>>> buckets)
        {
            Count = count;
            _buckets = buckets;
        }

        public PersistentDictionary()
        {
            Count = 0;
            var array = Enumerable.Range(0, InitialSize).Select(i => new List<KeyValuePair<TKey, TValue>>()).ToArray();
            _buckets = new PersistentList<List<KeyValuePair<TKey, TValue>>>().AddRange(array);
        }

        public int Count { get; }

        public static PersistentDictionary<TKey, TValue> Empty => new();

        private (int index, List<KeyValuePair<TKey, TValue>> bucket) GetBucket(TKey key)
        {
            var index = key.GetHashCode() % _buckets.Count;
            if (index < 0)
            {
                index += _buckets.Count;
            }
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

            _buckets = new PersistentList<List<KeyValuePair<TKey, TValue>>>().AddRange(array);
        }

        public IPersistentDictionary<TKey, TValue> AddRange(
            IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            return pairs.Aggregate(this, (current, pair) => current.Set(pair.Key, pair.Value));
        }

        public IPersistentDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys)
        {
            var dict = this;
            return keys.Aggregate(dict, (current, key) => current.Remove(key));
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Remove(TKey key) => Remove(key);

        public bool TryRemove(TKey key, out IPersistentDictionary<TKey, TValue> newVersion)
        {
            var (index, bucket) = GetBucket(key);

            var newBucket = bucket
                .Where(kv => !kv.Key.Equals(key))
                .ToList();

            if (newBucket.Count == bucket.Count)
            {
                newVersion = this;
                return false;
            }

            var newBuckets = _buckets.SetItem(index, newBucket);
            newVersion = new PersistentDictionary<TKey, TValue>(Count - 1, newBuckets);
            return true;
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.SetItem(TKey key, TValue value) =>
            Set(key, value);

        public IPersistentDictionary<TKey, TValue> SetItems(
            IEnumerable<KeyValuePair<TKey, TValue>> items) =>
            items.Aggregate(this, (current, pair) => current.Set(pair.Key, pair.Value));

        public IPersistentDictionary<TKey, TValue> Clear() => new PersistentDictionary<TKey, TValue>();

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Add(TKey key, TValue value) =>
            Add(new KeyValuePair<TKey, TValue>(key, value));

        public IPersistentDictionary<TKey, TValue> AddOrUpdate(TKey key, TValue value) => Set(key, value);

        public IPersistentDictionary<TKey, TValue> Update(TKey key, Func<TKey, TValue, TValue> valueFactory)
        {
            var (_, bucket) = GetBucket(key);
            foreach (var (k, v) in bucket)
            {
                if (k.Equals(key))
                {
                    return Set(k, valueFactory(k, v));
                }
            }

            throw new KeyNotFoundException(key.ToString());
        }

        public bool TryAdd(TKey key, TValue value, out IPersistentDictionary<TKey, TValue> newVersion)
        {
            var (_, bucket) = GetBucket(key);
            foreach (var (k, v) in bucket)
            {
                if (k.Equals(key))
                {
                    newVersion = this;
                    return false;
                }
            }

            newVersion = Set(key, value);
            return true;
        }

        public bool Contains(KeyValuePair<TKey, TValue> pair)
        {
            return TryGetValue(pair.Key, out var value) && Equals(value, pair.Value);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Remove(TKey key) => Remove(key);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.RemoveRange(IEnumerable<TKey> keys) =>
            RemoveRange(keys);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItem(TKey key, TValue value) =>
            Set(key, value);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItems(
            IEnumerable<KeyValuePair<TKey, TValue>> items) => SetItems(items);

        bool IImmutableDictionary<TKey, TValue>.TryGetKey(TKey equalKey, out TKey actualKey)
        {
            var (_, bucket) = GetBucket(equalKey);
            foreach (var (k, _) in bucket)
            {
                if (k.Equals(equalKey))
                {
                    actualKey = k;
                    return true;
                }
            }

            actualKey = equalKey;
            return false;
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

        public IPersistentDictionary<TKey, TValue> Add(KeyValuePair<TKey, TValue> value) => Set(value.Key, value.Value);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Add(TKey key, TValue value) =>
            Set(key, value);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.AddRange(
            IEnumerable<KeyValuePair<TKey, TValue>> pairs) => AddRange(pairs);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Clear() => Clear();

        IPersistentDictionary<TKey, TValue>
            IPersistentDataStructure<KeyValuePair<TKey, TValue>, IPersistentDictionary<TKey, TValue>>.AddRange(
                IEnumerable<KeyValuePair<TKey, TValue>> items) => AddRange(items);

        public IPersistentDictionary<TKey, TValue> AddRange(IReadOnlyCollection<KeyValuePair<TKey, TValue>> items) =>
            AddRange(items.AsEnumerable());

        IPersistentDictionary<TKey, TValue>
            IPersistentDataStructure<KeyValuePair<TKey, TValue>, IPersistentDictionary<TKey, TValue>>.Clear() =>
            Clear();

        public bool IsEmpty => Count == 0;

        public bool ContainsKey(TKey key) => Contains(key);

#pragma warning disable CS8767
        public bool TryGetValue(TKey key, out TValue? value)
#pragma warning restore CS8767
        {
            var (_, bucket) = GetBucket(key);
            foreach (var (k, v) in bucket)
            {
                if (k.Equals(key))
                {
                    value = v;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public TValue this[TKey key] => GetByKey(key);

        public IEnumerable<TKey> Keys => this.Select(kvp => kvp.Key);
        public IEnumerable<TValue> Values => this.Select(kvp => kvp.Value);
    }
}