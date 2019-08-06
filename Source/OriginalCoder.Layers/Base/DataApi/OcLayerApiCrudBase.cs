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
using OriginalCoder.Common.Api;
using OriginalCoder.Common.Extensions;
using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Interfaces.Data;
using OriginalCoder.Layers.Interfaces.DataApi;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Base.DataApi
{
    [PublicAPI]
    public abstract class OcLayerApiCrudBase<TRequest, TEntity, TKey, TNext> : OcLayerApiReadBase<TRequest, TEntity, TKey, TNext>, IOcLayerApiCrud<TRequest, TEntity, TKey>
        where TRequest : class, IOcRequest
        where TEntity : class
        where TNext : class, IOcLayerCrud<TRequest, TEntity, TKey>
    {
      #region Constructors

        protected OcLayerApiCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        { }

        protected OcLayerApiCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        { }

        protected OcLayerApiCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerType, resourceName, request, nextLayer)
        { }

        protected OcLayerApiCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerTypeName, resourceName, request, nextLayer)
        { }

        protected OcLayerApiCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType)
            : base(configuration, layerType, resourceName, request, nextLayerType)
        { }

        protected OcLayerApiCrudBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType, string nextLayerTypeName)
            : base(configuration, layerTypeName, resourceName, request, nextLayerType, nextLayerTypeName)
        { }

      #endregion

      #region ICrudCreate<TEntity>

        /// <inheritdoc />
        public virtual IOcApiResult<TEntity> CreateAndReturn(TEntity entity)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            return new OcApiResult<TEntity>($"{ResourceName}->{MethodBase.GetCurrentMethod().Name}", NextLayer.CreateAndReturn(entity), ResponseMessages);
        }

      #endregion

      #region ICrudUpdate<TEntity>

        /// <inheritdoc />
        public virtual IOcApiResult<bool> Update(TEntity entity)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            return new OcApiResult<bool>($"{ResourceName}->{MethodBase.GetCurrentMethod().Name}", NextLayer.Update(entity), ResponseMessages);
        }

        /// <inheritdoc />
        public virtual IOcApiResult<TEntity> UpdateAndReturn(TEntity entity)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            return new OcApiResult<TEntity>($"{ResourceName}->{MethodBase.GetCurrentMethod().Name}", NextLayer.UpdateAndReturn(entity), ResponseMessages);
        }

      #endregion

      #region ICrudCreate<TEntity, TKey>

        /// <inheritdoc />
        public virtual IOcApiResult<TKey> Create(TEntity entity)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            return new OcApiResult<TKey>($"{ResourceName}->{MethodBase.GetCurrentMethod().Name}", NextLayer.Create(entity), ResponseMessages);
        }

      #endregion

      #region ICrudDelete<TKey>

        /// <inheritdoc />
        public virtual IOcApiResult<bool> Delete(TKey key)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            return new OcApiResult<bool>($"{ResourceName}->{MethodBase.GetCurrentMethod().Name}", NextLayer.Delete(key), ResponseMessages);
        }

        /// <inheritdoc />
        public virtual IOcApiResult<int> Delete(IEnumerable<TKey> keys)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            return new OcApiResult<int>($"{ResourceName}->{MethodBase.GetCurrentMethod().Name}", NextLayer.Delete(keys), ResponseMessages);
        }

      #endregion
    }
}