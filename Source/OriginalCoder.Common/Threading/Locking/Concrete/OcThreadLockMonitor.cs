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
using JetBrains.Annotations;
using OriginalCoder.Common.Interfaces.Properties;
using OriginalCoder.Common.Threading.Locking.Base;
using OriginalCoder.Common.Threading.Locking.Locks;

namespace OriginalCoder.Common.Threading.Locking.Concrete
{
    /// <summary>
    /// Implements <see cref="IOcThreadLock"/> using <see cref="Monitor"/>.
    /// </summary>
    [PublicAPI]
    public sealed class OcThreadLockMonitor : OcThreadLockBase, IOcThreadLock, IStatusSummary
    {
        public OcThreadLockMonitor(string lockName)
            : base(lockName)
        { }

        /// <inheritdoc />
        public string StatusSummary => $"{GetType().Name} Name [{Name}] HasLock [{Monitor.IsEntered(this)}]";

        public override bool? HasWriteLock => null;

        /// <inheritdoc />
        IOcLockReadWrite IOcThreadLock.WriteLock()
        {
            var result = new WriteLock(this);
            return result;
        }

        internal override bool WriteLock()
        {
            Monitor.Enter(this);
            return true;
        }

        internal override void WriteUnlock()
        {
            Monitor.Exit(this);
        }
    }
}