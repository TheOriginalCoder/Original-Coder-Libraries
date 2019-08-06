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
    /// Thread-safe implementation of <see cref="List{T}"/> that implements separate read & write locking for efficiency (so that many read locks can be held concurrently if there is no write lock in effect).
    /// For code that needs to perform multiple read and/or write operations that are related obtain a <see cref="ReadLock"/> or <see cref="WriteLock"/> within a C# using statement to ensure the operations access a consistent view of the list.
    /// WARNING: The calling code must currently hold either a <see cref="ReadLock"/> or <see cref="WriteLock"/> to use any of the methods that enumerate over data in the list.
    /// </summary>
    [PublicAPI]
    public sealed class OcThreadSafeList<T> : IOcThreadSafeList<T>, IOcThreadSafeReadOnlyList<T>, IName
    {
      #region Constructors

        public OcThreadSafeList()
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _list = new List<T>();
        }

        public OcThreadSafeList(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _list = new List<T>();
        }

        public OcThreadSafeList(IEnumerable<T> collection)
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _list = new List<T>(collection);
        }

        public OcThreadSafeList(string name, IEnumerable<T> collection)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _list = new List<T>(collection);
        }

        public OcThreadSafeList(int capacity)
        {
            Name = $"Un-Named {GetType().FriendlyName()}";
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _list = new List<T>(capacity);
        }

        public OcThreadSafeList(string name, int capacity)
        {
            Name = string.IsNullOrWhiteSpace(name) ? $"Un-Named {GetType().FriendlyName()}" : name.Trim();
            _lock = new OcThreadLockReadWrite($"Lock for {Name}");
            _list = new List<T>(capacity);
        }

      #endregion

        private readonly IOcThreadLockReadWrite _lock;
        private readonly List<T> _list;

        public string Name { get; }

      #region Locking

        /// <inheritdoc cref=" IOcThreadSafeList{T}.ReadLock"/>
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
                throw new OcThreadLockException($"Either a Read or Write Lock must be held before enumerating {nameof(IOcThreadSafeList<T>)} {Name}", this);
            return _list.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_lock.HasAnyLock != true)
                throw new OcThreadLockException($"Either a Read or Write Lock must be held before enumerating {nameof(IOcThreadSafeList<T>)} {Name}", this);
            return _list.GetEnumerator();
        }

      #endregion

      #region ICollection<T>

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count => _list.Count;  // This operation is atomic enough that we can cheat and skip locking

        /// <inheritdoc />
        public bool IsReadOnly => (_list as ICollection<T>).IsReadOnly;

        /// <inheritdoc />
        public bool Contains(T item)
        {
            if (_lock.HasAnyLock == true)
            {
                return _list.Contains(item);
            }

            using (var readLock = ReadLock())
            {
                readLock.Lock();
                return _list.Contains(item);
            }
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (_lock.HasAnyLock == true)
            {
                (_list as ICollection<T>).CopyTo(array, arrayIndex);
                return;
            }

            using (var readLock = ReadLock())
            {
                readLock.Lock();
                (_list as ICollection<T>).CopyTo(array, arrayIndex);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            if (_lock.HasWriteLock == true)
            {
                _list.Clear();
                return;
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                _list.Clear();
            }
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            if (_lock.HasWriteLock == true)
            {
                _list.Add(item);
                return;
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                _list.Add(item);
            }
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            if (_lock.HasWriteLock == true)
            {
                return _list.Remove(item);
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                return _list.Remove(item);
            }
        }

      #endregion

      #region IList<T>

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            if (_lock.HasAnyLock == true)
            {
                return _list.IndexOf(item);
            }

            using (var readLock = ReadLock())
            {
                readLock.Lock();
                return _list.IndexOf(item);
            }
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            if (_lock.HasWriteLock == true)
            {
                _list.Insert(index, item);
                return;
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                _list.Insert(index, item);
            }
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            if (_lock.HasWriteLock == true)
            {
                _list.RemoveAt(index);
                return;
            }

            using (var writeLock = WriteLock())
            {
                writeLock.Lock();
                _list.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get
            {
                if (_lock.HasAnyLock == true)
                {
                    return _list[index];
                }

                using (var readLock = ReadLock())
                {
                    readLock.Lock();
                    return _list[index];
                }
            }
            set
            {
                if (_lock.HasWriteLock == true)
                {
                    _list[index] = value;
                    return;
                }

                using (var writeLock = WriteLock())
                {
                    writeLock.Lock();
                    _list[index] = value;
                }
            }
        }

      #endregion
    }
}
