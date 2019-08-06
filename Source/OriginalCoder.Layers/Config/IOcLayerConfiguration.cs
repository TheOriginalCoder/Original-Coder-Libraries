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
using OriginalCoder.Data.Mapper;
using OriginalCoder.Layers.Config.Factories;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Config
{
    [PublicAPI]
    public interface IOcLayerConfiguration
    {
      #region Services

        /// <summary>
        /// Service for converting data types into other data types.
        /// </summary>
        IOcDataMapper DataMapper { get; }

      #endregion

      #region Allowed Layer Types & Layer Type Names

        /// <summary>
        /// Returns true if <paramref name="layerType"/> is allowed in the system.
        /// </summary>
        bool IsAllowed(OcLayerType layerType);

        /// <summary>
        /// Returns true if <paramref name="layerTypeName"/> is allowed in the system.
        /// Note that this only works for non-standard layer type names.
        /// For cases where Layer Type is not <see cref="OcLayerType.Other"/> use one of the other overloads instead.
        /// </summary>
        bool IsAllowed(string layerTypeName);

        /// <summary>
        /// Returns true if the specified layer type is allowed in the system.
        /// Note that if <see cref="OcLayerType.Other"/> is specified then <paramref name="layerTypeName"/> must have a value.
        /// </summary>
        bool IsAllowed(OcLayerType layerType, string layerTypeName);

      #endregion

      #region Allowed Layer Type Interactions

        /// <summary>
        /// Returns true if <paramref name="fromLayerType"/> is allowed to access <paramref name="toLayerType"/>.
        /// Note that if no layer interaction restrictions have been configured then any valid layer is allowed to interact with any other valid layer.
        /// </summary>
        bool IsAllowed(OcLayerType fromLayerType, OcLayerType toLayerType);

        /// <summary>
        /// Returns true if the From Layer Type is allowed to access the To Layer Type.
        /// Note that if no layer interaction restrictions have been configured then any valid layer is allowed to interact with any other valid layer.
        /// </summary>
        bool IsAllowed(OcLayerType fromLayerType, string fromLayerTypeName, OcLayerType toLayerType, string toLayerTypeName);

      #endregion
    }

    [PublicAPI]
    public interface IOcLayerConfiguration<TRequest> : IOcLayerConfiguration, IOcLayerFactoryFactory<TRequest>, IOcLayerCrudFactory<TRequest>
        where TRequest : class, IOcRequest
    {
      #region Services

        /// <summary>
        /// The Authorization Server to be used.
        /// </summary>
        IOcAuthorizationService<TRequest> AuthorizationService { get; }

      #endregion
    }
}