//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2011, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System.Diagnostics;
using OriginalCoder.Common.Threading.Locking.Base;

namespace OriginalCoder.Common.Threading.Locking.Locks
{
    /// <summary>
    /// For holding and releasing a Read lock that can be upgraded to a write lock AND also allow a write lock to be directly obtained.
    /// </summary>
    internal sealed class ReadUpgradableCanWriteLock : IOcLockReadUpgrade
    {
        internal ReadUpgradableCanWriteLock(OcThreadLockUpgradableBase tl)
        {
            Debug.Assert(tl != null);
            _lock = tl;
        }

      #region Lock Properties (these are statically assigned to reduce overhead when locks are obtained and released)

        public bool CanUpgrade => true;
        public bool CanWriteLock => true;
        public bool IsWriteLock => false;

      #endregion

        private OcThreadLockUpgradableBase _lock;
        private bool _locked;

        /// <inheritdoc />
        public bool Lock()
        {
            if (_locked)
                return true;

            if (_lock.HasAnyLock == true)
                return true;  // Don't need lock because a different lock is already held for this thread

            _locked = _lock.ReadUpgradableLock();
            return _locked;
        }

        public void Dispose()
        {
            if (_locked)
            {
                _lock?.ReadUpgradableUnlock();
                _locked = false;
            }
            _lock = null;
        }
    }
}