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
using OriginalCoder.Layers.Exceptions;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Interfaces.Data;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Config.Factories
{
    internal class OcLayerRestrictedCrudFactory<TRequest> : IOcLayerRestrictedCrudFactory<TRequest>
        where TRequest : class, IOcRequest
    {
      #region Constructors

        internal OcLayerRestrictedCrudFactory(OcLayerType layerType, OcLayerConfiguration<TRequest> serviceLocator)
        {
            if (layerType == OcLayerType.Unknown || layerType == OcLayerType.Other)
                throw new ArgumentOutOfRangeException(nameof(layerType));
            if (serviceLocator == null)
                throw new ArgumentNullException(nameof(serviceLocator));

            LayerType = layerType;
            LayerTypeName = LayerType.ToString();
            _serviceLocator = serviceLocator;
        }

        internal OcLayerRestrictedCrudFactory(string layerTypeName, OcLayerConfiguration<TRequest> serviceLocator)
        {
            if (string.IsNullOrWhiteSpace(layerTypeName))
                throw new ArgumentNullException(nameof(layerTypeName));
            if (string.Equals(layerTypeName, OcLayerType.Unknown.ToString(), StringComparison.OrdinalIgnoreCase) || string.Equals(layerTypeName, OcLayerType.Other.ToString(), StringComparison.OrdinalIgnoreCase))
                throw new ArgumentOutOfRangeException(nameof(layerTypeName));
            if (serviceLocator == null)
                throw new ArgumentNullException(nameof(serviceLocator));

            LayerType = OcLayerType.Other;
            LayerTypeName = layerTypeName.Trim();
            _serviceLocator = serviceLocator;
        }

      #endregion

        private readonly OcLayerConfiguration<TRequest> _serviceLocator;

        /// <inheritdoc />
        public OcLayerType LayerType { get; }

        /// <inheritdoc />
        public string LayerTypeName { get; }

      #region IOcLayerResourceFactory<TRequest>

        /// <inheritdoc />
        public TInterface GetResource<TInterface>() where TInterface : class
        {
            return _serviceLocator.GetResource<TInterface>(LayerTypeName);
        }

        /// <inheritdoc />
        public TInterface GetResource<TInterface>([NotNull] TRequest request) where TInterface : class
        {
            return _serviceLocator.GetResource<TInterface>(LayerTypeName, request);
        }

        /// <inheritdoc />
        public IOcLayer<TRequest> GetResourceLayer([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request)
        {
            if (!_serviceLocator.IsAllowed(LayerType, layerType))
                throw NewRoutingException(resourceName, layerType, null, $"Failed to get Resource [{resourceName}] because Layer [{LayerTypeName}] is not allowed to access Layer [{layerType}]");
            return _serviceLocator.GetResourceLayer(resourceName, layerType, request);
        }

        /// <inheritdoc />
        public TLayer GetResourceLayer<TLayer>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TLayer : class, IOcLayer<TRequest>
        {
            if (!_serviceLocator.IsAllowed(LayerType, layerType))
                throw NewRoutingException(resourceName, layerType, null, $"Failed to get Resource [{resourceName}] because Layer [{LayerTypeName}] is not allowed to access Layer [{layerType}]");
            return _serviceLocator.GetResourceLayer<TLayer>(resourceName, layerType, request);
        }

        /// <inheritdoc />
        public IOcLayer<TRequest> GetResourceLayer([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request)
        {
            if (!_serviceLocator.IsAllowed(LayerType, layerTypeName))
                throw NewRoutingException(resourceName, OcLayerType.Other, layerTypeName, $"Failed to get Resource [{resourceName}] because Layer [{LayerTypeName}] is not allowed to access Layer [{layerTypeName}]");
            return _serviceLocator.GetResourceLayer(resourceName, layerTypeName, request);
        }

        /// <inheritdoc />
        public TLayer GetResourceLayer<TLayer>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TLayer : class, IOcLayer<TRequest>
        {
            if (!_serviceLocator.IsAllowed(LayerType, layerTypeName))
                throw NewRoutingException(resourceName, OcLayerType.Other, layerTypeName, $"Failed to get Resource [{resourceName}] because Layer [{LayerTypeName}] is not allowed to access Layer [{layerTypeName}]");
            return _serviceLocator.GetResourceLayer<TLayer>(resourceName, layerTypeName, request);
        }

      #endregion

        protected OcLayerRoutingException NewRoutingException(string resourceName, OcLayerType nextLayerType, string nextLayerTypeName, string message, [CanBeNull] Exception exception = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            return new OcLayerRoutingException(message, exception, resourceName, LayerType, LayerTypeName, nextLayerType, nextLayerTypeName, callerName, callerFile, callerLine);
            // ReSharper restore ExplicitCallerInfoArgument
        }

      #region IOcLayerResourceCrudFactory<TRequest>

        /// <inheritdoc />
        public IOcLayerRead<TRequest, TEntity, TKey> GetLayerRead<TEntity, TKey>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TEntity : class
            => GetResourceLayer(resourceName, layerType, request) as IOcLayerRead<TRequest, TEntity, TKey>;

        /// <inheritdoc />
        public IOcLayerRead<TRequest, TEntity, TKey> GetLayerRead<TEntity, TKey>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class
            => GetResourceLayer(resourceName, layerTypeName, request) as IOcLayerRead<TRequest, TEntity, TKey>;

        /// <inheritdoc />
        public IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch> GetLayerReadSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TEntity : class where TSearch : class
            => GetResourceLayer(resourceName, layerType, request) as IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch>;

        /// <inheritdoc />
        public IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch> GetLayerReadSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class where TSearch : class
            => GetResourceLayer(resourceName, layerTypeName, request) as IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch>;

        /// <inheritdoc />
        public IOcLayerCrud<TRequest, TEntity, TKey> GetLayerCrud<TEntity, TKey>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TEntity : class
            => GetResourceLayer(resourceName, layerType, request) as IOcLayerCrud<TRequest, TEntity, TKey>;

        /// <inheritdoc />
        public IOcLayerCrud<TRequest, TEntity, TKey> GetLayerCrud<TEntity, TKey>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class
            => GetResourceLayer(resourceName, layerTypeName, request) as IOcLayerCrud<TRequest, TEntity, TKey>;

        /// <inheritdoc />
        public IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch> GetLayerCrudSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TEntity : class where TSearch : class
            => GetResourceLayer(resourceName, layerType, request) as IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch>;

        /// <inheritdoc />
        public IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch> GetLayerCrudSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class where TSearch : class
            => GetResourceLayer(resourceName, layerTypeName, request) as IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch>;

      #endregion
    }
}