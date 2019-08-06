//=============================================================================
// Original Coder - Layer Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Interfaces.Data;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Config.Factories
{
    /// <summary>
    /// Factory for obtaining typed resources and resource layers with additional methods to more easily obtain layers that support Read, Read + Search, CRUD and CRUD + Search operations.
    /// </summary>
    [PublicAPI]
    public interface IOcLayerCrudFactory<TRequest> : IOcLayerFactory<TRequest>
        where TRequest : class, IOcRequest
    {
      #region Layers that support Read operations

        /// <summary>
        /// If available returns the read-only layer that provides <paramref name="layerType"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerType">Type of layer to search for</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        IOcLayerRead<TRequest, TEntity, TKey> GetLayerRead<TEntity, TKey>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TEntity : class;

        /// <summary>
        /// If available returns the read-only layer that provides <paramref name="layerTypeName"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerTypeName">Name of the layer type to search for (for use with layers that are <see cref="OcLayerType.Other"/>).</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        IOcLayerRead<TRequest, TEntity, TKey> GetLayerRead<TEntity, TKey>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class;

      #endregion

      #region Layers that support Read & Search operations

        /// <summary>
        /// If available returns the read-only layer that provides <paramref name="layerType"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerType">Type of layer to search for</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch> GetLayerReadSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TEntity : class where TSearch : class;

        /// <summary>
        /// If available returns the read-only layer that provides <paramref name="layerTypeName"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerTypeName">Name of the layer type to search for (for use with layers that are <see cref="OcLayerType.Other"/>).</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch> GetLayerReadSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class where TSearch : class;

      #endregion

      #region Layers that support CRUD (Create, Read, Update & Delete) operations

        /// <summary>
        /// If available returns the read-only layer that provides <paramref name="layerType"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerType">Type of layer to search for</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        IOcLayerCrud<TRequest, TEntity, TKey> GetLayerCrud<TEntity, TKey>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TEntity : class;

        /// <summary>
        /// If available returns the read-only layer that provides <paramref name="layerTypeName"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerTypeName">Name of the layer type to search for (for use with layers that are <see cref="OcLayerType.Other"/>).</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        IOcLayerCrud<TRequest, TEntity, TKey> GetLayerCrud<TEntity, TKey>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class;

      #endregion

      #region Layers that support CRUD (Create, Read, Update & Delete) plus Search operations

        /// <summary>
        /// If available returns the read-only layer that provides <paramref name="layerType"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerType">Type of layer to search for</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch> GetLayerCrudSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TEntity : class where TSearch : class;

        /// <summary>
        /// If available returns the read-only layer that provides <paramref name="layerTypeName"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerTypeName">Name of the layer type to search for (for use with layers that are <see cref="OcLayerType.Other"/>).</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch> GetLayerCrudSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class where TSearch : class;

      #endregion
    }
}