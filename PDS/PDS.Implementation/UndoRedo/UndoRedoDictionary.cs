using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using PDS.Collections;
using PDS.Implementation.Collections;
using PDS.UndoRedo;

namespace PDS.Implementation.UndoRedo
{
    public class UndoRedoDictionary<TKey, TValue> : IUndoRedoDictionary<TKey, TValue> 
        where TKey : notnull
    {
        private readonly IPersistentDictionary<TKey, TValue> _persistentDictionary;
        private readonly IPersistentStack<IPersistentDictionary<TKey, TValue>> _undoStack;
        private readonly IPersistentStack<IPersistentDictionary<TKey, TValue>> _redoStack;

        public UndoRedoDictionary()
        {
            _persistentDictionary = PersistentDictionary<TKey, TValue>.Empty;
            _undoStack = PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty;
            _redoStack = PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty;
        }
        
        public UndoRedoDictionary(IPersistentDictionary<TKey, TValue> persistentDictionary, 
            IPersistentStack<IPersistentDictionary<TKey, TValue>> undoStack, 
            IPersistentStack<IPersistentDictionary<TKey, TValue>> redoStack)
        {
            _persistentDictionary = persistentDictionary;
            _undoStack = undoStack;
            _redoStack = redoStack;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _persistentDictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _persistentDictionary.Count;
        IPersistentDictionary<TKey, TValue> 
            IPersistentDataStructure<KeyValuePair<TKey, TValue>, IPersistentDictionary<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> value)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Add(value), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IUndoRedoDictionary<TKey, TValue> 
            IUndoRedoDictionary<TKey, TValue>.AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.AddRange(pairs), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IUndoRedoDictionary<TKey, TValue> IUndoRedoDictionary<TKey, TValue>.RemoveRange(IEnumerable<TKey> keys)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.RemoveRange(keys), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IUndoRedoDictionary<TKey, TValue> IUndoRedoDictionary<TKey, TValue>.Remove(TKey key)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Remove(key), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        public bool TryRemove(TKey key, out IUndoRedoDictionary<TKey, TValue> newVersion)
        {
            if (_persistentDictionary.TryRemove(key, out var version))
            {
                var u = _undoStack.Push(_persistentDictionary);
                newVersion = new UndoRedoDictionary<TKey, TValue>(version, u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
                return true;
            }

            newVersion = this;
            return false;
        }

        IUndoRedoDictionary<TKey, TValue> IUndoRedoDictionary<TKey, TValue>.SetItem(TKey key, TValue value)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.SetItem(key, value), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IUndoRedoDictionary<TKey, TValue> IUndoRedoDictionary<TKey, TValue>.SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.SetItems(items), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IUndoRedoDictionary<TKey, TValue> IUndoRedoDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Add(key, value), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IUndoRedoDictionary<TKey, TValue> IUndoRedoDictionary<TKey, TValue>.AddOrUpdate(TKey key, TValue value)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.AddOrUpdate(key, value), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IUndoRedoDictionary<TKey, TValue> IUndoRedoDictionary<TKey, TValue>.Update(TKey key, Func<TKey, TValue, TValue> valueFactory)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Update(key, valueFactory), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        public bool TryAdd(TKey key, TValue value, out IUndoRedoDictionary<TKey, TValue> newVersion)
        {
            if (_persistentDictionary.TryAdd(key, value, out var version))
            {
                var u = _undoStack.Push(_persistentDictionary);
                newVersion = new UndoRedoDictionary<TKey, TValue>(version, u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
                return true;
            }

            newVersion = this;
            return false;
        }

        IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>> 
            IPersistentDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>>>.AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.AddRange(items), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IUndoRedoDictionary<TKey, TValue> IUndoRedoDictionary<TKey, TValue>.Clear()
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Clear(), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>> 
            IPersistentDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>>>.AddRange(IReadOnlyCollection<KeyValuePair<TKey, TValue>> items)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.AddRange(items), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>> 
            IPersistentDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>>>.Clear()
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Clear(), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>> 
            IPersistentDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>>>.Add(KeyValuePair<TKey, TValue> value)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Add(value), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.AddRange(pairs), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.RemoveRange(IEnumerable<TKey> keys)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.RemoveRange(keys), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Remove(TKey key)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Remove(key), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        public bool TryRemove(TKey key, out IPersistentDictionary<TKey, TValue> newVersion)
        {
            if (_persistentDictionary.TryRemove(key, out var version))
            {
                var u = _undoStack.Push(_persistentDictionary);
                newVersion = new UndoRedoDictionary<TKey, TValue>(version, u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
                return true;
            }

            newVersion = this;
            return false;
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.SetItem(TKey key, TValue value)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.SetItem(key, value), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.SetItems(items), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Clear()
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Clear(), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Add(key, value), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.AddOrUpdate(TKey key, TValue value)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.AddOrUpdate(key, value), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IPersistentDictionary<TKey, TValue> IPersistentDictionary<TKey, TValue>.Update(TKey key, Func<TKey, TValue, TValue> valueFactory)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Update(key, valueFactory), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        public bool TryAdd(TKey key, TValue value, out IPersistentDictionary<TKey, TValue> newVersion)
        {
            return ((IUndoRedoDictionary<TKey, TValue>)this).TryAdd(key, value, out newVersion);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Add(key, value), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.AddRange(pairs), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Clear()
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Clear(), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        public bool Contains(KeyValuePair<TKey, TValue> pair) => _persistentDictionary.Contains(pair);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Remove(TKey key)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Remove(key), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.RemoveRange(IEnumerable<TKey> keys)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.RemoveRange(keys), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItem(TKey key, TValue value)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.SetItem(key, value), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.SetItems(items), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        public bool TryGetKey(TKey equalKey, out TKey actualKey) =>
            _persistentDictionary.TryGetKey(equalKey, out actualKey);

        IPersistentDictionary<TKey, TValue> 
            IPersistentDataStructure<KeyValuePair<TKey, TValue>, IPersistentDictionary<TKey, TValue>>.AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.AddRange(items), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IPersistentDictionary<TKey, TValue> 
            IPersistentDataStructure<KeyValuePair<TKey, TValue>, IPersistentDictionary<TKey, TValue>>.AddRange(IReadOnlyCollection<KeyValuePair<TKey, TValue>> items)
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.AddRange(items), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        IPersistentDictionary<TKey, TValue> 
            IPersistentDataStructure<KeyValuePair<TKey, TValue>, IPersistentDictionary<TKey, TValue>>.Clear()
        {
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(_persistentDictionary.Clear(), u, PersistentStack<IPersistentDictionary<TKey, TValue>>.Empty);
        }

        public bool IsEmpty => _persistentDictionary.IsEmpty;
        public bool ContainsKey(TKey key) => _persistentDictionary.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value) => _persistentDictionary.TryGetValue(key, out value);

        public TValue this[TKey key] => _persistentDictionary[key];

        public IEnumerable<TKey> Keys => _persistentDictionary.Keys;
        public IEnumerable<TValue> Values => _persistentDictionary.Values;
        public IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>> Redo()
        {
            if (_redoStack.IsEmpty)
            {
                throw new InvalidOperationException("Redo stack is empty");
            }

            var lastVersion = _redoStack.Peek();
            var r = _redoStack.Pop();
            var u = _undoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(lastVersion, u, r);
        }

        public bool TryRedo(out IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>> collection)
        {
            if (_redoStack.IsEmpty)
            {
                collection = this;
                return false;
            }

            collection = Redo();
            return true;
        }

        public bool CanRedo => !_redoStack.IsEmpty;
        public IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>> Undo()
        {
            if (_undoStack.IsEmpty)
            {
                throw new InvalidOperationException("Undo stack is empty");
            }

            var lastVersion = _undoStack.Peek();
            var u = _undoStack.Pop();
            var r = _redoStack.Push(_persistentDictionary);
            return new UndoRedoDictionary<TKey, TValue>(lastVersion, u, r);
        }

        public bool TryUndo(out IUndoRedoDataStructure<KeyValuePair<TKey, TValue>, IUndoRedoDictionary<TKey, TValue>> collection)
        {
            if (_undoStack.IsEmpty)
            {
                collection = this;
                return false;
            }

            collection = Undo();
            return true;
        }

        public bool CanUndo => !_undoStack.IsEmpty;
    }
}