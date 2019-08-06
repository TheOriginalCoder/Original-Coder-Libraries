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
using OriginalCoder.Data.Exceptions;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Requests;

// ReSharper disable ExplicitCallerInfoArgument
namespace OriginalCoder.Layers.Exceptions
{
    [PublicAPI]
    public class OcLayerException : OcDataEntityException
    {
      #region Constructors 

        public OcLayerException(string message, IOcLayer layer, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, layer?.Properties, entity, entityKey, callerName, callerFile, callerLine)
        { }

        public OcLayerException(string message, [CanBeNull] Exception exception, IOcLayer layer, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, exception, layer?.Properties, entity, entityKey, callerName, callerFile, callerLine)
        { }

        public OcLayerException(string message, [CanBeNull] Exception exception, string resourceName, OcLayerType layerType, string layerTypeName, IOcRequest request = null, [CanBeNull] Type entityType = null, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Layer Routing Error" : message, exception, entityType, entity, entityKey, callerName, callerFile, callerLine)
        {
            ResourceName = resourceName;
            if (ResourceName != null)
                PropertySet(nameof(ResourceName), ResourceName);

            LayerType = layerType;
            if (LayerType != OcLayerType.Unknown)
                PropertySet(nameof(LayerType), LayerType);

            LayerTypeName = layerTypeName;
            if (LayerTypeName != null)
                PropertySet(nameof(LayerTypeName), LayerTypeName);

            Request = request;
            if (Request != null)
                PropertySet(nameof(Request), Request);
        }

      #endregion

        public string ResourceName { get; private set; }
        public OcLayerType LayerType { get; private set; }
        public string LayerTypeName { get; private set; }

        public IOcRequest Request { get; private set; }

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Layer Error");

        /// <inheritdoc />
        protected override void SummaryBuildProperties()
        {
            SummaryAddProperty("Request");
            base.SummaryBuildProperties();
            SummaryAddProperty("LayerTypeName");
            SummaryAddProperty("LayerType");
            SummaryAddProperty("ResourceName");
        }

      #endregion
    }
}