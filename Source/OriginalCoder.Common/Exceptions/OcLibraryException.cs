//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2013, 2019 by James B. Higgins
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
    /// Base class for Exceptions related to the Original Coder libraries.
    /// </summary>
    [PublicAPI]
    public class OcLibraryException : OcException
    {
      #region Constructors 

        public OcLibraryException(string message, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Library Error" : message, callerName, callerFile, callerLine)
        { }

        public OcLibraryException(string message, [CanBeNull] Exception exception, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Library Error" : message, exception, callerName, callerFile, callerLine)
        { }

        public OcLibraryException(string message, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, properties, callerName, callerFile, callerLine)
        { }

        public OcLibraryException(string message, [CanBeNull] Exception exception, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null,
            [CallerLineNumber] int callerLine = 0)
            : base(message, exception, properties, callerName, callerFile, callerLine)
        { }

      #endregion
 
      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Library Error");

      #endregion
   }
}