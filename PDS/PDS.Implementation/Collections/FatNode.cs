using System;
using System.Collections.Generic;

namespace PDS.Implementation.Collections
{
    internal sealed class FatNode<T>
    {
        private readonly Dictionary<int, T> _values = new();

        public FatNode(int versionId, T item)
        {
            _values[versionId] = item;
        }

        internal void Add(int versionId, T value)
        {
            if (!_values.TryAdd(versionId, value))
            {
                throw new ArgumentException($"Version {versionId} already exists");
            }
        }

        public T GetValue(int versionId)
        {
            return _values[versionId];
        }
    }
}