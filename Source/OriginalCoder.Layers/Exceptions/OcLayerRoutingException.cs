//=============================================================================
// Original Coder - Layer Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OriginalCoder.Layers.Interfaces;

namespace OriginalCoder.Layers.Exceptions
{
    [PublicAPI]
    public class OcLayerRoutingException : OcLayerException
    {
      #region Constructors 

        public OcLayerRoutingException(string message, IOcLayer layer, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, layer, entity, entityKey, callerName, callerFile, callerLine)
        { }

        public OcLayerRoutingException(string message, [CanBeNull] Exception exception, IOcLayer layer, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, exception, layer, entity, entityKey, callerName, callerFile, callerLine)
        { }

        public OcLayerRoutingException(string message, [CanBeNull] Exception exception, string resourceName, OcLayerType layerType, string layerTypeName, OcLayerType nextLayerType, string nextLayerTypeName, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, exception, resourceName, layerType, layerTypeName, null, null, null, null, callerName, callerFile, callerLine)
        {
            NextLayerType = nextLayerType;
            if (NextLayerType != OcLayerType.Unknown)
                PropertySet(nameof(NextLayerType), NextLayerType);

            NextLayerTypeName = nextLayerTypeName;
            if (NextLayerTypeName != null)
                PropertySet(nameof(NextLayerTypeName), NextLayerTypeName);
        }

      #endregion

        public OcLayerType NextLayerType { get; private set; }
        public string NextLayerTypeName { get; private set; }

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Layer Routing Error");

        protected override void SummaryBuildProperties()
        {
            base.SummaryBuildProperties();
            SummaryAddProperty("NextLayerType");
            SummaryAddProperty("NextLayerTypeName");
        }

      #endregion
    }
}