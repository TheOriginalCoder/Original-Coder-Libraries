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
    /// For holding and releasing a Write lock.
    /// </summary>
    internal sealed class WriteLock : IOcLockReadUpgrade
    {
        internal WriteLock(OcThreadLockBase tl)
        {
            Debug.Assert(tl != null);
            _lock = tl;
        }

      #region Lock Properties (these are statically assigned to reduce overhead when locks are obtained and released)

        public bool CanUpgrade => false;
        public bool CanWriteLock => false;
        public bool IsWriteLock => true;

      #endregion

        private OcThreadLockBase _lock;
        private bool _locked;

        /// <inheritdoc />
        public bool Lock()
        {
            if (_locked)
                return true;  // Lock previously obtained

            if (_lock.HasWriteLock == true)
                return true;  // Don't need lock because a different write lock is already held for this thread

            _locked = _lock.WriteLock();
            return _locked;
        }


        public void Dispose()
        {
            if (_locked)
            {
                _lock?.WriteUnlock();
                _locked = false;
            }
            _lock = null;
        }
    }
}