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
using JetBrains.Annotations;
using OriginalCoder.Data;
using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Interfaces.Data;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Repositories
{
    [PublicAPI]
    public abstract class OcLayerRepositoryReadBase<TRequest, TEntity, TKey> : OcLayerRepositoryBase<TRequest, TEntity, TKey>, IOcLayerRead<TRequest, TEntity, TKey>
        where TRequest : class, IOcRequest
        where TEntity : class
    {
      #region Constructors

        protected OcLayerRepositoryReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        { }

        protected OcLayerRepositoryReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        { }

      #endregion

      #region ICrudRead<TEntity>

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get()
        {
            if (HasErrors)
                return null;

            var results = new List<TEntity>();
            foreach (var entity in Query())
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
            if (HasErrors)
                return null;

            // Execute the operation
            var entity = FindSingle(Query(), key);

            // Apply logic & return output
            if (entity != null && ApplyLogic(entity, OcDataOperationType.Read))
                return entity;
            return null;
        }

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get(IEnumerable<TKey> keys)
        {
            if (HasErrors)
                return null;

            var entities = FindMany(Query(), keys);

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
            if (HasErrors)
                return null;

            var entities = Paged(Query(), qtySkip, qtyReturn, sortOrder);

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
