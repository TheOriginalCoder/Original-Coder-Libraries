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
using System.Threading;
using OriginalCoder.Common.Interfaces.Properties;
using OriginalCoder.Common.Threading.Locking.Base;
using OriginalCoder.Common.Threading.Locking.Locks;

namespace OriginalCoder.Common.Threading.Locking.Concrete
{
    /// <summary>
    /// Implements <see cref="IOcThreadLock"/> using <see cref="System.Threading.ReaderWriterLock"/>.
    /// Supports Read, Read Upgradable and Write locks.  Any number of threads can read concurrently, write locks are exclusive.
    /// </summary>
    public sealed class OcThreadLockReadWrite : OcThreadLockUpgradableCookieBase, IOcThreadLockReadUpgrade, IStatusSummary
    {
        public OcThreadLockReadWrite(string lockName)
            : base(lockName)
        { }

        /// <inheritdoc />
        public string StatusSummary => $"{GetType().Name} Name [{Name}] HasRead [{_lock.IsReaderLockHeld}] HasWrite [{_lock.IsWriterLockHeld}] WriterSeqNum [{_lock.WriterSeqNum}]";

        private readonly ReaderWriterLock _lock = new ReaderWriterLock();

      #region IThreadLock

        public override bool? HasWriteLock
            => _lock.IsWriterLockHeld;

        IOcLockReadWrite IOcThreadLock.WriteLock()
        {
            return new WriteLock(this);
        }

      #endregion

      #region IThreadLockReadWrite 

        public override bool? HasReadLock
            => _lock.IsReaderLockHeld;

        public override bool? HasAnyLock
            => _lock.IsReaderLockHeld || _lock.IsWriterLockHeld;

        IOcLockReadWrite IOcThreadLockReadWrite.ReadLock()
        {
            return new ReadLock(this);
        }

      #endregion

      #region IThreadLockReadUpgrade

        IOcLockReadUpgrade IOcThreadLockReadUpgrade.ReadLock()
        {
            return new ReadLock(this);
        }

        IOcLockReadUpgrade IOcThreadLockReadUpgrade.ReadUpgradableLock()
        {
            return new ReadUpgradableLock(this);
        }

        IOcLockReadUpgrade IOcThreadLockReadUpgrade.UpgradeLock()
        {
            return new UpgradedWriteLockCookie(this);
        }

        IOcLockReadUpgrade IOcThreadLockReadUpgrade.WriteLock()
        {
            return new WriteLock(this);
        }

      #endregion

      #region Obtaining Locks

        internal override bool ReadLock()
        {
            _lock.AcquireReaderLock(int.MaxValue);
            return true;
        }

        internal override bool ReadUpgradableLock()
        {
            _lock.AcquireReaderLock(int.MaxValue);
            return true;
        }

        internal override bool WriteUpgradedLock()
        {
            // This method should never get called by this implementation
            throw new NotImplementedException();
        }

        internal override bool WriteUpgradedLock(out LockCookie cookie)
        {
            cookie = _lock.UpgradeToWriterLock(int.MaxValue);
            return true;
        }

        internal override bool WriteLock()
        {
            _lock.AcquireWriterLock(int.MaxValue);
            return true;
        }

      #endregion

      #region Releasing Locks

        internal override void ReadUnlock()
        {
            _lock.ReleaseReaderLock();
        }

        internal override void WriteUnlock()
        {
            _lock.ReleaseWriterLock();
        }

        internal override void ReadUpgradableUnlock()
        {
            if (_lock.IsReaderLockHeld)
                _lock.ReleaseReaderLock();
        }

        internal override void WriteUpgradedUnlock()
        {
            // This method should never get called by this implementation
            throw new NotImplementedException();
        }

        internal override void WriteUpgradedUnlock(ref LockCookie cookie)
        {
            _lock.DowngradeFromWriterLock(ref cookie);
        }

      #endregion
    }
}