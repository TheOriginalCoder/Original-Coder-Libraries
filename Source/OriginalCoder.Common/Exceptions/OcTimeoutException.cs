//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2014, 2019 by James B. Higgins
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
    /// Application exception used when a timeout has been exceeded.
    /// </summary>
    [PublicAPI]
    public class OcTimeoutException : OcApplicationException
    {
      #region Constructors 

        public OcTimeoutException(string message, TimeSpan? elapsedTime = null, object resource = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Timeout" : message, callerName, callerFile, callerLine)
        {
            Resource = resource;
            if (Resource != null)
                PropertySet(nameof(Resource), Resource);
            ElapsedTime = elapsedTime;
            if (ElapsedTime.HasValue)
                PropertySet(nameof(ElapsedTime), ElapsedTime.Value);
        }

        public OcTimeoutException(string message, Exception exception, TimeSpan? elapsedTime = null, object resource = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Timeout" : message, exception, callerName, callerFile, callerLine)
        {
            Resource = resource;
            if (Resource != null)
                PropertySet(nameof(Resource), Resource);
            ElapsedTime = elapsedTime;
            if (ElapsedTime.HasValue)
                PropertySet(nameof(ElapsedTime), ElapsedTime.Value);
        }

        public OcTimeoutException(string message, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, properties, callerName, callerFile, callerLine)
        { }

        public OcTimeoutException(string message, [CanBeNull] Exception exception, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
                : base(message, exception, properties, callerName, callerFile, callerLine)
        { }

      #endregion

        public object Resource { get; }
        public TimeSpan? ElapsedTime { get; }

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Timeout");

        protected override void SummaryBuildProperties()
        {
            base.SummaryBuildProperties();
            SummaryAddProperty("ElapsedTime");
            SummaryAddProperty("Resource");
        }

      #endregion
    }
}