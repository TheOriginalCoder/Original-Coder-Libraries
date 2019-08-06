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

// ReSharper disable ExplicitCallerInfoArgument
namespace OriginalCoder.Common.Exceptions
{
    /// <summary>
    /// Exception for network related errors.
    /// </summary>
    [PublicAPI]
    public class OcNetworkException : OcException
    {
      #region Constructors 

        public OcNetworkException(string message, string localHost = null, string remoteResource = null, TimeSpan? elapsedTime = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Network Error" : message, callerName, callerFile, callerLine)
        {
            LocalHost = string.IsNullOrWhiteSpace(localHost) ? null : localHost.Trim();
            if (LocalHost != null)
                PropertySet(nameof(LocalHost), LocalHost);

            RemoteResource = string.IsNullOrWhiteSpace(remoteResource) ? null : remoteResource.Trim(); 
            if (RemoteResource != null)
                PropertySet(nameof(RemoteResource), RemoteResource);

            ElapsedTime = elapsedTime;
            if (ElapsedTime.HasValue)
                PropertySet(nameof(ElapsedTime), ElapsedTime.Value);
        }

        public OcNetworkException(string message, Exception exception, string localHost = null, string remoteResource = null, TimeSpan? elapsedTime = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Network Error" : message, exception, callerName, callerFile, callerLine)
        {
            LocalHost = string.IsNullOrWhiteSpace(localHost) ? null : localHost.Trim();
            if (LocalHost != null)
                PropertySet(nameof(LocalHost), LocalHost);

            RemoteResource = string.IsNullOrWhiteSpace(remoteResource) ? null : remoteResource.Trim(); 
            if (RemoteResource != null)
                PropertySet(nameof(RemoteResource), RemoteResource);

            ElapsedTime = elapsedTime;
            if (ElapsedTime.HasValue)
                PropertySet(nameof(ElapsedTime), ElapsedTime.Value);
        }

        public OcNetworkException(string message, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, properties, callerName, callerFile, callerLine)
        { }

        public OcNetworkException(string message, [CanBeNull] Exception exception, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
                : base(message, exception, properties, callerName, callerFile, callerLine)
        { }

      #endregion

        public string LocalHost { get; }
        public string RemoteResource { get; }
        public TimeSpan? ElapsedTime { get; }

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Network Error");

        protected override void SummaryBuildProperties()
        {
            base.SummaryBuildProperties();
            SummaryAddProperty("ElapsedTime");
            SummaryAddProperty("RemoteResource");
            SummaryAddProperty("LocalHost");
        }

      #endregion
    }
}