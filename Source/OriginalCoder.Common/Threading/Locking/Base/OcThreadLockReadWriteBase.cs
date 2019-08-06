//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2011, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;

namespace OriginalCoder.Common.Threading.Locking.Base
{
    [PublicAPI]
    public abstract class OcThreadLockReadWriteBase : OcThreadLockBase
    {
        protected OcThreadLockReadWriteBase(string lockName)
            : base(lockName)
        { }

        public abstract bool? HasReadLock { get; }
        public abstract bool? HasAnyLock {  get;}

        internal abstract bool ReadLock();
        internal abstract void ReadUnlock();
    }
}