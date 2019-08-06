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
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using OriginalCoder.Common.Api;
using OriginalCoder.Data;
using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Interfaces.Data;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Repositories
{
    [PublicAPI]
    public abstract class OcLayerRepositoryCrudBase<TRequest, TEntity, TKey> : OcLayerRepositoryReadBase<TRequest, TEntity, TKey>, IOcLayerCrud<TRequest, TEntity, TKey>
        where TRequest : class, IOcRequest
        where TEntity : class
    {
      #region Constructors

        protected OcLayerRepositoryCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        { }

        protected OcLayerRepositoryCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        { }

      #endregion

      #region Abstract Database Operations

        protected abstract TEntity DoCreate(TEntity entity);
        protected abstract TEntity DoUpdate(TEntity entity);
        protected abstract bool DoDelete(TEntity entity);
        protected abstract int DoDelete(IEnumerable<TEntity> entities);

      #endregion

      #region ICrudCreate<TEntity, TKey>

        /// <inheritdoc />
        public virtual TKey Create([NotNull] TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (HasErrors)
                return default;

            var result = CreateAndReturn(entity);
            if (result == null && !HasErrors)
                ResponseMessageAdd(OcApiMessageType.Error, $"An error occurred in Resource [{ResourceName}] when attempting to create Entity [{EntityStatusSummary(entity)}]");

            return HasErrors ? default : EntityKey(result);
        }

      #endregion

      #region ICrudCreate<TEntity>

        /// <inheritdoc />
        public virtual TEntity CreateAndReturn([NotNull] TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (HasErrors)
                return null;

            // Apply logic & execute operation
            if (!ApplyLogic(entity, OcDataOperationType.Create))
            {
                if (!HasErrors)
                    ResponseMessageAdd(OcApiMessageType.Error, $"An error occurred in Resource [{ResourceName}] when attempting to create Entity [{EntityStatusSummary(entity)}]");
                return null;
            }

            var result = DoCreate(entity);

            if (result == null && !HasErrors)
                ResponseMessageAdd(OcApiMessageType.Error, $"An error occurred in Resource [{ResourceName}] when attempting to create Entity [{EntityStatusSummary(entity)}]");

            return result;
        }

      #endregion

      #region ICrudUpdate<TEntity>

        /// <inheritdoc />
        public virtual bool Update([NotNull] TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (HasErrors)
                return false;

            var result = UpdateAndReturn(entity);
            if (HasErrors)
                return false;

            return result != null;
        }

        /// <inheritdoc />
        public virtual TEntity UpdateAndReturn(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (HasErrors)
                return null;

            if (!ApplyLogic(entity, OcDataOperationType.Update))
            {
                if (!HasErrors)
                    ResponseMessageAdd(OcApiMessageType.Error, $"An error occurred in Resource [{ResourceName}] when attempting to update Entity [{EntityStatusSummary(entity)}]");
                return null;
            }

            var result = DoUpdate(entity);
            if (result == null && !HasErrors)
                ResponseMessageAdd(OcApiMessageType.Error, $"An error occurred in Resource [{ResourceName}] when attempting to update Entity [{EntityStatusSummary(entity)}]");

            return result;
        }

      #endregion

      #region ICrudDelete<TKey>

        /// <inheritdoc />
        public virtual bool Delete(TKey key)
        {
            if (HasErrors)
                return false;

            var entity = FindSingle(Query(), key);
            if (entity == null || EntityIsInactive(entity))
            {
                ResponseMessageAdd(OcApiMessageType.NotificationDev, $"Delete not necessary because entity with Key [{key}] not found in Resource [{ResourceName}]");
                return false;
            }

            if (!ApplyLogic(entity, OcDataOperationType.Delete))
            {
                if (!HasErrors)
                    ResponseMessageAdd(OcApiMessageType.Error, $"An error occurred in Resource [{ResourceName}] when attempting to delete Entity [{EntityStatusSummary(entity)}]");
                return false;
            }

            var result = DoDelete(entity);
            if (result == false && !HasErrors)
                ResponseMessageAdd(OcApiMessageType.Error, $"An error occurred in Resource [{ResourceName}] when attempting to delete Entity [{EntityStatusSummary(entity)}]");

            return result;
        }

        /// <inheritdoc />
        public virtual int Delete(IEnumerable<TKey> keys)
        {
            if (HasErrors)
                return 0;

            var entities = FindMany(Query(), keys).Where(e => !EntityIsInactive(e));
            if (!ApplyLogic(entities, OcDataOperationType.Delete))
            {
                if (!HasErrors)
                    ResponseMessageAdd(OcApiMessageType.Error, $"An error occurred in Resource [{ResourceName}] when attempting to delete multiple Entities");
                return 0;
            }

            var count = DoDelete(entities);

            return count;
        }

      #endregion
    }
}