//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System.Threading;

namespace OriginalCoder.Common.Threading.Locking.Base
{
    public abstract class OcThreadLockUpgradableCookieBase : OcThreadLockUpgradableBase
    {
        protected OcThreadLockUpgradableCookieBase(string lockName)
            : base(lockName)
        { }

        internal abstract bool WriteUpgradedLock(out LockCookie cookie);
        internal abstract void WriteUpgradedUnlock(ref LockCookie cookie);
    }
}