//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using JetBrains.Annotations;
using OriginalCoder.Common.Threading.Locking.Locks;

namespace OriginalCoder.Common.Threading.Locking
{
    /// <summary>
    /// Standard interface for obtaining read locks, read locks that can be upgraded to write locks and write locks for thread-safe data sharing.
    /// </summary>
    [PublicAPI]
    public interface IOcThreadLockReadUpgrade : IOcThreadLockReadWrite
    {
        /// <summary>
        /// Obtains a Read lock and returns an <see cref="IDisposable"/> that
        /// MUST BE DISPOSED as soon as the write lock is no longer needed.
        /// </summary>
        new IOcLockReadUpgrade ReadLock();

        /// <summary>
        /// Obtains an Upgradable Read lock and returns an <see cref="IDisposable"/> that
        /// MUST BE DISPOSED as soon as the write lock is no longer needed.
        /// </summary>
        IOcLockReadUpgrade ReadUpgradableLock();

        /// <summary>
        /// Obtains an Upgraded Write lock and returns an <see cref="IDisposable"/> that
        /// MUST BE DISPOSED as soon as the write lock is no longer needed.
        /// </summary>
        IOcLockReadUpgrade UpgradeLock();

        /// <summary>
        /// Obtains a Write lock and returns an <see cref="IDisposable"/> that
        /// MUST BE DISPOSED as soon as the write lock is no longer needed.
        /// </summary>
        new IOcLockReadUpgrade WriteLock();
    }
}