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
using JetBrains.Annotations;
using OriginalCoder.Common.Interfaces;
using OriginalCoder.Common.Interfaces.Properties;

namespace OriginalCoder.Data.Interfaces
{
    /// <summary>
    /// Interface for representing a collection of operations being performed that can
    /// be committed (saved) or rolled back (cancelled).  Disposing of this object will roll back
    /// (cancel) any operations that were performed since <see cref="Commit"/> was called.
    /// Exact details about how the unit of work behaves will depend on the underlying
    /// implementation it encapsulates.
    /// </summary>
    [PublicAPI]
    public interface IOcUnitOfWork : IDisposable, IName, IStatusSummary
    {
        /// <summary>
        /// Unique integer ID for the object instance (intended for use in debugging)
        /// </summary>
        int InstanceId { get; }

        /// <summary>
        /// Specifies when this unit of work instances was created.  Mostly for debugging.
        /// </summary>
        DateTime WhenCreated { get; }

        /// <summary>
        /// Indicates the current state of this unit of work.  
        /// </summary>
        OcUnitOfWorkState State { get; }

        /// <summary>
        /// Optionally provides text logging capabilities to record operations performed
        /// </summary>
        IOcTextLog TextLog { get; set; }

        /// <summary>
        /// Commit and save all pending operations and changes.
        /// If an error occurs an exception will be thrown.
        /// If the commit is successful Dispose will be called for the unit of work.
        /// </summary>
        void Commit();

        /// <summary>
        /// Cancel and undo all pending operations and changes.
        /// If an error occurs an exception will be thrown.
        /// If the rollback is successful Dispose will be called for the unit of work.
        /// </summary>
        void Rollback();
    }
}