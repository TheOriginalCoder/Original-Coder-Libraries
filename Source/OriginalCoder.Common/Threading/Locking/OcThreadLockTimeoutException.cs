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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OriginalCoder.Common.Exceptions;

// ReSharper disable ExplicitCallerInfoArgument
namespace OriginalCoder.Common.Threading.Locking
{
    /// <summary>
    /// Exception thrown when a <see cref="IOcThreadLock"/> timeout occurs.
    /// </summary>
    [PublicAPI]
    public sealed class OcThreadLockTimeoutException : OcTimeoutException
    {
      #region Constructors 

        public OcThreadLockTimeoutException([NotNull] IOcThreadLock ocThreadLock, [NotNull] string lockType, [CanBeNull] TimeSpan? elapsedTime = null, [CanBeNull] object resource = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base($"Timeout waiting for {lockType} lock on Thread Lock" + (!string.IsNullOrWhiteSpace(ocThreadLock?.Name) ? $" named [{ocThreadLock.Name}]" : ""), elapsedTime, ocThreadLock, callerName, callerFile, callerLine)
        {
            OcThreadLock = ocThreadLock;
            if (OcThreadLock != null)
                PropertySet(nameof(OcThreadLock), OcThreadLock);
            LockType = lockType;
            if (!string.IsNullOrWhiteSpace(LockType))
                PropertySet(nameof(LockType), LockType);
        }

        public OcThreadLockTimeoutException(string message, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, properties, callerName, callerFile, callerLine)
        { }

        public OcThreadLockTimeoutException(string message, [CanBeNull] Exception exception, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, exception, properties, callerName, callerFile, callerLine)
        { }

        #endregion

        public IOcThreadLock OcThreadLock { get; }
        public string LockType { get; }

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Thread Lock Timeout");

        protected override void SummaryBuildProperties()
        {
            base.SummaryBuildProperties();
            SummaryAddProperty("LockType");
            SummaryAddProperty("ThreadLock");
        }

      #endregion
    }
}