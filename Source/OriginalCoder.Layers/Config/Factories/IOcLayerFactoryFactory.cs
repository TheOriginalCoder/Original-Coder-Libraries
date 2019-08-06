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
    [PublicAPI]
    public interface IOcLayerFactoryFactory<TRequest>
        where TRequest : class, IOcRequest
    {
        /// <summary>
        /// Returns an unrestricted Layer Resource Factory
        /// </summary>
        [NotNull] IOcLayerFactory<TRequest> Factory();

        /// <summary>
        /// If available returns an unrestricted Layer Resource Factory with additional methods to more easily obtain layers that support Read, Read + Search, CRUD and CRUD + Search operations.
        /// </summary>
        [CanBeNull] IOcLayerCrudFactory<TRequest> FactoryCrud();

        /// <summary>
        /// Returns a Layer Resource Factory that is restricted by <paramref name="layerType"/> to enforce any configured restrictions for interactions between layers.
        /// </summary>
        /// <remarks>
        /// <paramref name="layerType"/> can not be <see cref="OcLayerType.Unknown"/>.
        /// If <paramref name="layerType"/> is <see cref="OcLayerType.Other"/> use the overload that takes a layer type name instead.
        /// </remarks>
        [NotNull] IOcLayerRestrictedFactory<TRequest> RestrictedFactory(OcLayerType layerType);

        /// <summary>
        /// Returns a Layer Resource Factory that is restricted by <paramref name="layerTypeName"/> to enforce any configured restrictions for interactions between layers.
        /// </summary>
        /// <remarks>
        /// This overload is only applicable for use with <see cref="OcLayerType.Other"/>.
        /// </remarks>
        [NotNull] IOcLayerRestrictedFactory<TRequest> RestrictedFactory(string layerTypeName);

        /// <summary>
        /// Returns a Layer Resource Factory that is restricted by <paramref name="layerType"/> to enforce any configured restrictions for interactions between layers.
        /// The returned factory has additional methods to more easily obtain layers that support Read, Read + Search, CRUD and CRUD + Search operations.
        /// </summary>
        /// <remarks>
        /// <paramref name="layerType"/> can not be <see cref="OcLayerType.Unknown"/>.
        /// If <paramref name="layerType"/> is <see cref="OcLayerType.Other"/> use the overload that takes a layer type name instead.
        /// </remarks>
        [CanBeNull] IOcLayerRestrictedCrudFactory<TRequest> RestrictedFactoryCrud(OcLayerType layerType);

        /// <summary>
        /// Returns a Layer Resource Factory that is restricted by <paramref name="layerTypeName"/> to enforce any configured restrictions for interactions between layers.
        /// The returned factory has additional methods to more easily obtain layers that support Read, Read + Search, CRUD and CRUD + Search operations.
        /// </summary>
        /// <remarks>
        /// This overload is only applicable for use with <see cref="OcLayerType.Other"/>.
        /// </remarks>
        [CanBeNull] IOcLayerRestrictedCrudFactory<TRequest> RestrictedFactoryCrud(string layerTypeName);

        /// <summary>
        /// Returns a Layer Resource Factory that is restricted by <paramref name="layerType"/> and <paramref name="layerTypeName"/> to enforce any configured restrictions for interactions between layers.
        /// The returned factory has additional methods to more easily obtain layers that support Read, Read + Search, CRUD and CRUD + Search operations.
        /// </summary>
        /// <remarks>
        /// <paramref name="layerType"/> can not be <see cref="OcLayerType.Unknown"/>.
        /// If <paramref name="layerType"/> is <see cref="OcLayerType.Other"/> then <paramref name="layerTypeName"/> must be specified.
        /// </remarks>
        [CanBeNull] IOcLayerRestrictedCrudFactory<TRequest> RestrictedFactoryCrud(OcLayerType layerType, string layerTypeName);
    }
}