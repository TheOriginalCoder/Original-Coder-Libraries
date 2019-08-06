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
    public abstract class OcLayerLogicCrudBase<TRequest, TEntity, TKey, TNext> : OcLayerLogicReadBase<TRequest, TEntity, TKey, TNext>, IOcLayerCrud<TRequest, TEntity, TKey>
        where TRequest : class, IOcRequest
        where TEntity : class
        where TNext : class, IOcLayerCrud<TRequest, TEntity, TKey>
    {
      #region Constructors

        protected OcLayerLogicCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        { }

        protected OcLayerLogicCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        { }

        protected OcLayerLogicCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerType, resourceName, request, nextLayer)
        { }

        protected OcLayerLogicCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerTypeName, resourceName, request, nextLayer)
        { }

        protected OcLayerLogicCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType)
            : base(configuration, layerType, resourceName, request, nextLayerType)
        { }

        protected OcLayerLogicCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType, string nextLayerTypeName)
            : base(configuration, layerTypeName, resourceName, request, nextLayerType, nextLayerTypeName)
        { }

      #endregion

      #region ICrudCreate<TEntity>

        /// <inheritdoc />
        public virtual TEntity CreateAndReturn(TEntity entity)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");
            if (HasErrors)
                return null;

            // Apply logic & execute operation
            if (entity != null && ApplyLogic(entity, OcDataOperationType.Create))
                return NextLayer.CreateAndReturn(entity);
            return null;
        }

      #endregion

      #region ICrudUpdate<TEntity>

        /// <inheritdoc />
        public virtual bool Update(TEntity entity)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");
            if (HasErrors)
                return false;

            // Apply logic & execute operation
            if (entity != null && ApplyLogic(entity, OcDataOperationType.Update))
                return NextLayer.Update(entity);

            return false;
        }

        /// <inheritdoc />
        public virtual TEntity UpdateAndReturn(TEntity entity)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");
            if (HasErrors)
                return null;

            // Apply logic & execute operation
            if (entity != null && ApplyLogic(entity, OcDataOperationType.Update))
                return NextLayer.UpdateAndReturn(entity);

            return null;
        }

      #endregion

      #region ICrudCreate<TEntity, TKey>

        /// <inheritdoc />
        public virtual TKey Create(TEntity entity)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");
            if (HasErrors)
                return default;

            // Apply logic & execute operation
            if (entity != null && ApplyLogic(entity, OcDataOperationType.Create))
                return NextLayer.Create(entity);

            return default;
        }

      #endregion

      #region ICrudDelete<TKey>

        /// <inheritdoc />
        public virtual bool Delete(TKey key)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");
            if (HasErrors)
                return false;

            var entity = NextLayer.Get(key);

            // Apply logic & execute operation
            if (entity != null && ApplyLogic(entity, OcDataOperationType.Delete))
                return NextLayer.Delete(key);

            return false;
        }

        /// <inheritdoc />
        public virtual int Delete(IEnumerable<TKey> keys)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");
            if (HasErrors)
                return 0;

            var entities = NextLayer.Get(keys);

            var count = 0;
            foreach (var entity in entities)
            {
                if (entity != null && ApplyLogic(entity, OcDataOperationType.Delete) && NextLayer.Delete(EntityKey(entity)))
                    count++;
            }
            return count;
        }

      #endregion
    }
}