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
using System.Linq;
using JetBrains.Annotations;
using OriginalCoder.Common.Exceptions;
using OriginalCoder.Common.Threading.Containers;
using OriginalCoder.Data.Mapper;
using OriginalCoder.Layers.Config.Factories;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Interfaces.Data;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Config
{
    [PublicAPI]
    public class OcLayerConfiguration<TRequest> : IOcLayerConfiguration<TRequest>
        where TRequest : class, IOcRequest
    {
      #region Constructors

        public OcLayerConfiguration()
        { }

        public OcLayerConfiguration(IOcDataMapper dataMapper)
        {
            if (dataMapper == null)
                throw new ArgumentNullException(nameof(dataMapper));
            DataMapper = dataMapper;
        }

        public OcLayerConfiguration(IOcAuthorizationService<TRequest> authorizationService)
        {
            if (authorizationService == null)
                throw new ArgumentNullException(nameof(authorizationService));
            AuthorizationService = authorizationService;
        }

        public OcLayerConfiguration(IOcAuthorizationService<TRequest> authorizationService, IOcDataMapper dataMapper)
        {
            if (dataMapper == null)
                throw new ArgumentNullException(nameof(dataMapper));
            DataMapper = dataMapper;
            if (authorizationService == null)
                throw new ArgumentNullException(nameof(authorizationService));
            AuthorizationService = authorizationService;
        }

      #endregion

      #region Services

        /// <inheritdoc />
        public IOcDataMapper DataMapper { get; }

        /// <inheritdoc />
        public IOcAuthorizationService<TRequest> AuthorizationService { get; }

      #endregion

      #region Allowed Layer Types & Layer Type Names

        /// <summary>
        /// Specifies the set of <see cref="OcLayerType"/> which are allowed in the system.  If both this and <see cref="_layerTypeNames"/> are empty then any layer type is allowed.
        /// </summary>
        private readonly OcThreadSafeHashSet<OcLayerType> _layerTypes = new OcThreadSafeHashSet<OcLayerType>();

        /// <summary>
        /// Specifies the set of Layer Type Names which are allowed in the system.  If both this and <see cref="_layerTypeNames"/> are empty then any layer type is allowed.
        /// </summary>
        private readonly OcThreadSafeHashSet<string> _layerTypeNames = new OcThreadSafeHashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Returns true if <paramref name="layerType"/> is allowed in the system.
        /// </summary>
        public bool IsAllowed(OcLayerType layerType)
        {
            if (layerType == OcLayerType.Unknown)
                return false;
            if (layerType == OcLayerType.Other)
                return _layerTypeNames.Count > 0;

            using (var readLock = _layerTypes.ReadLock())
            {
                readLock.Lock();

                // If no layer types have been configured as allowed, then allow all layer types.
                return _layerTypes.Count == 0 || _layerTypes.Contains(layerType);
            }
        }

        /// <summary>
        /// Returns true if <paramref name="layerTypeName"/> is allowed in the system.
        /// Note that this only works for non-standard layer type names.
        /// For cases where Layer Type is not <see cref="OcLayerType.Other"/> use one of the other overloads instead.
        /// </summary>
        public bool IsAllowed(string layerTypeName)
        {
            if (string.IsNullOrWhiteSpace(layerTypeName))
                throw new ArgumentNullException(nameof(layerTypeName));

            using (var readLock = _layerTypes.ReadLock())
            {
                readLock.Lock();

                // If no layer types or layer type names have been configured as allowed, then allow all layer types.
                return (_layerTypes.Count == 0 && _layerTypeNames.Count == 0) || _layerTypeNames.Contains(layerTypeName);
            }
        }

        /// <summary>
        /// Returns true if the specified layer type is allowed in the system.
        /// Note that if <see cref="OcLayerType.Other"/> is specified then <paramref name="layerTypeName"/> must have a value.
        /// </summary>
        public bool IsAllowed(OcLayerType layerType, string layerTypeName)
        {
            return layerType == OcLayerType.Other ? IsAllowed(layerTypeName) : IsAllowed(layerType);
        }

      #endregion

      #region Allowed Layer Type Interactions

        /// <summary>
        /// Dictionary that specifies how layers are allowed to interact with each other.
        /// Key is the Layer Type Name of the layer requesting an interaction (the "from" layer).
        /// Value is a HashSet that contains the Layer Type Names (the "to" layers) which can be accessed from the requesting layer.
        /// For Layer Types other than <see cref="OcLayerType.Other"/> the Layer Type Name is the ToString() of the Layer Type enumeration value.
        /// </summary>
        private readonly OcThreadSafeDictionary<string, HashSet<string>> _layerNameInteractions = new OcThreadSafeDictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Returns true if <paramref name="fromLayerType"/> is allowed to access <paramref name="toLayerType"/>.
        /// Note that if no layer interaction restrictions have been configured then any valid layer is allowed to interact with any other valid layer.
        /// </summary>
        public bool IsAllowed(OcLayerType fromLayerType, OcLayerType toLayerType)
        {
            return IsAllowed(fromLayerType, null, toLayerType, null);
        }

        /// <summary>
        /// Returns true if the From Layer Type is allowed to access the To Layer Type.
        /// Note that if no layer interaction restrictions have been configured then any valid layer is allowed to interact with any other valid layer.
        /// </summary>
        public bool IsAllowed(OcLayerType fromLayerType, string fromLayerTypeName, OcLayerType toLayerType, string toLayerTypeName)
        {
            if (fromLayerType == OcLayerType.Unknown || toLayerType == OcLayerType.Unknown)
                return false;

            if (fromLayerType == OcLayerType.Other && string.IsNullOrWhiteSpace(fromLayerTypeName))
                throw new ArgumentNullException(nameof(fromLayerTypeName));
            if (toLayerType == OcLayerType.Other && string.IsNullOrWhiteSpace(toLayerTypeName))
                throw new ArgumentNullException(nameof(toLayerTypeName));

            if (fromLayerType != OcLayerType.Other)
                fromLayerTypeName = fromLayerType.ToString();
            if (toLayerType != OcLayerType.Other)
                toLayerTypeName = toLayerType.ToString();


            using (var readLock = _layerNameInteractions.ReadLock())
            {
                readLock.Lock();

                // If restrictions have been configured use them to answer the question
                if (_layerNameInteractions.TryGetValue(fromLayerTypeName, out var allowed))
                {
                    return allowed?.Contains(toLayerTypeName) ?? false;
                }

                // If restrictions are not configured, assume any type of interaction is allowed (assuming the from layer type is allowed by the system).
                return IsAllowed(fromLayerType, fromLayerTypeName) && IsAllowed(toLayerType, toLayerTypeName);
            }
        }

      #endregion

      #region Registered Non-Layer Resources

        /// <summary>
        /// Dictionary containing all of the non-layer Interfaces that have been registered as obtainable resources.
        /// The factory methods are of type Func{TInterface} OR Func{TRequest, TInterface} depending on if they are registered to support request and unit of work or not.
        /// </summary>
        private readonly OcThreadSafeDictionary<Type, (HashSet<string> allowedLayerNames, object factoryMethod)> _interfaceFactories = new OcThreadSafeDictionary<Type, (HashSet<string> allowedLayerNames, object factoryMethod)>();

        internal TInterface GetResource<TInterface>(OcLayerType forLayerType)
            where TInterface : class
        {
            return GetResource<TInterface>(forLayerType.ToString());
        }

        internal TInterface GetResource<TInterface>(string forLayerTypeName)
            where TInterface : class
        {
            if (string.IsNullOrWhiteSpace(forLayerTypeName))
                throw new ArgumentNullException(nameof(forLayerTypeName));
            if (string.Equals(forLayerTypeName, OcLayerType.Unknown.ToString(), StringComparison.OrdinalIgnoreCase) || string.Equals(forLayerTypeName, OcLayerType.Other.ToString(), StringComparison.OrdinalIgnoreCase))
                return null;

            var interfaceType = typeof(TInterface);
            if (!_interfaceFactories.TryGetValue(interfaceType, out var factory))
                return null;  // Not found

            if (factory.allowedLayerNames?.Count > 0 && !factory.allowedLayerNames.Contains(forLayerTypeName))
                return null;  // Not allowed

            if (factory.factoryMethod is Func<TRequest, TInterface>)
                throw new OcLibraryException($"The requested interface resource [{typeof(TInterface).FullName}] requires a Request and Unit of Work and can not be obtained without them.");

            if (!(factory.factoryMethod is Func<TInterface> interfaceFactory))
                throw new OcLibraryException($"Registered factory method for Type [{typeof(TInterface).FullName}] is of the wrong type");

            return interfaceFactory();
        }

        internal TInterface GetResource<TInterface>(OcLayerType forLayerType, [NotNull] TRequest request)
            where TInterface : class
        {
            return GetResource<TInterface>(forLayerType.ToString(), request);
        }

        internal TInterface GetResource<TInterface>(string forLayerTypeName, [NotNull] TRequest request)
            where TInterface : class
        {
            if (string.IsNullOrWhiteSpace(forLayerTypeName))
                throw new ArgumentNullException(nameof(forLayerTypeName));
            if (string.Equals(forLayerTypeName, OcLayerType.Unknown.ToString(), StringComparison.OrdinalIgnoreCase) || string.Equals(forLayerTypeName, OcLayerType.Other.ToString(), StringComparison.OrdinalIgnoreCase))
                return null;

            var interfaceType = typeof(TInterface);
            if (!_interfaceFactories.TryGetValue(interfaceType, out var factory))
                return null;  // Not found

            if (factory.allowedLayerNames?.Count > 0 && !factory.allowedLayerNames.Contains(forLayerTypeName))
                return null;  // Not allowed

            if (factory.factoryMethod is Func<TRequest, TInterface> asRequestWorkFactory)
                return asRequestWorkFactory(request);

            if (!(factory.factoryMethod is Func<TInterface> interfaceFactory))
                throw new OcLibraryException($"Registered factory method for Type [{typeof(TInterface).FullName}] is of the wrong type");

            return interfaceFactory();
        }

      #endregion

      #region Registered Named Resource Layers

        /// <summary>
        /// Dictionary containing all registered layer resources that can be obtained.
        /// The factory methods are of type Func{TRequest, IOcLayer{TRequest}}
        /// </summary>
        private readonly OcThreadSafeDictionary<(string resourceName, string layerName), Func<TRequest, IOcLayer<TRequest>>> _layerFactories = new OcThreadSafeDictionary<(string resourceName, string layerName), Func<TRequest, IOcLayer<TRequest>>>();

      #endregion

      #region IOcLayerResourceFactoryFactory<TRequest>

        /// <inheritdoc />
        public IOcLayerFactory<TRequest> Factory() => this;
        
        /// <inheritdoc />
        public IOcLayerCrudFactory<TRequest> FactoryCrud() => this;

        /// <inheritdoc />
        public IOcLayerRestrictedFactory<TRequest> RestrictedFactory(OcLayerType layerType)
        {
            return new OcLayerRestrictedCrudFactory<TRequest>(layerType, this);
        }

        /// <inheritdoc />
        public IOcLayerRestrictedFactory<TRequest> RestrictedFactory(string layerTypeName)
        {
            return new OcLayerRestrictedCrudFactory<TRequest>(layerTypeName, this);
        }

        /// <inheritdoc />
        public IOcLayerRestrictedCrudFactory<TRequest> RestrictedFactoryCrud(OcLayerType layerType)
        {
            return new OcLayerRestrictedCrudFactory<TRequest>(layerType, this);
        }

        /// <inheritdoc />
        public IOcLayerRestrictedCrudFactory<TRequest> RestrictedFactoryCrud(string layerTypeName)
        {
            return new OcLayerRestrictedCrudFactory<TRequest>(layerTypeName, this);
        }

        /// <inheritdoc />
        public IOcLayerRestrictedCrudFactory<TRequest> RestrictedFactoryCrud(OcLayerType layerType, string layerTypeName)
        {
            if (layerType == OcLayerType.Other)
                return RestrictedFactoryCrud(layerTypeName);
            return RestrictedFactoryCrud(layerType);
        }

      #endregion

      #region IOcLayerResourceFactory<TRequest>

        /// <inheritdoc />
        public TInterface GetResource<TInterface>()
            where TInterface : class
        {
            var interfaceType = typeof(TInterface);
            if (!_interfaceFactories.TryGetValue(interfaceType, out var factory))
                return null;  // Not found

            if (factory.factoryMethod is Func<TRequest, TInterface>)
                throw new OcLibraryException($"The requested interface resource [{typeof(TInterface).FullName}] requires a Request and Unit of Work and can not be obtained without them.");

            if (!(factory.factoryMethod is Func<TInterface> interfaceFactory))
                throw new OcLibraryException($"Registered factory method for Type [{typeof(TInterface).FullName}] is of the wrong type");

            return interfaceFactory();
        }

        /// <inheritdoc />
        public TInterface GetResource<TInterface>([NotNull] TRequest request) where TInterface : class
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var interfaceType = typeof(TInterface);
            if (!_interfaceFactories.TryGetValue(interfaceType, out var factory))
                return null;  // Not found

            if (factory.factoryMethod is Func<TRequest, TInterface> asRequestWorkFactory)
                return asRequestWorkFactory(request);

            if (!(factory.factoryMethod is Func<TInterface> interfaceFactory))
                throw new OcLibraryException($"Registered factory method for Type [{typeof(TInterface).FullName}] is of the wrong type");

            return interfaceFactory();
        }

        /// <inheritdoc />
        public IOcLayer<TRequest> GetResourceLayer([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var key = ResourceLayerKey(resourceName, layerType);

            if (!_layerFactories.TryGetValue(key, out var factory))
                return null;

            if (factory == null)
                throw new OcLibraryException($"Registered factory Resource [{resourceName}] LayerType [{layerType}] does not have a factory method assigned");

            return factory(request);
        }

        /// <inheritdoc />
        public TLayer GetResourceLayer<TLayer>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TLayer : class, IOcLayer<TRequest>
            => GetResourceLayer(resourceName, layerType, request) as TLayer;

        /// <inheritdoc />
        public IOcLayer<TRequest> GetResourceLayer([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var key = ResourceLayerKey(resourceName, layerTypeName);

            if (!_layerFactories.TryGetValue(key, out var factory))
                return null;

            if (factory == null)
                throw new OcLibraryException($"Registered factory Resource [{resourceName}] LayerTypeName [{layerTypeName}] does not have a factory method assigned");

            return factory(request);
        }

        /// <inheritdoc />
        public TLayer GetResourceLayer<TLayer>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TLayer : class, IOcLayer<TRequest>
            => GetResourceLayer(resourceName, layerTypeName, request) as TLayer;

      #endregion

      #region IOcLayerResourceCrudFactory<TRequest>

        /// <inheritdoc />
        public IOcLayerRead<TRequest, TEntity, TKey> GetLayerRead<TEntity, TKey>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TEntity : class
            => GetResourceLayer(resourceName, layerType, request) as IOcLayerRead<TRequest, TEntity, TKey>;

        /// <inheritdoc />
        public IOcLayerRead<TRequest, TEntity, TKey> GetLayerRead<TEntity, TKey>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class
            => GetResourceLayer(resourceName, layerTypeName, request) as IOcLayerRead<TRequest, TEntity, TKey>;

        /// <inheritdoc />
        public IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch> GetLayerReadSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TEntity : class where TSearch : class
            => GetResourceLayer(resourceName, layerType, request) as IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch>;

        /// <inheritdoc />
        public IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch> GetLayerReadSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class where TSearch : class
            => GetResourceLayer(resourceName, layerTypeName, request) as IOcLayerReadSearch<TRequest, TEntity, TKey, TSearch>;

        /// <inheritdoc />
        public IOcLayerCrud<TRequest, TEntity, TKey> GetLayerCrud<TEntity, TKey>([NotNull] string resourceName, OcLayerType layerType, [NotNull]  TRequest request) where TEntity : class
            => GetResourceLayer(resourceName, layerType, request) as IOcLayerCrud<TRequest, TEntity, TKey>;

        /// <inheritdoc />
        public IOcLayerCrud<TRequest, TEntity, TKey> GetLayerCrud<TEntity, TKey>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class
            => GetResourceLayer(resourceName, layerTypeName, request) as IOcLayerCrud<TRequest, TEntity, TKey>;

        /// <inheritdoc />
        public IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch> GetLayerCrudSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, OcLayerType layerType, [NotNull] TRequest request) where TEntity : class where TSearch : class
            => GetResourceLayer(resourceName, layerType, request) as IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch>;

        /// <inheritdoc />
        public IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch> GetLayerCrudSearch<TEntity, TKey, TSearch>([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] TRequest request) where TEntity : class where TSearch : class
            => GetResourceLayer(resourceName, layerTypeName, request) as IOcLayerCrudSearch<TRequest, TEntity, TKey, TSearch>;

      #endregion

      #region Configure allowed layer types

        /// <summary>
        /// Configure the layer types that are allowed by the system.
        /// </summary>
        public void ConfigureAllowedLayerTypes([NotNull] params OcLayerType[] allowedLayerTypes)
        {
            if (allowedLayerTypes == null || allowedLayerTypes.Length == 0)
                throw new ArgumentException(nameof(allowedLayerTypes));

            using (var writeLock = _layerTypes.WriteLock())
            {
                writeLock.Lock();

                foreach (var layerType in allowedLayerTypes)
                    if (!_layerTypes.Contains(layerType))
                        _layerTypes.Add(layerType);
            }
        }

        /// <summary>
        /// Configure the layer types and layer type names that are allowed by the system.
        /// </summary>
        public void ConfigureAllowedLayerTypes(IEnumerable<OcLayerType> allowedLayerTypes, IEnumerable<string> allowedLayerTypeNames)
        {
            if (allowedLayerTypes != null)
            {
                using (var writeLock = _layerTypes.WriteLock())
                {
                    writeLock.Lock();
                    foreach (var layerType in allowedLayerTypes.Where(lt => lt != OcLayerType.Unknown && lt != OcLayerType.Other))
                        if (!_layerTypes.Contains(layerType))
                            _layerTypes.Add(layerType);
                }
            }

            if (allowedLayerTypeNames != null)
            {
                using (var writeLock = _layerTypeNames.WriteLock())
                {
                    writeLock.Lock();
                    foreach (var layerTypeName in allowedLayerTypeNames.Where(ltn => !string.IsNullOrWhiteSpace(ltn)).Select(ltn => ltn.Trim()))
                        if (!_layerTypeNames.Contains(layerTypeName))
                            _layerTypeNames.Add(layerTypeName);
                }
            }
        }

      #endregion

      #region Configure Restrictions for allowed layer interaction

        /// <summary>
        /// Restrict the layer types which can be accessed from <paramref name="layerType"/>.
        /// </summary>
        public void ConfigureLayerRestrictions(OcLayerType layerType, [NotNull] params OcLayerType[] allowAccessToLayerTypes)
        {
            if (layerType == OcLayerType.Unknown || layerType == OcLayerType.Other)
                throw new ArgumentOutOfRangeException(nameof(layerType));
            ConfigureLayerRestrictions(layerType.ToString(), allowAccessToLayerTypes, null);
        }

        /// <summary>
        /// Restrict the layer types and layer type names which can be accessed from <paramref name="layerType"/>.
        /// </summary>
        public void ConfigureLayerRestrictions(OcLayerType layerType, IEnumerable<OcLayerType> allowAccessToLayerTypes, IEnumerable<string> allowAccessToLayerTypeNames)
        {
            if (layerType == OcLayerType.Unknown || layerType == OcLayerType.Other)
                throw new ArgumentOutOfRangeException(nameof(layerType));
            ConfigureLayerRestrictions(layerType.ToString(), allowAccessToLayerTypes, allowAccessToLayerTypeNames);
        }

        /// <summary>
        /// Restrict the layer types and layer type names which can be accessed from <paramref name="layerTypeName"/>.
        /// </summary>
        public void ConfigureLayerRestrictions([NotNull] string layerTypeName, IEnumerable<OcLayerType> allowAccessToLayerTypes, IEnumerable<string> allowAccessToLayerTypeNames)
        {
            if (string.IsNullOrWhiteSpace(layerTypeName))
                throw new ArgumentNullException(nameof(layerTypeName));
            if (string.Equals(layerTypeName, OcLayerType.Unknown.ToString(), StringComparison.OrdinalIgnoreCase) || string.Equals(layerTypeName, OcLayerType.Other.ToString(), StringComparison.OrdinalIgnoreCase))
                throw new ArgumentOutOfRangeException(nameof(layerTypeName));

            using (var writeLock = _layerNameInteractions.WriteLock())
            {
                writeLock.Lock();

                layerTypeName = layerTypeName.Trim();
                if (!_layerNameInteractions.TryGetValue(layerTypeName, out var allowed))
                {
                    allowed = new HashSet<string>();
                    _layerNameInteractions.Add(layerTypeName, allowed);
                }

                Debug.Assert(allowed != null);

                if (allowAccessToLayerTypes != null)
                {
                    foreach (var allowedType in allowAccessToLayerTypes.Where(lt => lt != OcLayerType.Unknown && lt != OcLayerType.Other))
                    {
                        var allowedName = allowedType.ToString();
                        if (!allowed.Contains(allowedName))
                            allowed.Add(allowedName);
                    }
                }

                if (allowAccessToLayerTypeNames != null)
                {
                    foreach (var allowedName in allowAccessToLayerTypeNames.Where(ltn => !string.IsNullOrWhiteSpace(ltn)).Select(ltn => ltn.Trim()))
                    {
                        if (string.Equals(allowedName, OcLayerType.Unknown.ToString(), StringComparison.OrdinalIgnoreCase) || string.Equals(allowedName, OcLayerType.Other.ToString(), StringComparison.OrdinalIgnoreCase))
                            continue;
                        if (!allowed.Contains(allowedName))
                            allowed.Add(allowedName);
                    }
                }
            }
        }

      #endregion

      #region Register interface types

        /// <summary>
        /// Register an interface that can be obtained from any layer type.
        /// The provided <paramref name="factoryMethod"/> is used to obtain or construct the interface instance when needed.
        /// </summary>
        public void Register<TInterface>([NotNull] Func<TInterface> factoryMethod)
        {
            var interfaceType = typeof(TInterface);
            using (var writeLock = _interfaceFactories.WriteLock())
            {
                writeLock.Lock();
                if (_interfaceFactories.TryGetValue(interfaceType, out _))
                {
                    // TODO Log replacement warning 
                }
                _interfaceFactories[interfaceType] = (null, factoryMethod);
            }
        }

        /// <summary>
        /// Register an interface that can only be obtained from layer types specified in <paramref name="allowForLayerTypes"/>.
        /// The provided <paramref name="factoryMethod"/> is used to obtain or construct the interface instance when needed.
        /// </summary>
        public void Register<TInterface>([NotNull] Func<TInterface> factoryMethod, [NotNull] params OcLayerType[] allowForLayerTypes)
        {
            var interfaceType = typeof(TInterface);
            using (var writeLock = _interfaceFactories.WriteLock())
            {
                writeLock.Lock();
                if (_interfaceFactories.TryGetValue(interfaceType, out _))
                {
                    // TODO Log replacement warning 
                }
                _interfaceFactories[interfaceType] = (LayerTypesToHashSet(allowForLayerTypes), factoryMethod);
            }
        }

        /// <summary>
        /// Register an interface that can only be obtained from layer types specified in <paramref name="allowForLayerTypes"/> or <paramref name="allowForLayerTypeNames"/>.
        /// The provided <paramref name="factoryMethod"/> is used to obtain or construct the interface instance when needed.
        /// </summary>
        public void Register<TInterface>([NotNull] Func<TInterface> factoryMethod, IEnumerable<OcLayerType> allowForLayerTypes, IEnumerable<string> allowForLayerTypeNames)
        {
            var interfaceType = typeof(TInterface);

            using (var writeLock = _interfaceFactories.WriteLock())
            {
                writeLock.Lock();
                if (_interfaceFactories.TryGetValue(interfaceType, out _))
                {
                    // TODO Log replacement warning 
                }
                _interfaceFactories[interfaceType] = (LayerTypesToHashSet(allowForLayerTypes, allowForLayerTypeNames), factoryMethod);
            }
        }

      #endregion

      #region Register Named Resource Layers

        /// <summary>
        /// Register a specific <paramref name="layerType"/> for <paramref name="resourceName"/>.
        /// The provided <paramref name="factoryMethod"/> is used to obtain or construct the resource layer when needed.
        /// </summary>
        public void RegisterLayer([NotNull] string resourceName, OcLayerType layerType, [NotNull] Func<TRequest, IOcLayer<TRequest>> factoryMethod)
        {
            if (layerType == OcLayerType.Unknown || layerType == OcLayerType.Other)
                throw new ArgumentOutOfRangeException(nameof(layerType));

            RegisterLayer(resourceName, layerType.ToString(), factoryMethod);
        }

        /// <summary>
        /// Register a specific <paramref name="layerTypeName"/> for <paramref name="resourceName"/>.
        /// The provided <paramref name="factoryMethod"/> is used to obtain or construct the resource layer when needed.
        /// </summary>
        public void RegisterLayer([NotNull] string resourceName, [NotNull] string layerTypeName, [NotNull] Func<TRequest, IOcLayer<TRequest>> factoryMethod)
        {
            var key = ResourceLayerKey(resourceName, layerTypeName);
            using (var writeLock = _layerFactories.WriteLock())
            {
                writeLock.Lock();

                if (_layerFactories.ContainsKey(key))
                {
                    // TODO Log replacement warning 
                }
                _layerFactories[key] = factoryMethod;
            }
        }

      #endregion

      #region Support Methods

        private (string resourceName, string layerName) ResourceLayerKey([NotNull] string resourceName, OcLayerType layerType)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentNullException(resourceName);
            if (layerType == OcLayerType.Unknown || layerType == OcLayerType.Other)
                throw new ArgumentOutOfRangeException(nameof(layerType));

            return (resourceName.Trim().ToUpper(), layerType.ToString().ToUpper());
        }

        private (string resourceName, string layerName) ResourceLayerKey([NotNull] string resourceName, [NotNull] string layerTypeName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentNullException(resourceName);
            if (string.IsNullOrWhiteSpace(layerTypeName))
                throw new ArgumentNullException(nameof(layerTypeName));
            if (string.Equals(layerTypeName, OcLayerType.Unknown.ToString(), StringComparison.OrdinalIgnoreCase) || string.Equals(layerTypeName, OcLayerType.Other.ToString(), StringComparison.OrdinalIgnoreCase))
                throw new ArgumentOutOfRangeException(nameof(layerTypeName));

            return (resourceName.Trim().ToUpper(), layerTypeName.Trim().ToUpper());
        }

        private HashSet<string> LayerTypesToHashSet(IEnumerable<OcLayerType> layerTypes)
        {
            var result = new HashSet<string>();
            if (layerTypes != null)
            {
                foreach (var layerType in layerTypes.Where(lt => lt != OcLayerType.Unknown && lt != OcLayerType.Other))
                {
                    var layerName = layerType.ToString();
                    if (!result.Contains(layerName))
                        result.Add(layerName);
                }
            }
            return result.Count == 0 ? null : result;
        }

        private HashSet<string> LayerTypesToHashSet(IEnumerable<OcLayerType> layerTypes, IEnumerable<string> layerTypeNames)
        {
            var result = new HashSet<string>();
            if (layerTypes != null)
            {
                foreach (var layerType in layerTypes.Where(lt => lt != OcLayerType.Unknown && lt != OcLayerType.Other))
                {
                    var layerName = layerType.ToString();
                    if (!result.Contains(layerName))
                        result.Add(layerName);
                }
            }
            if (layerTypeNames != null)
            {
                foreach (var layerName in layerTypeNames.Where(ltn => !string.IsNullOrWhiteSpace(ltn)).Select(ltn => ltn.Trim()))
                {
                    if (string.Equals(layerName, OcLayerType.Unknown.ToString(), StringComparison.OrdinalIgnoreCase) || string.Equals(layerName, OcLayerType.Other.ToString(), StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (!result.Contains(layerName))
                        result.Add(layerName);
                }
            }
            return result.Count == 0 ? null : result;
        }

      #endregion
    }
}