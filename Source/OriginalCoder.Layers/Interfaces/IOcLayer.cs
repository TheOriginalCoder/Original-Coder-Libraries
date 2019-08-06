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
using JetBrains.Annotations;
using OriginalCoder.Common.Interfaces.Properties;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Interfaces
{
    /// <summary>
    /// Base interface for a request processing layer.
    /// </summary>
    [PublicAPI]
    public interface IOcLayer : IDisposable, IName, IDescription, IStatusSummary, IProperties
    {
        /// <summary>
        /// Specifies the type of Layer.
        /// If this is <see cref="OcLayerType.Other"/> then <see cref="LayerTypeName"/> should specify the type.
        /// <see cref="OcLayerType.Unknown"/> is not permitted.
        /// </summary>
        OcLayerType LayerType { get; }

        /// <summary>
        /// Name of the layer type.
        /// This should match <see cref="LayerType"/> unless it is <see cref="OcLayerType.Other"/>.
        /// </summary>
        string LayerTypeName { get; }

        /// <summary>
        /// Name of the resource exposed by the repository
        /// </summary>
        string ResourceName { get; }

        /// <summary>
        /// Unique integer ID for the repository instance (intended for use in debugging)
        /// </summary>
        long InstanceId { get; }
    }

    /// <summary>
    /// Base interface for a request processing layer.
    /// </summary>
    /// <typeparam name="TRequest">Interface type of the request object</typeparam>
    [PublicAPI]
    public interface IOcLayer<out TRequest> : IOcLayer
        where TRequest : class, IOcRequest
    {
        /// <summary>
        /// Provides information about and status of the request to be handled
        /// </summary>
        TRequest Request { get; }
    }

    [PublicAPI]
    public interface IOcLayer<out TRequest, in TEntity, TKey> : IOcLayer<TRequest>
        where TRequest : class, IOcRequest
        where TEntity : class
    {
      #region Entity Functions

        /// <summary>
        /// Returns the primary key value for <paramref name="entity"/>.
        /// </summary>
        TKey EntityKey(TEntity entity);

        /// <summary>
        /// Returns true if <paramref name="entity"/> is marked as deleted.
        /// For systems that hard delete data from the data store this can always return null.
        /// For systems that maintain deleted records by marking them deleted, override this to return if they are marked as deleted.
        /// </summary>
        bool EntityIsInactive(TEntity entity);

        string EntityStatusSummary(TKey key);

        string EntityStatusSummary(TEntity entity);

      #endregion
    }
}