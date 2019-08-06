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
using System.Threading;
using JetBrains.Annotations;
using OriginalCoder.Common;
using OriginalCoder.Common.Api;
using OriginalCoder.Common.Exceptions;
using OriginalCoder.Common.Extensions;
using OriginalCoder.Common.Interfaces;
using OriginalCoder.Common.Interfaces.Properties;
using OriginalCoder.Data.Interfaces;
using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Config.Factories;
using OriginalCoder.Layers.Exceptions;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Interfaces.Data;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Base
{
    /// <summary>
    /// Abstract base class that lays the foundation for classes that implement layers.
    /// </summary>
    [PublicAPI]
    public abstract class OcLayerBase<TRequest> : OcDisposableHierarchyBase, IOcLayer<TRequest>
        where TRequest : class, IOcRequest
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
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : this(configuration, layerType, null, resourceName, request)
        { }

        /// <summary>
        /// Preferred constructor for use with <see cref="OcLayerType.Other"/> and specifying a <paramref name="layerTypeName"/>.
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerTypeName"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : this(configuration, OcLayerType.Other, layerTypeName, resourceName, request)
        { }

        /// <summary>
        /// Alternative constructor for when <paramref name="layerType"/> is not <see cref="OcLayerType.Other"/>
        /// When possible it is better to use the other, preferred constructor and specify the <see cref="ResourceName"/> rather than attempting to determine it programatically.
        /// </summary>
        /// <param name="layerType"></param>
        /// <param name="configuration"></param>
        /// <param name="request"></param>
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, [NotNull] TRequest request)
            : this(configuration, layerType, null, null, request)
        { }

        /// <summary>
        /// Alternative constructor for use with <see cref="OcLayerType.Other"/> and specifying a <paramref name="layerTypeName"/>.
        /// When possible it is better to use the other, preferred constructor and specify the <see cref="ResourceName"/> rather than attempting to determine it programatically.
        /// </summary>
        /// <param name="layerTypeName"></param>
        /// <param name="configuration"></param>
        /// <param name="request"></param>
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, [NotNull] TRequest request)
            : this(configuration, OcLayerType.Other, layerTypeName, null, request)
        { }

        private OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string layerTypeName, string resourceName, [NotNull] TRequest request)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (layerType == OcLayerType.Unknown)
                throw new ArgumentOutOfRangeException(nameof(layerType), layerType, $"{nameof(layerType)} can not be {nameof(OcLayerType.Unknown)}");
            if (layerType == OcLayerType.Other && string.IsNullOrWhiteSpace(layerTypeName))
                throw new ArgumentOutOfRangeException(nameof(layerType), layerType, $"{nameof(layerTypeName)} must be specified for {nameof(OcLayerType.Other)}");
            if (layerType != OcLayerType.Other && layerTypeName != null)
                throw new ArgumentOutOfRangeException(nameof(layerType), layerType, $"{nameof(layerTypeName)} must be null unless {nameof(layerType)} is {nameof(OcLayerType.Other)}");
            if (request == null)
                throw new ArgumentNullException(nameof(request));          

            if (!configuration.IsAllowed(layerType, layerTypeName))
                throw new ArgumentOutOfRangeException(nameof(layerType), $"Layers of LayerType [{layerType}] and LayerTypeName [{layerTypeName}] are not allowed in this systems");

            InstanceId = Interlocked.Increment(ref _globalInstanceCounter);

            LayerType = layerType;
            LayerTypeName = LayerType == OcLayerType.Other ? LayerType.ToString() : layerTypeName?.Trim();
            Debug.Assert(!string.IsNullOrWhiteSpace(LayerTypeName));

            ResourceName = DetermineResourceName(resourceName, LayerTypeName, GetType());

            Request = request;

            Factory = configuration.RestrictedFactoryCrud(LayerType, LayerTypeName);
            Debug.Assert(Factory != null);

            Log?.WriteLine($"LAYER CREATE - InstanceID [{InstanceId}] Type [{GetType().FriendlyName()}] Resource [{ResourceName}] LayerTypeName [{LayerTypeName}]");
        }

      #endregion

        /// <inheritdoc />
        public long InstanceId { get; }

        // ReSharper disable once StaticMemberInGenericType
        private static long _globalInstanceCounter;

        /// <inheritdoc />
        public string Name => $"{ResourceName} {LayerTypeName}";

        /// <inheritdoc />
        public virtual string Description => $"{Name} for Request: {Request?.Name}";

        /// <inheritdoc />
        public virtual string StatusSummary => $"{Name} - Request [{Request?.StatusSummary}]";

      #region Factory

        internal IOcLayerRestrictedCrudFactory<TRequest> Factory { get; }

        /// <summary>
        /// If available returns the resource that is uniquely associated with <typeparamref name="TInterface"/>.
        /// Returns null if a suitable resource could not be found.
        /// </summary>
        /// <typeparam name="TInterface">Interface type that uniquely specifies the resource to obtain</typeparam>
        protected virtual TInterface GetResource<TInterface>() where TInterface : class
        {
            return Factory.GetResource<TInterface>(Request);
        }

        /// <summary>
        /// If available returns the specified interface to the layer that provides <paramref name="layerType"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <typeparam name="TLayer">The interface (which inherits from <see cref="IOcLayer{TRequest}"/> being sought.</typeparam>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerType">Type of layer to search for</param>
        protected virtual TLayer GetLayer<TLayer>(string resourceName, OcLayerType layerType) where TLayer : class, IOcLayer<TRequest>
        {
            return Factory.GetResourceLayer(resourceName, layerType, Request) as TLayer;
        }

        /// <summary>
        /// If available returns the specified interface to the layer that provides <paramref name="layerTypeName"/> services for <paramref name="resourceName"/>.
        /// Returns null if a suitable resource layer could not be found.
        /// </summary>
        /// <typeparam name="TLayer">The interface (which inherits from <see cref="IOcLayer{TRequest}"/> being sought.</typeparam>
        /// <param name="resourceName">Name of the resource that they layer provides access to</param>
        /// <param name="layerTypeName">Name of the layer type to search for (for use with layers that are <see cref="OcLayerType.Other"/>).</param>
        protected virtual TLayer GetLayer<TLayer>(string resourceName, string layerTypeName) where TLayer : class, IOcLayer<TRequest>
        {
            return Factory.GetResourceLayer(resourceName, layerTypeName, Request) as TLayer;
        }

      #endregion

      #region API Resource Information

        /// <inheritdoc />
        public string ResourceName { get; }

        /// <inheritdoc />
        public OcLayerType LayerType { get; }

        /// <inheritdoc />
        public string LayerTypeName { get; }

        [NotNull]
        protected static string DetermineResourceName(string resourceName, [NotNull] string layerTypeName, [NotNull] Type type, params string[] removeWords)
        {
            if (!string.IsNullOrWhiteSpace(resourceName))
                return resourceName.Trim();

            if (string.IsNullOrWhiteSpace(layerTypeName))
                throw new ArgumentNullException(nameof(layerTypeName));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var typeName = type.Name.ToUpper().Replace(layerTypeName.Trim(), "", true);

            if (removeWords == null)
                removeWords = new [] { "Adapter", "API", "Business", "Cache", "Controller", "DataAccess", "Domain", "Facade", "Layer", "Logic", "Persistence", "Remote", "Repository", "Service", "Storage" };
            Debug.Assert(removeWords != null);

            foreach (var word in removeWords)
                typeName = typeName.Replace(word, "", true);

            if (string.IsNullOrWhiteSpace(typeName))
                return type.Name.ToUpper();

            return typeName;
        }

      #endregion

      #region Request

        /// <inheritdoc />
        public TRequest Request { get; }

        public OcRequestOptions Options => Request?.Options ?? OcRequestOptions.Default;

      #endregion

      #region Response

        public IOcApiMessages ResponseMessages => Request.ResponseMessages;

        public bool HasErrors => ResponseMessages.HasErrors;

        /// <summary>
        /// Returns true if the <see cref="ResponseMessages"/> collection contains any messages with a <see cref="OcApiMessageType"/> specified in <paramref name="apiMessageTypes"/>.
        /// </summary>
        public bool ResponseMessagesHas(params OcApiMessageType[] apiMessageTypes) => ResponseMessages?.Has(apiMessageTypes) ?? false;

        protected void ResponseMessageAdd(OcApiMessageType type, [NotNull] string message)
        {
            Debug.Assert(Request?.ResponseMessages != null);
            Request.ResponseMessages.Add(type, message);
        }

        protected void ResponseMessageAdd(OcApiMessageType type, [NotNull] string message, string referenceType, string referenceKey)
        {
            Debug.Assert(Request?.ResponseMessages != null);
            Request.ResponseMessages.Add(type, message, referenceType, referenceKey);
        }

        public void ResponseMessagesAdd([CanBeNull] IEnumerable<IOcApiMessage> messages)
        {
            Debug.Assert(Request?.ResponseMessages != null);
            Request.ResponseMessages.Add(messages);
        }

      #endregion

      #region Properties

        /// <inheritdoc />
        public IReadOnlyDictionary<string, object> Properties
        {
            get
            {
                if (_properties == null)
                    PropertyPopulateAll();
                return _properties;
            }
        }
        private Dictionary<string, object> _properties;

        /// <summary>
        /// Called to build / refresh the contents of <see cref="Properties"/>.
        /// Use <see cref="PropertySet"/> to add property values.
        /// </summary>
        protected virtual void PropertyPopulateAll()
        {
            PropertySet("TRequestType", typeof(TRequest).FriendlyName());
            PropertySet("InstanceId", InstanceId);
            PropertySet("Name", Name);
            PropertySet("ResourceName", ResourceName);
            PropertySet("LayerType", LayerType);
            PropertySet("LayerTypeName", LayerTypeName);
            PropertySet("Request", Request);
            PropertySet("HasErrors", HasErrors);
            PropertySet("ResponseMessages", ResponseMessages);
        }

        protected void PropertySet(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            name = name.Trim();
            if (value == null && _properties == null)
                return;  // Don't need to remove if it doesn't exist

            if (_properties == null)
                _properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (value == null)
            {
                _properties.Remove(name);
                return;
            }

            _properties[name] = value;
        }

      #endregion

      #region Logging

        /// <summary>
        /// Optional logging interface
        /// </summary>
        protected IOcTextLog Log => Request.Log;

      #endregion

      #region Throw Exceptions

        protected OcLayerException NewConstructorException(string message, [CanBeNull] Exception exception, string resourceName, OcLayerType layerType, string layerTypeName, IOcRequest request = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            var ex = new OcLayerException($"Error constructing {GetType().FriendlyName()}" + (string.IsNullOrWhiteSpace(message) ? "" : message), exception, resourceName, layerType, layerTypeName, request, null, null, null, callerName, callerFile, callerLine);
            // ReSharper restore ExplicitCallerInfoArgument
            Request.ResponseMessages.Add(OcApiMessageType.Error, $"Layer Constructor Exception: {ex.Summary}");
            return ex;
        }

        protected virtual OcLayerException NewException(string message, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            var ex = new OcLayerException(message, this, entity, entityKey, callerName, callerFile, callerLine);
            // ReSharper restore ExplicitCallerInfoArgument
            Request.ResponseMessages.Add(OcApiMessageType.Error, $"New Exception: {ex.Summary}");
            return ex;
        }

        protected virtual OcLayerException NewException(string message, [CanBeNull] Exception exception, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            var ex = new OcLayerException(message, exception, this, entity, entityKey, callerName, callerFile, callerLine);
            // ReSharper restore ExplicitCallerInfoArgument
            Request.ResponseMessages.Add(OcApiMessageType.Error, $"New Exception: {ex.Summary}");
            return ex;
        }

      #endregion

      #region IDisposable

        /// <inheritdoc />
        protected override void DisposeManaged()
        {
            Log?.WriteLine("LAYER: DISPOSE - InstanceID:" + InstanceId + " - Type: " + GetType().FriendlyName() + " - ResourceName: " + ResourceName);
            Request.Dispose();
        }

      #endregion

        public override string ToString() => StatusSummary;
    }



    /// <summary>
    /// Abstract base class that lays the foundation for classes that implement
    /// layers which work with one specific <typeparamref name="TEntity"/> type.
    /// </summary>
    [PublicAPI]
    public abstract class OcLayerBase<TRequest, TEntity, TKey> : OcLayerBase<TRequest>
        where TRequest : class, IOcRequest
        where TEntity : class
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
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        { }

        /// <summary>
        /// Preferred constructor for use with <see cref="OcLayerType.Other"/> and specifying a <paramref name="layerTypeName"/>.
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerTypeName"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        { }

        /// <summary>
        /// Alternative constructor for when <paramref name="layerType"/> is not <see cref="OcLayerType.Other"/>
        /// When possible it is better to use the other, preferred constructor and specify the ResourceName rather than attempting to determine it programatically.
        /// </summary>
        /// <param name="layerType"></param>
        /// <param name="configuration"></param>
        /// <param name="request"></param>
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, [NotNull] TRequest request)
            : base(configuration, layerType, request)
        { }

        /// <summary>
        /// Alternative constructor for use with <see cref="OcLayerType.Other"/> and specifying a <paramref name="layerTypeName"/>.
        /// When possible it is better to use the other, preferred constructor and specify the ResourceName rather than attempting to determine it programatically.
        /// </summary>
        /// <param name="layerTypeName"></param>
        /// <param name="configuration"></param>
        /// <param name="request"></param>
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, request)
        { }

      #endregion

        /// <inheritdoc />
        public override string Description => $"{Name} for Request: {Request?.Name}";

        /// <inheritdoc />
        public override string StatusSummary => $"{Name} - Request [{Request?.StatusSummary}]";

      #region Entity Functions

        /// <summary>
        /// Returns the primary key value for <paramref name="entity"/>.
        /// </summary>
        protected abstract TKey EntityKey(TEntity entity);

        /// <summary>
        /// Returns true if <paramref name="entity"/> is marked as deleted.
        /// For systems that hard delete data from the data store this can always return null.
        /// For systems that maintain deleted records by marking them deleted, override this to return if they are marked as deleted.
        /// </summary>
        protected virtual bool EntityIsInactive(TEntity entity) => false;

        protected virtual string EntityStatusSummary(TKey key)
        {
            return $"{typeof(TEntity).Name} with key [{key}]";
        }

        protected virtual string EntityStatusSummary(TEntity entity)
        {
            if (entity is IStatusSummary aStatusSummary)
                return aStatusSummary.StatusSummary;
            return $"{typeof(TEntity).Name} with key [{EntityKey(entity)}]";
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
            PropertySet("EntityType", typeof(TEntity));
            PropertySet("EntityKeyType", typeof(TKey));
        }

      #endregion

      #region Validation Support

        /// <summary>
        /// Performs validation of <paramref name="entity"/>.
        /// If validation is not implemented null is returned.
        /// If validation is implemented and no issues are found true is returned.
        /// If validation issues are found false.
        /// If <paramref name="addResponseMessages"/> is true any messages generated by the
        /// validation process will be added to <see cref="OcLayerBase{TRequest}.ResponseMessages"/>.
        /// </summary>
        protected bool? IsValid(TEntity entity, bool addResponseMessages = true)
        {
            if (!(entity is IOcValidation validation))
                return null;

            var result = validation.IsValid();
            if (addResponseMessages)
                Request.ResponseMessages.Add(validation.ValidationMessages);
            return result;
        }

        /// <summary>
        /// Performs validation of <paramref name="entity"/>.
        /// If validation is not implemented null is returned.
        /// If validation is implemented and no issues are found true is returned.
        /// If validation issues are found false is returned.
        /// Any messages generated by the validation process will be added to <paramref name="messages"/>.
        /// </summary>
        protected bool? IsValid(TEntity entity, ref OcApiMessages messages)
        {
            if (!(entity is IOcValidation validation))
                return null;

            var result = validation.IsValid();
            if (validation.ValidationMessages?.Count > 0 && messages == null)
                messages = new OcApiMessages();
            messages?.Add(validation.ValidationMessages);
            return result;
        }

      #endregion
    
      #region Factory

        protected virtual IOcLayerRead<TRequest, TEntity, TKey> GetLayerRead(string resourceName, OcLayerType layerType, [NotNull] TRequest request, [NotNull] IOcUnitOfWork unitOfWork)
        {
            var result = Factory.GetLayerRead<TEntity, TKey>(resourceName, layerType, Request);
            if (result != null)
                DisposeAdd(result);
            return result;
        }

        protected virtual IOcLayerRead<TRequest, TEntity, TKey> GetLayerRead(string resourceName, string layerTypeName, [NotNull] TRequest request, [NotNull] IOcUnitOfWork unitOfWork)
        {
            var result = Factory.GetLayerRead<TEntity, TKey>(resourceName, layerTypeName, Request);
            if (result != null)
                DisposeAdd(result);
            return result;
        }

        protected virtual IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch> GetLayerReadSearch<TSearch>(string resourceName, OcLayerType layerType, [NotNull] TRequest request, [NotNull] IOcUnitOfWork unitOfWork) where TSearch : class
        {
            var result = Factory.GetLayerReadSearch<TEntity, TKey, TSearch>(resourceName, layerType, Request);
            if (result != null)
                DisposeAdd(result);
            return result;
        }

        protected virtual IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch> GetLayerReadSearch<TSearch>(string resourceName, string layerTypeName, [NotNull] TRequest request, [NotNull] IOcUnitOfWork unitOfWork) where TSearch : class
        {
            var result = Factory.GetLayerReadSearch<TEntity, TKey, TSearch>(resourceName, layerTypeName, Request);
            if (result != null)
                DisposeAdd(result);
            return result;
        }

        protected virtual IOcLayerCrud<TRequest, TEntity, TKey> GetLayerCrud(string resourceName, OcLayerType layerType, [NotNull] TRequest request, [NotNull] IOcUnitOfWork unitOfWork)
        {
            var result = Factory.GetLayerCrud<TEntity, TKey>(resourceName, layerType, Request);
            if (result != null)
                DisposeAdd(result);
            return result;
        }

        protected virtual IOcLayerCrud<TRequest, TEntity, TKey> GetLayerCrud(string resourceName, string layerTypeName, [NotNull] TRequest request, [NotNull] IOcUnitOfWork unitOfWork)
        {
            var result = Factory.GetLayerCrud<TEntity, TKey>(resourceName, layerTypeName, Request);
            if (result != null)
                DisposeAdd(result);
            return result;
        }

        protected virtual IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch> GetLayerCrudSearch<TSearch>(string resourceName, OcLayerType layerType, [NotNull] TRequest request, [NotNull] IOcUnitOfWork unitOfWork) where TSearch : class
        {
            var result = Factory.GetLayerCrudSearch<TEntity, TKey, TSearch>(resourceName, layerType, Request);
            if (result != null)
                DisposeAdd(result);
            return result;
        }

        protected virtual IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch> GetLayerCrudSearch<TSearch>(string resourceName, string layerTypeName, [NotNull] TRequest request, [NotNull] IOcUnitOfWork unitOfWork) where TSearch : class
        {
            var result = Factory.GetLayerCrudSearch<TEntity, TKey, TSearch>(resourceName, layerTypeName, Request);
            if (result != null)
                DisposeAdd(result);
            return result;
        }

        #endregion
    }



    /// <summary>
    /// Abstract class that provides foundation for implementing a standardized business layer.
    /// </summary>
    [PublicAPI]
    public abstract class OcLayerBase<TRequest, TEntity, TKey, TNext> : OcLayerBase<TRequest, TEntity, TKey> // , IOcLayerUnitOfWork<TRequest>
        where TRequest : class, IOcRequest
        where TEntity : class
        where TNext : class, IOcLayer<TRequest>
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
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        {
            // For use from the bottom layer in the hierarchy (no further chaining)
            NextLayerType = OcLayerType.Unknown;
        }

        /// <summary>
        /// Preferred constructor for use with <see cref="OcLayerType.Other"/> and specifying a <paramref name="layerTypeName"/>.
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerTypeName"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        {
            // For use from the bottom layer in the hierarchy (no further chaining)
            NextLayerType = OcLayerType.Unknown;
        }

        /// <summary>
        /// Preferred constructor when <paramref name="layerType"/> is not <see cref="OcLayerType.Other"/>
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerType"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        /// <param name="nextLayer"></param>
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerType, resourceName, request)
        {
            NextLayerAssign(nextLayer);
        }

        /// <summary>
        /// Preferred constructor for use with <see cref="OcLayerType.Other"/> and specifying a <paramref name="layerTypeName"/>.
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerTypeName"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        /// <param name="nextLayer"></param>
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, TNext nextLayer)
            : base(configuration, layerTypeName, resourceName, request)
        {
            NextLayerAssign(nextLayer);
        }

        /// <summary>
        /// Preferred constructor when <paramref name="layerType"/> is not <see cref="OcLayerType.Other"/>
        /// It is better to have <paramref name="resourceName"/> specified manually than the alternative constructor that attempts to determine it programatically.
        /// </summary>
        /// <param name="layerType"></param>
        /// <param name="configuration"></param>
        /// <param name="resourceName"></param>
        /// <param name="request"></param>
        /// <param name="nextLayerType"></param>
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType)
            : base(configuration, layerType, resourceName, request)
        {
            NextLayerType = nextLayerType;
            NextLayerTypeName = nextLayerType.ToString();
        }

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
        protected OcLayerBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request, OcLayerType nextLayerType, string nextLayerTypeName)
            : base(configuration, layerTypeName, resourceName, request)
        {
            NextLayerType = nextLayerType;
            NextLayerTypeName = nextLayerTypeName;
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

      #region Layering

        public OcLayerType NextLayerType { get; private set; }
        public string NextLayerTypeName { get; private set; }

        /// <summary>
        /// Next Layer that operations chain to from here for processing.
        /// </summary>
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