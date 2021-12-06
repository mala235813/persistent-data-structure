using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PDS.Collections;

namespace PDS.Implementation.Collections
{
    public class FatNodeArray<T> : IPersistentArray<T>
    {
        private readonly int _versionId; //TODO: version generation
        private readonly FatNode<T>[] _nodes;

        internal FatNodeArray(int count, int versionId = 0)
        {
            _versionId = versionId;
            //TODO: Handle null references when T is non nullable
            _nodes = Enumerable.Range(0, count).Select(_ => new FatNode<T>(_versionId, default(T))).ToArray();
        }

        internal FatNodeArray(IEnumerable<T> items, int versionId = 0)
        {
            _versionId = versionId;
            _nodes = items.Select(i => new FatNode<T>(_versionId, i)).ToArray();
        }

        private FatNodeArray(FatNode<T>[] nodes, int versionId)
        {
            _versionId = versionId;
            _nodes = nodes;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _nodes.Select(node => node.GetValue(_versionId)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _nodes.Length;

        public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
        {
            equalityComparer = ;
//            this.Skip(index).Take(count).Fir

            if (index >= Count)
            {
                throw new ArgumentException($"Invalid starting index: {index} is greater than {Count}");
            }

            if (index + count >= Count)
            {
                throw new ArgumentException($"Invalid search range: {index} + {count} is greater than {Count}");
            }

            if (equalityComparer is null)
            {
                for (var i = index; i < index + count; i++)
                {
                    if (_nodes[i].GetValue(_versionId).Equals(item))
                    {
                        return i;
                    }
                }
            }

            for (var i = index; i < index + count; i++)
            {
                if (equalityComparer?.Equals(item, _nodes[i].GetValue(_versionId)) == true)
                {
                    return i;
                }
            }

            return -1;
        }

        public IPersistentArray<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
        {
            throw new System.NotImplementedException();
        }

        public IPersistentArray<T> SetItem(int index, T value)
        {
            var nextVersion = _versionId + 1;
            _nodes[index].Add(nextVersion, value);

            return new FatNodeArray<T>(_nodes, nextVersion);
        }

        public T this[int index] => _nodes[index].GetValue(_versionId);
        
        public IPersistentArray<T> Add(T value)
        {
            throw new NotImplementedException();
        }

        public IPersistentArray<T> AddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public IPersistentArray<T> Clear()
        {
            throw new NotImplementedException();
        }
    }
}