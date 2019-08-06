//=============================================================================
// Original Coder - Layer Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using OriginalCoder.Common.Extensions;
using OriginalCoder.Data.Mapper;
using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Interfaces.Data;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Adapters
{
    [PublicAPI]
    public abstract class OcLayerAdapterReadSearchBase<TRequest, TEntity, TKey, TSearch, TNextEntity, TNextLayer> : OcLayerAdapterReadBase<TRequest, TEntity, TKey, TNextEntity, TNextLayer>, IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch>
        where TRequest : class, IOcRequest
        where TEntity : class
        where TSearch : class
        where TNextEntity : class
        where TNextLayer : class, IOcLayerReadSearch<TRequest, TNextEntity, TKey, TSearch>
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
        protected OcLayerAdapterReadSearchBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, TNextLayer nextLayer)
            : base(configuration, layerType, resourceName, request, nextLayer)
        { }

        /// <summary>
        /// Preferred constructor for use with <see cref="OcLayerType.Other"/> and specifying a <paramref name="layerTypeName"/>.
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerTypeName"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        /// <param name="nextLayer"></param>
        protected OcLayerAdapterReadSearchBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, TNextLayer nextLayer)
            : base(configuration, layerTypeName, resourceName, request, nextLayer)
        { }

        /// <summary>
        /// Preferred constructor when <paramref name="layerType"/> is not <see cref="OcLayerType.Other"/>
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerType"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        /// <param name="nextLayerType"></param>
        protected OcLayerAdapterReadSearchBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType)
            : base(configuration, layerType, resourceName, request, nextLayerType)
        { }

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
        protected OcLayerAdapterReadSearchBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType, string nextLayerTypeName)
            : base(configuration, layerTypeName, resourceName, request, nextLayerType, nextLayerTypeName)
        { }

      #endregion

      #region ICrudSearch<TEntity, TSearch>

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get(TSearch searchParams)
        {
            if (NextLayer == null)
                throw NewException($"Adapter from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()} for {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            try
            { return DataMapper.ConvertToList<TNextEntity, TEntity>(NextLayer.Get(searchParams)); }
            catch (OcDataMappingException ex)
            { throw NewException($"Error mapping data from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()}", ex, typeof(TEntity), typeof(TNextEntity)); }
        }

      #endregion

      #region ICrudSearchPaged<TEntity, TSearch>

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get(TSearch searchParams, int qtySkip, int qtyReturn, IReadOnlyList<(string columnName, bool ascending)> sortOrder = null)
        {
            if (NextLayer == null)
                throw NewException($"Adapter from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()} for {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            try
            { return DataMapper.ConvertToList<TNextEntity, TEntity>(NextLayer.Get(searchParams, qtySkip, qtyReturn, sortOrder)); }
            catch (OcDataMappingException ex)
            { throw NewException($"Error mapping data from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()}", ex, typeof(TEntity), typeof(TNextEntity)); }
        }

      #endregion
    }
}