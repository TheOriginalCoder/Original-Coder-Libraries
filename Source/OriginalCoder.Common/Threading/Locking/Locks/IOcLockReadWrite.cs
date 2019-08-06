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

namespace OriginalCoder.Common.Threading.Locking.Locks
{
    /// <summary>
    /// Interface used to hold a thread lock and release it via <see cref="IDisposable"/>.
    /// </summary>
    [PublicAPI]
    public interface IOcLockReadWrite : IDisposable
    {
        /// <summary>
        /// Called to activate the lock.  This should be called from within a using or try-finally block.
        /// </summary>
        bool Lock();

        /// <summary>
        /// True if the lock held allows writing to the shared data.
        /// </summary>
        bool IsWriteLock { get; }

        /// <summary>
        /// True if the lock held allows a write lock to be obtained directly (without upgrading).
        /// </summary>
        bool CanWriteLock { get; }
    }
}