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
using OriginalCoder.Common.Extensions;

namespace OriginalCoder.Common.Threading.Locking.Base
{
    public abstract class OcThreadLockBase : IDisposable
    {
        protected OcThreadLockBase(string lockName)
        {
            Name = string.IsNullOrWhiteSpace(lockName) ? $"Un-Named {GetType().FriendlyName()}" : lockName;
        }

        public string Name { get; }

        public abstract bool? HasWriteLock { get; }

        internal abstract bool WriteLock();
        internal abstract void WriteUnlock();

        public virtual void Dispose()
        { }
    }
}