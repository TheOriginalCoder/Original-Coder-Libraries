//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2012, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Threading;
using OriginalCoder.Common;
using OriginalCoder.Common.Interfaces;

namespace OriginalCoder.Data.Interfaces
{
    public abstract class OcUnitOfWorkBase : OcDisposableBase, IOcUnitOfWork
    {
      #region Constructors

        private static int _identity;

        protected OcUnitOfWorkBase()
            : this(null, null)
        { }

        protected OcUnitOfWorkBase(string name)
            : this(name, null)
        { }

        protected OcUnitOfWorkBase(IOcTextLog log)
            : this(null, log)
        { }

        protected OcUnitOfWorkBase(string name, IOcTextLog log)
        {
            State = OcUnitOfWorkState.InProgress;
            WhenCreated = DateTime.UtcNow;
            InstanceId = Interlocked.Increment(ref _identity);
            Name = $"UoW#{InstanceId}{(string.IsNullOrWhiteSpace(name) ? "" : ":" + name)}";
            TextLog = log;
        }

      #endregion

        /// <inheritdoc />
        public string StatusSummary => $"UnitOfWork {Name} in State {State}";

      #region IUnitOfWork

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public int InstanceId { get; }

        /// <inheritdoc />
        public DateTime WhenCreated { get; }

        /// <inheritdoc />
        public OcUnitOfWorkState State { get; private set; }

        /// <inheritdoc />
        public IOcTextLog TextLog { get; set; }

        /// <inheritdoc />
        public abstract void Commit();

        /// <inheritdoc />
        public abstract void Rollback();

        protected void SetSaved()
        {
            State = OcUnitOfWorkState.Saved;
        }

        protected void SetCancelled()
        {
            State = OcUnitOfWorkState.Cancelled;
        }

        protected void SetError()
        {
            State = OcUnitOfWorkState.Error;
        }

      #endregion

        protected override void DisposeManaged()
        {
            if (State == OcUnitOfWorkState.InProgress)
                Rollback();
            base.DisposeManaged();
        }
    }
}