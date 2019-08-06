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
using JetBrains.Annotations;
using OriginalCoder.Common.Extensions;
using OriginalCoder.Common.Interfaces.Properties;
using OriginalCoder.Common.Threading.Locking;
using OriginalCoder.Common.Threading.Locking.Concrete;
using OriginalCoder.Common.Threading.Locking.Locks;

namespace OriginalCoder.Common.Threading.Containers
{
    /// <summary>
    /// Thread-safe implementation of <see cref="HashSet{T}"/> that implements separate read & write locking for efficiency (so that many read locks can be held concurrently if there is no write lock in effect).
    /// For code that needs to perform multiple read and/or write operations that are related obtain a <see cref="ReadLock"/> or <see cref="WriteLock"/> within a C# using statement to ensure the operations access a consistent view of the hash set.
    /// WARNING: The calling code must currently hold either a <see cref="ReadLock"/> or <see cref="WriteLock"/> to use any of the methods that enumerate over data in the hash set.
    /// </summary>
    [PublicAPI]
    public sealed class OcThreadSafeHashSet<T> : IOcThreadSafeHashSet<T>, IOcThreadSafeReadOnlyCollection<T>, IName
    {
      #region Constructors

        public OcThreadSafeHashSet()
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _hashSet = new HashSet<T>();
        }

        public OcThreadSafeHashSet(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _hashSet = new HashSet<T>();
        }

        public OcThreadSafeHashSet(IEqualityComparer<T> comparer)
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _hashSet = new HashSet<T>(comparer);
        }

        public OcThreadSafeHashSet(string name, IEqualityComparer<T> comparer)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _hashSet = new HashSet<T>(comparer);
        }

        public OcThreadSafeHashSet(IEnumerable<T> collection)
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _hashSet = new HashSet<T>(collection);
        }

        public OcThreadSafeHashSet(string name, IEnumerable<T> collection)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _hashSet = new HashSet<T>(collection);
        }

        public OcThreadSafeHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _hashSet = new HashSet<T>(collection, comparer);
        }

        public OcThreadSafeHashSet(string name, IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _hashSet = new HashSet<T>(collection, comparer);
        }

      #endregion

        private readonly IOcThreadLockReadWrite _lock;
        private readonly HashSet<T> _hashSet;

        public string Name { get; }

      #region Locking

        /// <inheritdoc cref=" IOcThreadSafeHashSet{T}.ReadLock"/>
        public IOcLockReadWrite ReadLock()
            => _lock.ReadLock();

        /// <inheritdoc />
        public IOcLockReadWrite WriteLock()
            => _lock.WriteLock();

      #endregion

      #region IEnumerable

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            if (_lock.HasAnyLock != true)
                throw new OcThreadLockException($"Either a Read or Write Lock must be held before enumerating {nameof(OcThreadSafeHashSet<T>)} {Name}", this);
            return _hashSet.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_lock.HasAnyLock != true)
                throw new OcThreadLockException($"Either a Read or Write Lock must be held before enumerating {nameof(OcThreadSafeHashSet<T>)} {Name}", this);
            return _hashSet.GetEnumerator();
        }

      #endregion

      #region ICollection<T>

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count => _hashSet.Count;  // This operation is atomic enough that we can cheat and skip locking

        /// <inheritdoc />
        public bool IsReadOnly => (_hashSet as ICollection<T>).IsReadOnly;

        /// <inheritdoc />
        public bool Contains(T item)
        {
            if (_lock.HasAnyLock == true)
            {
                return _hashSet.Contains(item);
            }

            using (var readLock = ReadLock())
            {
                readLock.Lock();
                return _hashSet.Contains(item);
            }
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (_lock.HasAnyLock == true)
            {
                (_hashSet as ICollection<T>).CopyTo(array, arrayIndex);
                return;
            }

            using (var readLock = ReadLock())
            {
                readLock.Lock();
                (_hashSet as ICollection<T>).CopyTo(array, arrayIndex);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            if (_lock.HasWriteLock == true)
            {
                _hashSet.Clear();
                return;
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                _hashSet.Clear();
            }
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            if (_lock.HasWriteLock == true)
            {
                _hashSet.Add(item);
                return;
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                _hashSet.Add(item);
            }
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            if (_lock.HasWriteLock == true)
            {
                return _hashSet.Remove(item);
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                return _hashSet.Remove(item);
            }
        }

      #endregion
    }
}
