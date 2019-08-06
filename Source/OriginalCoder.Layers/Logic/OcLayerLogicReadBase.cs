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
using OriginalCoder.Data;
using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Interfaces.Data;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Logic
{
    [PublicAPI]
    public abstract class OcLayerLogicReadBase<TRequest, TEntity, TKey, TNext> : OcLayerLogicBase<TRequest, TEntity, TKey, TNext>, IOcLayerRead<TRequest, TEntity, TKey>
        where TRequest : class, IOcRequest
        where TEntity : class
        where TNext : class, IOcLayerRead<TRequest, TEntity, TKey>
    {
      #region Constructors

        protected OcLayerLogicReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        { }

        protected OcLayerLogicReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        { }

        protected OcLayerLogicReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerType, resourceName, request, nextLayer)
        { }

        protected OcLayerLogicReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerTypeName, resourceName, request, nextLayer)
        { }

        protected OcLayerLogicReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType)
            : base(configuration, layerType, resourceName, request, nextLayerType)
        { }

        protected OcLayerLogicReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType, string nextLayerTypeName)
            : base(configuration, layerTypeName, resourceName, request, nextLayerType, nextLayerTypeName)
        { }

      #endregion

      #region ICrudRead<TEntity>

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get()
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");
            if (HasErrors)
                return null;

            var entities = NextLayer.Get();

            var results = new List<TEntity>();
            foreach (var entity in entities)
            {
                if (entity != null && ApplyLogic(entity, OcDataOperationType.Read))
                    results.Add(entity);
            }
            return results;
        }

      #endregion

      #region ICrudRead<TEntity, TKey>

        /// <inheritdoc />
        public virtual TEntity Get(TKey key)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");
            if (HasErrors)
                return null;

            // Execute the operation
            var entity = NextLayer.Get(key);

            // Apply logic & return output
            if (entity != null && ApplyLogic(entity, OcDataOperationType.Read))
                return entity;
            return null;
        }

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get(IEnumerable<TKey> keys)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");
            if (HasErrors)
                return null;

            var entities = NextLayer.Get(keys);

            var results = new List<TEntity>();
            foreach (var entity in entities)
            {
                if (entity != null && ApplyLogic(entity, OcDataOperationType.Read))
                    results.Add(entity);
            }
            return results;
        }

      #endregion

      #region ICrudReadPaged<TEntity>

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get(int qtySkip, int qtyReturn, IReadOnlyList<(string columnName, bool ascending)> sortOrder = null)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");
            if (HasErrors)
                return null;

            var entities = NextLayer.Get(qtySkip, qtyReturn, sortOrder);

            var results = new List<TEntity>();
            foreach (var entity in entities)
            {
                if (entity != null && ApplyLogic(entity, OcDataOperationType.Read))
                    results.Add(entity);
            }
            return results;
        }

      #endregion
    }
}
