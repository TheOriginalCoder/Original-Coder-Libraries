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
using System.Collections.Generic;
using JetBrains.Annotations;
using OriginalCoder.Common.Threading.Locking.Locks;

namespace OriginalCoder.Common.Threading.Containers
{
    [PublicAPI]
    public interface IOcThreadSafeReadOnlyDictionary<TK, TV> : IReadOnlyDictionary<TK, TV>
    {
        /// <summary>
        /// Returns an <see cref="IDisposable"/> that represents a read lock for use with a C# using statement.
        /// WARNING: You *MUST* call the <see cref="IOcLockReadWrite.Lock"/> to activate the lock as the first thing within the using block.
        /// </summary>
        [NotNull] IOcLockReadWrite ReadLock();
    }
}