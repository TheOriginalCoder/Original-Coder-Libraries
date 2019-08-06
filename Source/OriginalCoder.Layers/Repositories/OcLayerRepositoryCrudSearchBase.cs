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
using System.Linq;
using JetBrains.Annotations;
using OriginalCoder.Data;
using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Interfaces.Data;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Repositories
{
    [PublicAPI]
    public abstract class OcLayerRepositoryCrudSearchBase<TRequest, TEntity, TKey, TSearch> : OcLayerRepositoryCrudBase<TRequest, TEntity, TKey>, IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch>
        where TRequest : class, IOcRequest
        where TEntity : class
        where TSearch : class
    {
      #region Constructors

        protected OcLayerRepositoryCrudSearchBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        { }

        protected OcLayerRepositoryCrudSearchBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        { }

      #endregion

        /// <summary>
        /// Filters the records in <paramref name="query"/> according to the criteria specified in <paramref name="searchParams"/>.
        /// </summary>
        protected abstract IQueryable<TEntity> FilterForSearch(IQueryable<TEntity> query, TSearch searchParams);

      #region ICrudSearch<TEntity, TSearch>

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get(TSearch searchParams)
        {
            if (HasErrors)
                return null;

            var entities = FilterForSearch(Query(), searchParams);

            var results = new List<TEntity>();
            foreach (var entity in entities)
            {
                if (entity != null && ApplyLogic(entity, OcDataOperationType.Read))
                    results.Add(entity);
            }
            return results;
        }

      #endregion

      #region ICrudSearchPaged<TEntity, TSearch>

        /// <inheritdoc />
        public virtual IReadOnlyList<TEntity> Get(TSearch searchParams, int qtySkip, int qtyReturn, IReadOnlyList<(string columnName, bool ascending)> sortOrder = null)
        {
            if (HasErrors)
                return null;

            var entities = Paged(FilterForSearch(Query(), searchParams), qtySkip, qtyReturn, sortOrder);

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