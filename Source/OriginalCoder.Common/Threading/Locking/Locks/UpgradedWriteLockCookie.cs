﻿//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System.Diagnostics;
using System.Threading;
using OriginalCoder.Common.Threading.Locking.Base;

namespace OriginalCoder.Common.Threading.Locking.Locks
{
    /// <summary>
    /// For holding and releasing a Write lock and also saves a <see cref="LockCookie"/> for reverting back to a read lock.
    /// </summary>
    internal sealed class UpgradedWriteLockCookie : IOcLockReadUpgrade
    {
        internal UpgradedWriteLockCookie(OcThreadLockUpgradableCookieBase tl)
        {
            Debug.Assert(tl != null);
            _lock = tl;
        }

      #region Lock Properties (these are statically assigned to reduce overhead when locks are obtained and released)

        public bool CanUpgrade => false;
        public bool CanWriteLock => false;
        public bool IsWriteLock => true;

      #endregion

        private OcThreadLockUpgradableCookieBase _lock;
        private bool _locked;
        private LockCookie _lockCookie;

        /// <inheritdoc />
        public bool Lock()
        {
            if (_locked)
                return true;

            if (_lock.HasWriteLock == true)
                return true;  // Don't need lock because a different write lock is already held for this thread

            if (_lock.WriteUpgradedLock(out _lockCookie))
            {
                _locked = true;
            }
            return _locked;
        }

        public void Dispose()
        {
            if (_locked)
            {
                _lock.WriteUpgradedUnlock(ref _lockCookie);
                _locked = false;
            }
            _lock = null;
        }
    }
}