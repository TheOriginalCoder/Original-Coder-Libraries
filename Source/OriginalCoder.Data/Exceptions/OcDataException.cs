//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2009, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OriginalCoder.Common.Exceptions;

// ReSharper disable ExplicitCallerInfoArgument
namespace OriginalCoder.Data.Exceptions
{
    /// <summary>
    /// Original Coder exception that is related to data or data processing.
    /// </summary>
    [PublicAPI]
    public class OcDataException : OcLibraryException
    {
      #region Constructors 

        public OcDataException(string message, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Data Error" : message, callerName, callerFile, callerLine)
        { }

        public OcDataException(string message, [CanBeNull] Exception exception, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Data Error" : message, exception, callerName, callerFile, callerLine)
        { }

        public OcDataException(string message, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, properties, callerName, callerFile, callerLine)
        { }

        public OcDataException(string message, [CanBeNull] Exception exception, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
                : base(message, exception, properties, callerName, callerFile, callerLine)
        { }

      #endregion

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Data Error");

      #endregion
    }
}