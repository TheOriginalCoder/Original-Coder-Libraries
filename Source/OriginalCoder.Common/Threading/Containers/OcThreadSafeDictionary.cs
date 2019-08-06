//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using OriginalCoder.Common.Extensions;
using OriginalCoder.Common.Interfaces.Properties;
using OriginalCoder.Common.Threading.Locking;
using OriginalCoder.Common.Threading.Locking.Concrete;
using OriginalCoder.Common.Threading.Locking.Locks;

namespace OriginalCoder.Common.Threading.Containers
{
    /// <summary>
    /// Thread-safe implementation of <see cref="Dictionary{TK,TV}"/> that implements separate read & write locking for efficiency (so that many read locks can be held concurrently if there is no write lock in effect).
    /// For code that needs to perform multiple read and/or write operations that are related obtain a <see cref="ReadLock"/> or <see cref="WriteLock"/> within a C# using statement to ensure the operations access a consistent view of the dictionary.
    /// WARNING: The calling code must currently hold either a <see cref="ReadLock"/> or <see cref="WriteLock"/> to use any of the methods that enumerate over data in the dictionary.
    /// </summary>
    /// <typeparam name="TKey">Type used for the dictionary key.</typeparam>
    /// <typeparam name="TValue">Type for the values stored by the dictionary.</typeparam>
    [PublicAPI]
    public sealed class OcThreadSafeDictionary<TKey, TValue> : IOcThreadSafeDictionary<TKey, TValue>, IOcThreadSafeReadOnlyDictionary<TKey, TValue>, IName
    {
      #region Constructors

        public OcThreadSafeDictionary()
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public OcThreadSafeDictionary(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public OcThreadSafeDictionary(IDictionary<TKey, TValue> dictionary)
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        public OcThreadSafeDictionary(string name, IDictionary<TKey, TValue> dictionary)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        public OcThreadSafeDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public OcThreadSafeDictionary(string name, IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public OcThreadSafeDictionary(IEqualityComparer<TKey> comparer)
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public OcThreadSafeDictionary(string name, IEqualityComparer<TKey> comparer)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public OcThreadSafeDictionary(int capacity)
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public OcThreadSafeDictionary(string name, int capacity)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public OcThreadSafeDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        public OcThreadSafeDictionary(string name, int capacity, IEqualityComparer<TKey> comparer)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

      #endregion

        private readonly IOcThreadLockReadWrite _lock;
        private readonly Dictionary<TKey, TValue> _dictionary;

        public string Name { get; }

      #region Locking

        /// <inheritdoc cref=" IOcThreadSafeReadOnlyDictionary{TK,TV}.ReadLock"/>
        public IOcLockReadWrite ReadLock()
            => _lock.ReadLock();

        /// <inheritdoc />
        public IOcLockReadWrite WriteLock()
            => _lock.WriteLock();

      #endregion

      #region IEnumerable

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (_lock.HasAnyLock != true)
                throw new OcThreadLockException($"Either a Read or Write Lock must be held before enumerating {nameof(OcThreadSafeDictionary<TKey,TValue>)} {Name}", this);
            return _dictionary.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_lock.HasAnyLock != true)
                throw new OcThreadLockException($"Either a Read or Write Lock must be held before enumerating {nameof(OcThreadSafeDictionary<TKey, TValue>)} {Name}", this);
            return _dictionary.GetEnumerator();
        }

      #endregion

      #region ICollection<KeyValuePair<TK,TV>>

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count => _dictionary.Count;  // This operation is atomic enough that we can cheat and skip locking

        /// <inheritdoc />
        public bool IsReadOnly => (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).IsReadOnly;

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (_lock.HasAnyLock == true)
            {
                return (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
            }

            using (var readLock = ReadLock())
            {
                readLock.Lock();
                return (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
            }
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (_lock.HasAnyLock == true)
            {
                (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
                return;
            }

            using (var readLock = ReadLock())
            {
                readLock.Lock();
                (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            if (_lock.HasWriteLock == true)
            {
                _dictionary.Clear();
                return;
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                _dictionary.Clear();
            }
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (_lock.HasWriteLock == true)
            {
                (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
                return;
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
            }
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_lock.HasWriteLock == true)
            {
                return (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                return (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
            }
        }

      #endregion

      #region IReadOnlyDictionary<TK,TV>

        /// <inheritdoc />
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get
            {
                if (_lock.HasAnyLock != true)
                    throw new OcThreadLockException($"Either a Read or Write Lock must be held before enumerating the Keys of {nameof(OcThreadSafeDictionary<TKey, TValue>)} {Name}", this);
                return _dictionary.Keys;
            }
        }

        /// <inheritdoc />
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get
            {
                if (_lock.HasAnyLock != true)
                    throw new OcThreadLockException($"Either a Read or Write Lock must be held before enumerating the Values of {nameof(OcThreadSafeDictionary<TKey, TValue>)} {Name}", this);
                return _dictionary.Values;
            }
        }

      #endregion

      #region IDictionary<TK,TV>

        /// <inheritdoc cref="IDictionary{TKey,TValue}.TryGetValue"/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_lock.HasAnyLock == true)
            {
                return _dictionary.TryGetValue(key, out value);
            }

            using (var readLock = ReadLock())
            {
                readLock.Lock();
                return _dictionary.TryGetValue(key, out value);
            }
        }

        /// <inheritdoc cref="IDictionary{TK,TV}.ContainsKey"/>
        public bool ContainsKey(TKey key)
        {
            if (_lock.HasAnyLock == true)
            {
                return _dictionary.ContainsKey(key);
            }

            using (var readLock = ReadLock())
            {
                readLock.Lock();
                return _dictionary.ContainsKey(key);
            }
        }

        /// <inheritdoc />
        public ICollection<TValue> Values
        {
            get
            {
                if (_lock.HasAnyLock == true)
                {
                    return _dictionary.Values;
                }

                // WARNING: This could potentially be expensive if there are many values in the dictionary
                using (var readLock = ReadLock())
                {
                    readLock.Lock();
                    return _dictionary.Values.ToList();
                }
            }
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}.this"/>
        public TValue this[TKey key]
        {
            get
            {
                if (_lock.HasAnyLock == true)
                {
                    return _dictionary[key];
                }

                using (var readLock = ReadLock())
                {
                    readLock.Lock();
                    return _dictionary[key];
                }
            }
            set
            {
                if (_lock.HasWriteLock == true)
                {
                    _dictionary[key] = value;
                    return;
                }

                using (var writeLock = WriteLock())
                {
                    writeLock.Lock();
                    _dictionary[key] = value;
                }
            }
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys
        {
            get
            {
                if (_lock.HasAnyLock == true)
                {
                    return _dictionary.Keys;
                }

                // WARNING: This could potentially be expensive if there are many keys in the dictionary
                using (var readLock = ReadLock())
                {
                    readLock.Lock();
                    return _dictionary.Keys.ToList();
                }
            }
        }

        /// <inheritdoc />
        public void Add(TKey key, TValue value)
        {
            if (_lock.HasWriteLock == true)
            {
                _dictionary.Add(key, value);
                return;
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                _dictionary.Add(key, value);
            }
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            if (_lock.HasWriteLock == true)
            {
                return _dictionary.Remove(key);
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                return _dictionary.Remove(key);
            }
        }

      #endregion
    }
}
