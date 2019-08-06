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
using OriginalCoder.Common.Interfaces.Properties;
using OriginalCoder.Common.Threading.Locking.Locks;

namespace OriginalCoder.Common.Threading.Locking
{
    /// <summary>
    /// Standard interface for obtaining write locks for thread-safe data sharing.
    /// </summary>
    [PublicAPI]
    public interface IOcThreadLock : IName
    {
        /// <summary>
        /// Return true if a write lock is held by the current thread or false if no write lock is currently held by the thread.
        /// Implementations that can not determine if a lock is held will return null.
        /// </summary>
        bool? HasWriteLock { get; }

        /// <summary>
        /// Obtains a Write lock and returns an <see cref="IDisposable"/> that
        /// MUST BE DISPOSED as soon as the write lock is no longer needed.
        /// </summary>
        IOcLockReadWrite WriteLock();
    }
}