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
    /// Original Coder exception related to Thread Locking.
    /// </summary>
    [PublicAPI]
    public sealed class OcThreadLockException : OcLibraryException
    {
      #region Constructors 

        public OcThreadLockException(string message, object lockObject = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Thread Locking Error" : message, callerName, callerFile, callerLine)
        {
            Lock = lockObject;
            if (Lock != null)
                PropertySet(nameof(Lock), Lock);
        }

        public OcThreadLockException(string message, Exception exception, object lockObject = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Thread Locking Error" : message, exception, callerName, callerFile, callerLine)
        {
            Lock = lockObject;
            if (Lock != null)
                PropertySet(nameof(Lock), Lock);
        }

        public OcThreadLockException(string message, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, properties, callerName, callerFile, callerLine)
        { }

        public OcThreadLockException(string message, [CanBeNull] Exception exception, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
                : base(message, exception, properties, callerName, callerFile, callerLine)
        { }

      #endregion

        public object Lock { get; }

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Thread Locking Error");

        protected override void SummaryBuildProperties()
        {
            base.SummaryBuildProperties();
            SummaryAddProperty("Lock");
        }

      #endregion
    }
}