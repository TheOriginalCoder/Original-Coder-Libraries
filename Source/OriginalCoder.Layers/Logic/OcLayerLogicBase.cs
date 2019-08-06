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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OriginalCoder.Common.Api;
using OriginalCoder.Common.Exceptions;
using OriginalCoder.Common.Extensions;
using OriginalCoder.Data;
using OriginalCoder.Layers.Base;
using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Exceptions;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Logic
{
    /// <summary>
    /// Generic base class that adds in common types of logic that are broadly useful when implementing layers.
    /// </summary>
    [PublicAPI]
    public abstract class OcLayerLogicBase<TRequest, TEntity, TKey> : OcLayerBase<TRequest, TEntity, TKey>
        where TRequest : class, IOcRequest
        where TEntity : class
    {
      #region Constructors

        protected OcLayerLogicBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        {
            AuthorizationService = configuration.AuthorizationService;
        }

        protected OcLayerLogicBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        {
            AuthorizationService = configuration.AuthorizationService;
        }

      #endregion

      #region Applying Logic per Entity

        /// <summary>
        /// Applies logic specific to the <paramref name="operationType"/> on the passed in <paramref name="entity"/>.
        /// Any messages generated during the process will be added to the <see cref="OcApiMessages"/> collection in  <see cref="OcLayerBase{TRequest}.Request"/>.
        /// Actions taken during processing will depend on the flags set in <see cref="OcLayerBase{TRequest}.Options"/>.
        /// </summary>
        /// <param name="entity">Entity which is having the operation performed on it</param>
        /// <param name="operationType">Indicates the type of data operation being performed</param>
        /// <returns>Returns true if the operation was completed successfully, false otherwise.</returns>
        protected virtual bool ApplyLogic([NotNull] TEntity entity, OcDataOperationType operationType)
        {
            Debug.Assert(entity != null);

            // Handle Read operations
            if (operationType == OcDataOperationType.Read)
            {
                // Authorization
                if (Options.HasFlag(OcRequestOptions.ExcludeUnauthorized) && !IsAuthorized(entity, OcDataOperationType.Read))
                {
                    if (Options.HasFlag(OcRequestOptions.MessageUnauthorized))
                        ResponseMessageAdd(OcApiMessageType.NotificationSystem, $"Excluding {EntityStatusSummary(entity)} in {ResourceName} because the user is not authorized");
                    return false;
                }
                // Deleted
                if (Options.HasFlag(OcRequestOptions.ExcludeInactive) && EntityIsInactive(entity))
                {
                    if (Options.HasFlag(OcRequestOptions.MessageExcluded))
                        ResponseMessageAdd(OcApiMessageType.NotificationSystem, $"Excluding {EntityStatusSummary(entity)} in {ResourceName} because it is marked as inactive");
                    return false;
                }
                // Execute read and return success
                return ApplyReadLogic(entity);
            }

            // Authorization
            if (!IsAuthorized(entity, operationType))
            {
                ResponseMessageAdd(OcApiMessageType.AuthorizationFailure, $"{operationType} of {EntityStatusSummary(entity)} in {ResourceName} not performed because the user is not authorized");
                return false;
            }

            // Perform operation
            bool success;
            switch (operationType)
            {
                case OcDataOperationType.Create:
                    success = ApplyCreateLogic(entity);
                    break;

                case OcDataOperationType.Update:
                    success = ApplyUpdateLogic(entity);
                    break;

                case OcDataOperationType.Delete:
                    success = ApplyDeleteLogic(entity);
                    break;

                case OcDataOperationType.Other:
                    success = ApplyOtherLogic(entity);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(operationType), operationType, null);
            }
            if (!success)
            {
                if (Options.HasFlag(OcRequestOptions.VerboseMessages))
                    ResponseMessageAdd(OcApiMessageType.ValidationFailure, $"{operationType} of {EntityStatusSummary(entity)} in {ResourceName} failed during processing");
                return false;
            }

            // Validation
            if (Options.HasFlag(OcRequestOptions.ValidateEntities) && IsValid(entity) == false)
            {
                if (!ResponseMessagesHas(OcApiMessageType.ValidationFailure) || Options.HasFlag(OcRequestOptions.VerboseMessages))
                    ResponseMessageAdd(OcApiMessageType.ValidationFailure, $"{operationType} of {EntityStatusSummary(entity)} in {ResourceName} not performed because the entity failed validation");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Applies logic specific to the <paramref name="operationType"/> on the passed in <paramref name="entities"/>.
        /// Any messages generated during the process will be added to the <see cref="OcApiMessages"/> collection in  <see cref="OcLayerBase{TRequest}.Request"/>.
        /// Actions taken during processing will depend on the flags set in <see cref="OcLayerBase{TRequest}.Options"/>.
        /// </summary>
        /// <param name="entities">Collection of entities the operation is being performed on</param>
        /// <param name="operationType">Indicates the type of data operation being performed</param>
        /// <returns>Returns true if all operations and configured validation were performed successful, false otherwise.</returns>
        protected virtual bool ApplyLogic(IEnumerable<TEntity> entities, OcDataOperationType operationType)
        {
            Debug.Assert(entities != null);

            bool success;
            if (operationType == OcDataOperationType.Read)
            {
                success = false;  // Only successful if at least one record passes and can be returned.
                foreach (var entity in entities)
                    success = success || ApplyLogic(entity, OcDataOperationType.Read);
            }
            else
            {
                success = true;  // Successful if all operations processing succeeds.  If any step in processing fails abort the entire request.
                foreach (var entity in entities)
                    success = success && ApplyLogic(entity, operationType);
            }
            return success;
        }

        /// <summary>
        /// Applies logic and makes any changes to <paramref name="entity"/> that are required
        /// as part of a read request before it is returned to the client.
        /// </summary>
        /// <returns>Returns true if <paramref name="entity"/> should be included or false to exclude it</returns>
        /// <remarks>
        /// If entities in this repository have meta or calculated fields that must be initialized after being
        /// read from the data store those fields should be set by this method.
        /// </remarks>
        protected virtual bool ApplyReadLogic(TEntity entity)
        {
            Debug.Assert(entity != null);
            return true;
        }

        /// <summary>
        /// Applies logic and performs any actions on <paramref name="entity"/>
        /// that are required before it is used to create a persistent entity in the data store.
        /// </summary>
        /// <remarks>
        /// If entities in this repository have meta fields that must be initialized before a create
        /// operation is performed those fields should be set by this method.  For example
        /// if the entity includes CreatedWhen and CreatedBy fields then those should be populated by this method.
        /// </remarks>
        protected virtual bool ApplyCreateLogic(TEntity entity)
        {
            Debug.Assert(entity != null);
            return true;
        }

        /// <summary>
        /// Applies logic and performs any actions on <paramref name="entity"/>
        /// that are required before executing an update operation on the data store.
        /// </summary>
        /// <remarks>
        /// If entities in this repository have meta fields that must be updated whenever a updated action
        /// is performed then those fields should be set by this method.  For example
        /// if the entity includes UpdatedWhen and UpdatedBy fields then those should be populated by this method.
        /// </remarks>
        protected virtual bool ApplyUpdateLogic(TEntity entity)
        {
            Debug.Assert(entity != null);
            return true;
        }

        /// <summary>
        /// Applies logic and performs any actions on <paramref name="entity"/>
        /// that are required before it is deleted from the data store.
        /// The return value indicates if this repository performs hard deletes, where the data is permanently
        /// erased in the data store, or soft deletes where the entity is kept but somehow marked as deleted.
        /// In the case of soft deletes the low-level repository code will not issue a delete action to the
        /// underlying data store but will instead issue an update.
        /// </summary>
        /// <remarks>
        /// If entity uses soft deletes and has meta fields that must be set when deleting 
        /// then those fields should be set by this method.  For example
        /// if the entity includes DeletedWhen and DeletedBy fields then those should be populated by this method.
        /// </remarks>
        /// <param name="entity"></param>
        /// Returns true if the data store for this entity uses a soft deletion mechanism where
        /// the data is not actually deleted from the data store but instead is somehow marked as
        /// having been deleted.  Returns false (the default) if data entities in this repository
        /// are permanently and completely deleted by this operation.
        /// <returns></returns>
        protected virtual bool ApplyDeleteLogic(TEntity entity)
        {
            Debug.Assert(entity != null);
            return false;
        }

        /// <summary>
        /// Applies logic and performs any actions on <paramref name="entity"/>
        /// that are required before executing a non-standard operation on the data store.
        /// Note that other operations are never performed by the base class implementation,
        /// other is reserved for any operations added by the application developer.
        /// </summary>
        protected virtual bool ApplyOtherLogic(TEntity entity)
        {
            Debug.Assert(entity != null);
            return true;
        }

      #endregion

      #region Authorization Support

        protected IOcAuthorizationService<TRequest> AuthorizationService { get; }

        /// <summary>
        /// Returns true if <paramref name="operationType"/> is allowed.
        /// </summary>
        public bool IsAuthorized(OcDataOperationType operationType)
        {
            if (AuthorizationService == null)
                return true;
            return AuthorizationService.IsAuthorized(Request, ResourceName, operationType);
        }

        /// <summary>
        /// Returns true if <paramref name="operationName"/> is allowed.
        /// </summary>
        public bool IsAuthorized(string operationName)
        {
            if (AuthorizationService == null)
                return true;
            return AuthorizationService.IsAuthorized(Request, ResourceName, operationName);
        }

        /// <summary>
        /// Returns true if <paramref name="operationType"/> on <paramref name="entity"/> is allowed.
        /// </summary>
        public bool IsAuthorized(TEntity entity, OcDataOperationType operationType)
        {
            if (AuthorizationService == null)
                return true;
            return AuthorizationService.IsAuthorized(entity, Request, ResourceName, operationType);
        }

        /// <summary>
        /// Returns true if <paramref name="operationName"/> on <paramref name="entity"/> is allowed.
        /// </summary>
        public bool IsAuthorized(TEntity entity, string operationName)
        {
            if (AuthorizationService == null)
                return true;
            return AuthorizationService.IsAuthorized(entity, Request, ResourceName, operationName);
        }

      #endregion
    }


    /// <summary>
    /// Generic base class that adds in common types of logic that are broadly useful when implementing layers.
    /// </summary>
    [PublicAPI]
    public abstract class OcLayerLogicBase<TRequest, TEntity, TKey, TNext> : OcLayerLogicBase<TRequest, TEntity, TKey>
        where TRequest : class, IOcRequest
        where TEntity : class
        where TNext : class, IOcLayer<TRequest>
    {
      #region Constructors

        protected OcLayerLogicBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        {
            // For use from the bottom layer in the hierarchy (no further chaining)
            NextLayerType = OcLayerType.Unknown;
        }

        protected OcLayerLogicBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        {
            // For use from the bottom layer in the hierarchy (no further chaining)
            NextLayerType = OcLayerType.Unknown;
        }

        protected OcLayerLogicBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerType, resourceName, request)
        {
            NextLayerAssign(nextLayer);
        }

        protected OcLayerLogicBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerTypeName, resourceName, request)
        {
            NextLayerAssign(nextLayer);
        }

        protected OcLayerLogicBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType)
            : base(configuration, layerType, resourceName, request)
        {
            NextLayerType = nextLayerType;
            NextLayerTypeName = nextLayerType.ToString();
        }

        protected OcLayerLogicBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType, string nextLayerTypeName)
            : base(configuration, layerTypeName, resourceName, request)
        {
            NextLayerType = nextLayerType;
            NextLayerTypeName = nextLayerTypeName;
        }

      #endregion

      #region Layering

        public OcLayerType NextLayerType { get; private set; }
        public string NextLayerTypeName { get; private set; }

        protected TNext NextLayer
        {
            get
            {
                if (_nextLayer == null)
                    _nextLayer = FindNextLayer();
                return _nextLayer;
            }
        }
        private TNext _nextLayer;

        private void NextLayerAssign(TNext nextLayer)
        {
            if (_nextLayer != null)
                throw new OcLibraryException($"{nameof(_nextLayer)} is already assigned when calling {nameof(NextLayerAssign)}");

            if (nextLayer != null)
                DisposeAdd(nextLayer);

            _nextLayer = nextLayer;
            NextLayerType = _nextLayer?.LayerType ?? OcLayerType.Unknown;
            NextLayerTypeName = _nextLayer?.LayerTypeName;
        }

        protected TNext FindNextLayer()
        {
            Debug.Assert(Factory != null);
            if (NextLayerType == OcLayerType.Unknown)
                throw NewRoutingException($"Next layer is set to {nameof(OcLayerType.Unknown)} for {nameof(ResourceName)} [{ResourceName}], {nameof(LayerType)} [{LayerType}], {nameof(LayerTypeName)} [{LayerTypeName}]");

            var nextLayerTypeName = NextLayerType == OcLayerType.Other ? NextLayerTypeName.Trim() : NextLayerType.ToString();
            Debug.Assert(!string.IsNullOrWhiteSpace(nextLayerTypeName));

            var layer = Factory.GetResourceLayer(ResourceName, nextLayerTypeName, Request);
            if (layer != null)
                DisposeAdd(layer);  // Add layer to be automatically disposed when this instance is.

            if (layer is TNext asNext)
                return asNext;

            if (layer == null)
                throw NewRoutingException($"Unable to find any layer that matches {nameof(ResourceName)} [{ResourceName}], {nameof(NextLayerType)} [{NextLayerType}] and {nameof(NextLayerTypeName)} [{NextLayerTypeName}] and is configured as accessible from {nameof(LayerType)} [{LayerType}] and {nameof(LayerTypeName)} [{LayerTypeName}]");

            throw NewRoutingException($"Found layer that matches the {nameof(ResourceName)} [{ResourceName}], {nameof(NextLayerType)} [{NextLayerType}] and {nameof(NextLayerTypeName)} [{NextLayerTypeName}] but it does not implement the specified interface [{typeof(TNext).FriendlyName()}]");
        }

      #endregion

      #region Entity Functions

        /// <summary>
        /// Returns the primary key value for <paramref name="entity"/>.
        /// </summary>
        protected override TKey EntityKey(TEntity entity)
        {
            if (NextLayer is IOcLayer<TRequest, TEntity, TKey> asNextEntityLayer)
                return asNextEntityLayer.EntityKey(entity);

            throw NewException($"Resource [{ResponseMessages}] by <{GetType().FriendlyName()}> does not implement {nameof(EntityKey)}({nameof(entity)}) and {nameof(NextLayerType)} does not implement {typeof(IOcLayer<TRequest, TEntity, TKey>).FriendlyName()}", entity);
        }

        /// <summary>
        /// Returns true if <paramref name="entity"/> is marked as inactive or deleted.
        /// For systems that hard delete data from the data store this can always return false.
        /// For systems that maintain deleted records by marking them deleted, override this to return if they are marked as deleted.
        /// </summary>
        protected override bool EntityIsInactive(TEntity entity)
        {
            if (NextLayer is IOcLayer<TRequest, TEntity, TKey> asNextEntityLayer)
                return asNextEntityLayer.EntityIsInactive(entity);

            throw NewException($"Resource [{ResponseMessages}] by <{GetType().FriendlyName()}> does not implement {nameof(EntityIsInactive)}({nameof(entity)}) and {nameof(NextLayerType)} does not implement {typeof(IOcLayer<TRequest, TEntity, TKey>).FriendlyName()}", entity);
        }

        /// <summary>
        /// Returns a short string that can be used to describe the entity key.
        /// </summary>
        protected override string EntityStatusSummary(TKey key)
        {
            if (NextLayer is IOcLayer<TRequest, TEntity, TKey> asNextEntityLayer)
                return asNextEntityLayer.EntityStatusSummary(key);

            return $"{typeof(TEntity).FriendlyName()} with key [{key}]";
        }

        /// <summary>
        /// Returns a short string that describes the entity as text.
        /// </summary>
        protected override string EntityStatusSummary(TEntity entity)
        {
            if (NextLayer is IOcLayer<TRequest, TEntity, TKey> asNextEntityLayer)
                return asNextEntityLayer.EntityStatusSummary(entity);

            return entity.GetBestString();
        }

      #endregion

      #region Properties

        /// <summary>
        /// Called to build / refresh the contents of <see cref="OcLayerBase{TRequest}.Properties"/>.
        /// Use <see cref="OcLayerBase{TRequest}.PropertySet"/> to add property values.
        /// </summary>
        protected override void PropertyPopulateAll()
        {
            base.PropertyPopulateAll();
            PropertySet("NextLayer", NextLayer);
            PropertySet("NextLayerType", NextLayerType);
            PropertySet("NextLayerTypeName", NextLayerTypeName);
        }

      #endregion

      #region Throw Exceptions

        protected virtual OcLayerException NewRoutingException(string message, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            var ex = new OcLayerRoutingException(message, this, entity, entityKey, callerName, callerFile, callerLine);
            // ReSharper restore ExplicitCallerInfoArgument
            Request.ResponseMessages.Add(OcApiMessageType.Error, $"New Routing Exception: {ex.Summary}");
            return ex;
        }

        protected virtual OcLayerException NewRoutingException(string message, [CanBeNull] Exception exception, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            var ex = new OcLayerRoutingException(message, exception, this, entity, entityKey, callerName, callerFile, callerLine);
            // ReSharper restore ExplicitCallerInfoArgument
            Request.ResponseMessages.Add(OcApiMessageType.Error, $"New Routing Exception: {ex.Summary}");
            return ex;
        }

      #endregion

      #region IDisposable

        /// <inheritdoc />
        protected override void DisposeManaged()
        {
            NextLayer?.Dispose();
            base.DisposeManaged();
        }

      #endregion
    }
}