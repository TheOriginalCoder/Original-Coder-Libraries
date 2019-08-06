//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OriginalCoder.Common.Extensions;

// ReSharper disable ExplicitCallerInfoArgument
namespace OriginalCoder.Common.Exceptions
{
    /// <summary>
    /// Exception for errors related to a user interface.
    /// </summary>
    [PublicAPI]
    public class OcUserInterfaceException : OcException
    {
      #region Constructors 

        public OcUserInterfaceException(string message, object uiControl = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified User Interface Error" : message, callerName, callerFile, callerLine)
        {
            UiControl = uiControl;
            if (UiControl != null)
                PropertySet(nameof(UiControl), UiControl);
        }

        public OcUserInterfaceException(string message, Exception exception, object uiControl = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified User Interface Error" : message, exception, callerName, callerFile, callerLine)
        {
            UiControl = uiControl;
            if (UiControl != null)
                PropertySet(nameof(UiControl), UiControl);
        }

        public OcUserInterfaceException(string message, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, properties, callerName, callerFile, callerLine)
        { }

        public OcUserInterfaceException(string message, [CanBeNull] Exception exception, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
                : base(message, exception, properties, callerName, callerFile, callerLine)
        { }

      #endregion

        public object UiControl { get; }

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("User Interface Error");

        protected override void SummaryBuildProperties()
        {
            base.SummaryBuildProperties();
            SummaryAddProperty("UiControl");
            SummaryAddProperty("UiControlType", UiControl?.GetType().FriendlyName());
        }

      #endregion
    }
}