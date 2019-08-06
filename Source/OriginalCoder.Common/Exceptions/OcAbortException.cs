//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2010, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

// ReSharper disable ExplicitCallerInfoArgument
namespace OriginalCoder.Common.Exceptions
{
    /// <summary>
    /// Application exception used to indicate that an abort has occurred.  
    /// </summary>
    [PublicAPI]
    public class OcAbortException : OcApplicationException
    {
      #region Constructors 

        public OcAbortException(string message, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Abort" : message, callerName, callerFile, callerLine)
        { }

        public OcAbortException(Exception exception, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base("Unspecified Abort", exception, callerName, callerFile, callerLine)
        { }

        public OcAbortException(string message, Exception exception, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Abort" : message, exception, callerName, callerFile, callerLine)
        { }

        public OcAbortException(string message, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, properties, callerName, callerFile, callerLine)
        { }

        public OcAbortException(string message, [CanBeNull] Exception exception, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
                : base(message, exception, properties, callerName, callerFile, callerLine)
        { }

      #endregion

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Abort");

      #endregion
    }
}