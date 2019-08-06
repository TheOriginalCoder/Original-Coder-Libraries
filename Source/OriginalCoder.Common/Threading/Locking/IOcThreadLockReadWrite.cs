//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2011, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using JetBrains.Annotations;
using OriginalCoder.Common.Threading.Locking.Locks;

namespace OriginalCoder.Common.Threading.Locking
{
    /// <summary>
    /// Standard interface for obtaining read and write locks for thread-safe data sharing.
    /// </summary>
    [PublicAPI]
    public interface IOcThreadLockReadWrite : IOcThreadLock
    {
        /// <summary>
        /// Return true if a read lock is held by the current thread or false if no read lock is currently held by the thread.
        /// Implementations that can not determine if a lock is held will return null.
        /// </summary>
        bool? HasReadLock { get; }

        /// <summary>
        /// Return true if either a read or write lock is held by the current thread or false if no lock is currently held by the thread.
        /// Implementations that can not determine if a lock is held will return null.
        /// </summary>
        bool? HasAnyLock { get; }

        /// <summary>
        /// Obtains a Read lock and returns an <see cref="IDisposable"/> that
        /// MUST BE DISPOSED as soon as the write lock is no longer needed.
        /// </summary>
        IOcLockReadWrite ReadLock();
    }
}