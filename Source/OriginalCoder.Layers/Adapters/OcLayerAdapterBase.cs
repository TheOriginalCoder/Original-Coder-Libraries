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
using OriginalCoder.Common.Api;
using OriginalCoder.Data.Mapper;
using OriginalCoder.Layers.Base;
using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Exceptions;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Adapters
{
    /// <summary>
    /// Abstract base class for converting resources to different data types.
    /// </summary>
    [PublicAPI]
    public abstract class OcLayerAdapterBase<TRequest, TEntity, TKey, TNextLayer> : OcLayerBase<TRequest, TEntity, TKey, TNextLayer>
        where TRequest : class, IOcRequest
        where TEntity : class
        where TNextLayer : class, IOcLayer<TRequest>
    {
      #region Constructors

        /// <summary>
        /// Preferred constructor when <paramref name="layerType"/> is not <see cref="OcLayerType.Other"/>
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerType"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        /// <param name="nextLayer"></param>
        protected OcLayerAdapterBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, TNextLayer nextLayer)
            : base(configuration, layerType, resourceName, request, nextLayer)
        {
            DataMapper = configuration.DataMapper;
            if (DataMapper == null)
                throw NewConstructorException($"{nameof(DataMapper)} not available from {nameof(configuration)}", null, resourceName, layerType, null, request);
        }

        /// <summary>
        /// Preferred constructor for use with <see cref="OcLayerType.Other"/> and specifying a <paramref name="layerTypeName"/>.
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerTypeName"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        /// <param name="nextLayer"></param>
        protected OcLayerAdapterBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, TNextLayer nextLayer)
            : base(configuration, layerTypeName, resourceName, request, nextLayer)
        {
            DataMapper = configuration.DataMapper;
            if (DataMapper == null)
                throw NewConstructorException($"{nameof(DataMapper)} not available from {nameof(configuration)}", null, resourceName, OcLayerType.Other, layerTypeName, request);
        }

        /// <summary>
        /// Preferred constructor when <paramref name="layerType"/> is not <see cref="OcLayerType.Other"/>
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerType"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        /// <param name="nextLayerType"></param>
        protected OcLayerAdapterBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType)
            : base(configuration, layerType, resourceName, request, nextLayerType)
        {
            DataMapper = configuration.DataMapper;
            if (DataMapper == null)
                throw NewConstructorException($"{nameof(DataMapper)} not available from {nameof(configuration)}", null, resourceName, layerType, null, request);
        }

        /// <summary>
        /// Preferred constructor for use with <see cref="OcLayerType.Other"/> and specifying a <paramref name="layerTypeName"/>.
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerTypeName"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        /// <param name="nextLayerType"></param>
        /// <param name="nextLayerTypeName"></param>
        protected OcLayerAdapterBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType, string nextLayerTypeName)
            : base(configuration, layerTypeName, resourceName, request, nextLayerType, nextLayerTypeName)
        {
            DataMapper = configuration.DataMapper;
            if (DataMapper == null)
                throw NewConstructorException($"{nameof(DataMapper)} not available from {nameof(configuration)}", null, resourceName, OcLayerType.Other, layerTypeName, request);
        }

      #endregion

        /// <summary>
        /// Interface that provides data conversion services
        /// </summary>
        protected IOcDataMapper DataMapper { get; }

      #region Throw Exceptions

        protected OcLayerException NewException(string message, Type typeIn, Type typeOut, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            var ex = new OcAdapterLayerException(message, this, typeIn, typeOut, callerName, callerFile, callerLine);
            // ReSharper restore ExplicitCallerInfoArgument
            Request.ResponseMessages.Add(OcApiMessageType.Error, $"New Exception: {ex.Summary}");
            return ex;
        }

        protected OcLayerException NewException(string message, [CanBeNull] Exception exception, Type typeIn, Type typeOut, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            var ex = new OcLayerException(message, exception, this, typeIn, typeOut, callerName, callerFile, callerLine);
            // ReSharper restore ExplicitCallerInfoArgument
            Request.ResponseMessages.Add(OcApiMessageType.Error, $"New Exception: {ex.Summary}");
            return ex;
        }

      #endregion
    }
}