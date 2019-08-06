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
    public abstract class OcLayerApiReadBase<TRequest, TEntity, TKey, TNext> : OcLayerBase<TRequest, TEntity, TKey, TNext>, IOcLayerApiRead<TRequest, TEntity, TKey>
        where TRequest : class, IOcRequest
        where TEntity : class
        where TNext : class, IOcLayerRead<TRequest, TEntity, TKey>
    {
      #region Constructors

        protected OcLayerApiReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        { }

        protected OcLayerApiReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        { }

        protected OcLayerApiReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerType, resourceName, request, nextLayer)
        { }

        protected OcLayerApiReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerTypeName, resourceName, request, nextLayer)
        { }

        protected OcLayerApiReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType)
            : base(configuration, layerType, resourceName, request, nextLayerType)
        { }

        protected OcLayerApiReadBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType, string nextLayerTypeName)
            : base(configuration, layerTypeName, resourceName, request, nextLayerType, nextLayerTypeName)
        { }

      #endregion

      #region ICrudRead<TEntity>

        /// <inheritdoc />
        public virtual IOcApiResult<IReadOnlyList<TEntity>> Get()
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            return new OcApiResult<IReadOnlyList<TEntity>>($"{ResourceName}->{MethodBase.GetCurrentMethod().Name}", NextLayer.Get(), ResponseMessages);
        }

      #endregion

      #region ICrudRead<TEntity, TKey>

        /// <inheritdoc />
        public virtual IOcApiResult<TEntity> Get(TKey key)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            return new OcApiResult<TEntity>($"{ResourceName}->{MethodBase.GetCurrentMethod().Name}", NextLayer.Get(key), ResponseMessages);
        }

        /// <inheritdoc />
        public virtual IOcApiResult<IReadOnlyList<TEntity>> Get(IEnumerable<TKey> keys)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            return new OcApiResult<IReadOnlyList<TEntity>>($"{ResourceName}->{MethodBase.GetCurrentMethod().Name}", NextLayer.Get(keys), ResponseMessages);
        }

      #endregion

      #region ICrudReadPaged<TEntity>

        /// <inheritdoc />
        public virtual IOcApiResult<IReadOnlyList<TEntity>> Get(int qtySkip, int qtyReturn, IReadOnlyList<(string columnName, bool ascending)> sortOrder = null)
        {
            if (NextLayer == null)
                throw NewException($"Default implementation of {ResourceName}->{MethodBase.GetCurrentMethod().Name} in <{GetType().FriendlyName()}> can not pass through because {nameof(NextLayer)} is null.");

            return new OcApiResult<IReadOnlyList<TEntity>>($"{ResourceName}->{MethodBase.GetCurrentMethod().Name}", NextLayer.Get(qtySkip, qtyReturn, sortOrder), ResponseMessages);
        }

      #endregion
    }
}
