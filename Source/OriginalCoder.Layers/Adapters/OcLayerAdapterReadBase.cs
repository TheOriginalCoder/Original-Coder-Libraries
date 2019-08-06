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
    public class OcLayerAdapterReadBase<TRequest, TEntity, TEntityKey, TNextEntity, TNextLayer> : OcLayerAdapterBase<TRequest, TEntity, TEntityKey, TNextLayer>, IOcLayerRead<TRequest, TEntity, TEntityKey>
        where TRequest : class, IOcRequest
        where TEntity : class
        where TNextEntity : class
        where TNextLayer : class, IOcLayerRead<TRequest, TNextEntity, TEntityKey>
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
        protected OcLayerAdapterReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, TNextLayer nextLayer)
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
        protected OcLayerAdapterReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, TNextLayer nextLayer)
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
        protected OcLayerAdapterReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType)
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
        protected OcLayerAdapterReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType, string nextLayerTypeName)
            : base(configuration, layerTypeName, resourceName, request, nextLayerType, nextLayerTypeName)
        { }

      #endregion

      #region ICrudRead<TEntity>

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get()
        {
            if (NextLayer == null)
                throw NewException($"Adapter from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()} for {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            try
            { return DataMapper.ConvertToList<TNextEntity, TEntity>(NextLayer.Get()); }
            catch (OcDataMappingException ex)
            { throw NewException($"Error mapping data from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()}", ex, typeof(TEntity), typeof(TNextEntity)); }
        }

      #endregion

      #region ICrudRead<TEntity, TKey>

        /// <inheritdoc />
        public virtual TEntity Get(TEntityKey key)
        {
            if (NextLayer == null)
                throw NewException($"Adapter from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()} for {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            try
            { return DataMapper.Convert<TNextEntity, TEntity>(NextLayer.Get(key)); }
            catch (OcDataMappingException ex)
            { throw NewException($"Error mapping data from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()}", ex, typeof(TEntity), typeof(TNextEntity)); }
        }

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get(IEnumerable<TEntityKey> keys)
        {
            if (NextLayer == null)
                throw NewException($"Adapter from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()} for {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            try
            { return DataMapper.ConvertToList<TNextEntity, TEntity>(NextLayer.Get(keys)); }
            catch (OcDataMappingException ex)
            { throw NewException($"Error mapping data from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()}", ex, typeof(TEntity), typeof(TNextEntity)); }
        }

      #endregion

      #region ICrudReadPaged<TEntity>

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get(int qtySkip, int qtyReturn, IReadOnlyList<(string columnName, bool ascending)> sortOrder = null)
        {
            if (NextLayer == null)
                throw NewException($"Adapter from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()} for {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            try
            { return DataMapper.ConvertToList<TNextEntity, TEntity>(NextLayer.Get(qtySkip, qtyReturn, sortOrder)); }
            catch (OcDataMappingException ex)
            { throw NewException($"Error mapping data from {typeof(TEntity).FriendlyName()} to {typeof(TNextEntity).FriendlyName()}", ex, typeof(TEntity), typeof(TNextEntity)); }
        }

      #endregion
    }
}
