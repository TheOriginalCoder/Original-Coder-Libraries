//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2011, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

namespace OriginalCoder.Common.Threading.Locking.Base
{
    public abstract class OcThreadLockUpgradableBase : OcThreadLockReadWriteBase
    {
        protected OcThreadLockUpgradableBase(string lockName)
            : base(lockName)
        { }

        internal abstract bool ReadUpgradableLock();
        internal abstract void ReadUpgradableUnlock();
        internal abstract bool WriteUpgradedLock();
        internal abstract void WriteUpgradedUnlock();
    }
}