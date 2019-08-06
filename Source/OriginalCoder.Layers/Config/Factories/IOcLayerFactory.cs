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
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Config.Factories
{
    /// <summary>
    /// Factory for obtaining typed resources and resource layers.
    /// </summary>
    [PublicAPI]
    public interface IOcLayerFactory<TRequest>
        where TRequest : class, IOcRequest
    {
        /// <summary>
        /// If available returns the resource that is uniquely associated with <typeparamref name="TInterface"/>.
        /// Returns null if a suitable resource could not be found.
        /// </summary>
        /// <typeparam name="TInterface">Interface type that uniquely specifies the resource to obtain</typeparam>
        TInterface GetResource<TInterface>() where TInterface : class;

        /// <summary>
        /// If available returns the resource that is uniquely associated with <typeparamref name="TInterface"/>.
        /// Returns null if a suitable resource could not be found.
        /// If the found interface supports <paramref name="request"/> it will be configured to use those before returning.
        /// </summary>
        /// <typeparam name="TInterface">Interface type that uniquely specifies the resource to obtain</typeparam>
        TInterface GetResource<TInterface>([NotNull] TRequest request) where TInterface : class;

        /// <summary>
        /// If available returns the layer that provides <paramref name="layerType"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerType">Type of layer to search for</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        IOcLayer<TRequest> GetResourceLayer([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request);

        /// <summary>
        /// If available returns the specified interface to the layer that provides <paramref name="layerType"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <typeparam name="TLayer">The interface (which inherits from <see cref="IOcLayer{TRequest}"/> being sought.</typeparam>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerType">Type of layer to search for</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        TLayer GetResourceLayer<TLayer>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TLayer : class, IOcLayer<TRequest>;

        /// <summary>
        /// If available returns the layer that provides <paramref name="layerTypeName"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerTypeName">Name of the layer type to search for (for use with layers that are <see cref="OcLayerType.Other"/>).</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        IOcLayer<TRequest> GetResourceLayer([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request);

        /// <summary>
        /// If available returns the specified interface to the layer that provides <paramref name="layerTypeName"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <typeparam name="TLayer">The interface (which inherits from <see cref="IOcLayer{TRequest}"/> being sought.</typeparam>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerTypeName">Name of the layer type to search for (for use with layers that are <see cref="OcLayerType.Other"/>).</param>
        /// <param name="request">The request the layer will be used to fulfill.</param>
        TLayer GetResourceLayer<TLayer>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TLayer : class, IOcLayer<TRequest>;
    }
}