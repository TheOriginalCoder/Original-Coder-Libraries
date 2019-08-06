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
    /// For holding and releasing a READ lock that does not allow write locks (either by upgrading or directly obtaining one).
    /// </summary>
    internal sealed class ReadLock : IOcLockReadUpgrade
    {
        internal ReadLock(OcThreadLockReadWriteBase tl)
        {
            Debug.Assert(tl != null);
            _lock = tl;
        }

      #region Lock Properties (these are statically assigned to reduce overhead when locks are obtained and released)

        public bool CanUpgrade => false;
        public bool CanWriteLock => false;
        public bool IsWriteLock => false;

      #endregion

        private OcThreadLockReadWriteBase _lock;
        private bool _locked;

        /// <inheritdoc />
        public bool Lock()
        {
            if (_locked)
                return true;  // Lock previously obtained

            if (_lock.HasAnyLock == true)
                return true;  // Don't need lock because a different lock is already held for this thread

            _locked = _lock.ReadLock();
            return _locked;
        }

        public void Dispose()
        {
            if (_locked)
            {
                _lock?.ReadUnlock();
                _locked = false;
            }
            _lock = null;
        }
    }
}