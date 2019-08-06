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
    public interface IOcLockReadUpgrade : IOcLockReadWrite
    {
        /// <summary>
        /// True if the lock held can be upgraded to a write lock (by calling an upgrade method, not by directly obtaining a write lock).
        /// </summary>
        bool CanUpgrade { get; }
    }
}