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
using OriginalCoder.Common.Api;
using OriginalCoder.Common.Extensions;
using OriginalCoder.Layers.Base;
using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Logic;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Repositories
{
    [PublicAPI]
    public abstract class OcLayerRepositoryBase<TRequest, TEntity, TKey> : OcLayerLogicBase<TRequest, TEntity, TKey>, IOcLayer<TRequest, TEntity, TKey>
        where TRequest : class, IOcRequest
        where TEntity : class
    {
      #region Constructors

        protected OcLayerRepositoryBase([NotNull] IOcLayerConfiguration<TRequest> configuration, OcLayerType layerType, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerType, resourceName, request)
        { }

        protected OcLayerRepositoryBase([NotNull] IOcLayerConfiguration<TRequest> configuration, [NotNull] string layerTypeName, string resourceName, [NotNull] TRequest request)
            : base(configuration, layerTypeName, resourceName, request)
        { }

      #endregion

      #region Metadata

        /// <summary>
        /// Specifies the features implemented for this repository.
        /// </summary>
        public virtual OcRepositoryFeatures Features => OcRepositoryFeatures.None;

        /// <summary>
        /// Returns the complete list of columns for this resource that can be used for selecting a custom set of columns or specifying a custom order by.
        /// </summary>
        public virtual IReadOnlyCollection<string> ColumnList()
        {
            if (Features.HasFlag(OcRepositoryFeatures.ColumnList))
                ResponseMessageAdd(OcApiMessageType.WarningSystem, $"Column List is not implemented for [{ResourceName}] by <{GetType().FriendlyName()}> even though it is specified in {nameof(Features)}.");
            return null;
        }

      #endregion

      #region Entity Functions

        /// <summary>
        /// Returns the primary key value for <paramref name="entity"/>.
        /// </summary>
        protected abstract TKey GetEntityKey(TEntity entity);

        /// <summary>
        /// Returns true if <paramref name="entity"/> is marked as inactive or deleted.
        /// For systems that hard delete data from the data store this can always return false.
        /// For systems that maintain deleted records by marking them deleted, override this to return if they are marked as deleted.
        /// </summary>
        protected abstract bool GetEntityIsInactive(TEntity entity);

      #endregion

      #region LINQ queries

        /// <summary>
        /// Returns a queryable interface that selects all available columns of all records available in the data store.
        /// Should return records with minimal joins suitable for performing CRUD operations on.  Additional joins can
        /// be added for specific types of queries by overriding the Select*Columns methods.
        /// </summary>
        protected abstract IQueryable<TEntity> DataSource();

        /// <summary>
        /// Returns a queryable interface configured to perform a select query for the request.
        /// </summary>
        protected virtual IQueryable<TEntity> Query(IReadOnlyCollection<string> selectColumns = null)
        {
            var result = DataSource();

            if (selectColumns?.Count > 0)
                result = SelectCustomColumns(result, selectColumns);
            else if (Options.HasFlag(OcRequestOptions.SelectDetails))
                result = SelectDetailColumns(result);
            else if (Options.HasFlag(OcRequestOptions.SelectReporting))
                result = SelectDetailColumns(result);
            else if (Options.HasFlag(OcRequestOptions.SelectSummary))
                result = SelectDetailColumns(result);
            else if (Options.HasFlag(OcRequestOptions.SelectMinimal))
                result = SelectDetailColumns(result);

            if (Options.HasFlag(OcRequestOptions.ExcludeUnauthorized))
                result = ExcludeUnAuthorized(result);
            if (Options.HasFlag(OcRequestOptions.ExcludeInactive))
                result = ExcludeInactive(result);

            return result;
        }

      #endregion

      #region Specify the included columns (some of these may add additional joins)

        /// <summary>
        /// Configures <paramref name="query"/> to return the default set of columns for this resource (may potentially add joins).
        /// </summary>
        protected virtual IQueryable<TEntity> SelectDefaultColumns(IQueryable<TEntity> query)
        { return query; }

        /// <summary>
        /// Configures <paramref name="query"/> to return the maximal set of columns (will likely add joins).
        /// </summary>
        protected virtual IQueryable<TEntity> SelectDetailColumns(IQueryable<TEntity> query)
        {
            if (Features.HasFlag(OcRepositoryFeatures.SelectDetails))
                ResponseMessageAdd(OcApiMessageType.WarningSystem, $"Detail Column Select not implemented for [{ResourceName}] by <{GetType().FriendlyName()}> even though it is specified in {nameof(Features)}.");
            ResponseMessageAdd(OcApiMessageType.NotificationDev, $"Selecting DEFAULT COLUMNS because Detail select is not implemented for Resource [{ResourceName}]");
            return SelectDefaultColumns(query);
        }

        /// <summary>
        /// Configures <paramref name="query"/> to return a set of columns suitable for reporting (may potentially add joins).
        /// </summary>
        protected virtual IQueryable<TEntity> SelectReportingColumns(IQueryable<TEntity> query)
        {
            if (Features.HasFlag(OcRepositoryFeatures.SelectReporting))
                ResponseMessageAdd(OcApiMessageType.WarningSystem, $"Reporting Select not implemented for [{ResourceName}] by <{GetType().FriendlyName()}> even though it is specified in {nameof(Features)}.");
            ResponseMessageAdd(OcApiMessageType.NotificationDev, $"Selecting DEFAULT COLUMNS because Reporting select is not implemented for Resource [{ResourceName}]");
            return SelectDefaultColumns(query);
        }

        /// <summary>
        /// Configures <paramref name="query"/> to return a set of columns that provides summary level data of the records (may potentially add joins).
        /// </summary>
        protected virtual IQueryable<TEntity> SelectSummaryColumns(IQueryable<TEntity> query)
        {
            if (Features.HasFlag(OcRepositoryFeatures.SelectSummary))
                ResponseMessageAdd(OcApiMessageType.WarningSystem, $"Summary Column Select not implemented for [{ResourceName}] by <{GetType().FriendlyName()}> even though it is specified in {nameof(Features)}.");
            ResponseMessageAdd(OcApiMessageType.NotificationDev, $"Selecting DEFAULT COLUMNS because Summary select is not implemented for Resource [{ResourceName}]");
            return SelectDefaultColumns(query);
        }

        /// <summary>
        /// Configures <paramref name="query"/> to return the minimal useful set of columns (unlikely to add add joins).
        /// </summary>
        protected virtual IQueryable<TEntity> SelectMinimalColumns(IQueryable<TEntity> query)
        {
            if (Features.HasFlag(OcRepositoryFeatures.SelectMinimal))
                ResponseMessageAdd(OcApiMessageType.WarningSystem, $"Minimal Column Select not implemented for [{ResourceName}] by <{GetType().FriendlyName()}> even though it is specified in {nameof(Features)}.");
            ResponseMessageAdd(OcApiMessageType.NotificationDev, $"Selecting DEFAULT COLUMNS because Minimal select is not implemented for Resource [{ResourceName}]");
            return SelectDefaultColumns(query);
        }

        /// <summary>
        /// Configures <paramref name="query"/> to return a custom set of columns based on <paramref name="selectColumns"/>.
        /// </summary>
        protected virtual IQueryable<TEntity> SelectCustomColumns(IQueryable<TEntity> query, IReadOnlyCollection<string> selectColumns)
        {
            if (Features.HasFlag(OcRepositoryFeatures.SelectCustom))
                ResponseMessageAdd(OcApiMessageType.WarningSystem, $"Custom Column Select not implemented for [{ResourceName}] by <{GetType().FriendlyName()}> even though it is specified in {nameof(Features)}.");
            ResponseMessageAdd(OcApiMessageType.NotificationDev, $"Selecting DETAIL COLUMNS because Custom select is not implemented for Resource [{ResourceName}]");
            return SelectDetailColumns(query);
        }

      #endregion

      #region Specify Ordering and/or Paging of results

        /// <summary>
        /// Causes the records in <paramref name="query"/> to get sorted by the default ordering criteria for this resource.
        /// </summary>
        protected abstract IQueryable<TEntity> OrderByDefault(IQueryable<TEntity> query);

        /// <summary>
        /// Causes the records in <paramref name="query"/> to get sorted by the ordering criteria in <paramref name="orderBy"/>.
        /// </summary>
        protected virtual IQueryable<TEntity> OrderBy(IQueryable<TEntity> query, IReadOnlyList<(string columnName, bool ascending)> orderBy)
        {
            if (Features.HasFlag(OcRepositoryFeatures.CustomOrderBy))
                ResponseMessageAdd(OcApiMessageType.WarningSystem, $"Custom Order By not implemented for [{ResourceName}] by <{GetType().FriendlyName()}> even though it is specified in {nameof(Features)}.");
            ResponseMessageAdd(OcApiMessageType.WarningDev, $"Using DEFAULT ORDER BY because custom OrderBy not implemented for Resource [{ResourceName}]");
            return OrderByDefault(query);
        }

        /// <summary>
        /// Causes the records in <paramref name="query"/> to get sorted and then limits the results to a certain subset of records for paging.
        /// If <paramref name="orderBy"/> does not specify ordering criteria then <see cref="OrderByDefault"/> will be used.
        /// </summary>
        protected virtual IQueryable<TEntity> Paged(IQueryable<TEntity> query, int qtySkip, int qtyReturn, IReadOnlyList<(string columnName, bool ascending)> orderBy = null)
        {
            if (qtySkip < 0)
            {
                ResponseMessageAdd(OcApiMessageType.WarningDev, $"The {nameof(qtySkip)} value must be greater than or equal to 0 but was specified as [{qtySkip}], 0 will be used instead.");
                qtySkip = 0;
            }
            if (qtyReturn < 0)
            {
                ResponseMessageAdd(OcApiMessageType.WarningDev, $"The {nameof(qtyReturn)} value must be greater than 0 but was specified as [{qtyReturn}], 10 will be used instead.");
                qtyReturn = 10;
            }
            query = orderBy?.Count > 0 ? OrderBy(query, orderBy) : OrderByDefault(query);
            return query.Skip(qtySkip).Take(qtyReturn);
        }

      #endregion

      #region Filter by Key

        /// <summary>
        /// Fetches a single entity (record) from the data store that matches <paramref name="key"/>.
        /// </summary>
        protected abstract TEntity DoFindSingle(IQueryable<TEntity> query, TKey key);

        /// <summary>
        /// Fetches a single entity (record) from the data store that matches <paramref name="key"/>.
        /// </summary>
        protected TEntity FindSingle(IQueryable<TEntity> query, TKey key)
        {
            var entity = DoFindSingle(query, key);
            if (entity == null)
            {
                ResponseMessageAdd(OcApiMessageType.WarningDev, $"Entity record for Resource [{ResourceName}] was not found for Key [{key}].");
                return null;
            }
            return entity;
        }

        /// <summary>
        /// Fetches a single entity (record) from the data store that matches the key assigned to <paramref name="entity"/>.
        /// </summary>
        protected virtual TEntity FindSingle(IQueryable<TEntity> query, TEntity entity)
        {
            if (entity == null)
            {
                ResponseMessageAdd(OcApiMessageType.WarningDev, $"Entity record for Resource [{ResourceName}] could not be obtained because no entity was supplied to obtain a key from.");
                return null;
            }
            return FindSingle(query, EntityKey(entity));
        }

        /// <summary>
        /// Returns entities from the data store that match any of <paramref name="keys"/>.
        /// </summary>
        protected abstract IQueryable<TEntity> FindMany(IQueryable<TEntity> query, IEnumerable<TKey> keys);

      #endregion

      #region Filtering

        /// <summary>
        /// Filters the records in <paramref name="query"/> to exclude any that are marked as inactive or deleted.
        /// </summary>
        protected virtual IQueryable<TEntity> ExcludeInactive(IQueryable<TEntity> query)
        {
            if (Features.Has(OcRepositoryFeatures.ExcludeInactive))
            {
                ResponseMessageAdd(OcApiMessageType.WarningSystem, $"Resource [{ResourceName}] by <{GetType().FriendlyName()}> does not implement filtering out inactive records at the data repository layer even though it is specified as a feature.");
                ResponseMessageAdd(OcApiMessageType.WarningDev, $"Resource [{ResourceName}] by <{GetType().FriendlyName()}> does not implement filtering out inactive records at the data repository layer even though it is specified as a feature.");
            }
            return query;
        }

        /// <summary>
        /// Filters the records in <paramref name="query"/> to exclude any that are not authorized for the user specified in <see cref="OcLayerBase{TRequest}.Request"/>.
        /// </summary>
        protected virtual IQueryable<TEntity> ExcludeUnAuthorized(IQueryable<TEntity> query)
        {
            if (Features.Has(OcRepositoryFeatures.ExcludeUnauthorized))
            {
                ResponseMessageAdd(OcApiMessageType.WarningSystem, $"Resource [{ResourceName}] by <{GetType().FriendlyName()}> does not implement filtering out unauthorized records at the data repository layer even though it is specified as a feature.");
                ResponseMessageAdd(OcApiMessageType.WarningDev, $"Resource [{ResourceName}] by <{GetType().FriendlyName()}> does not implement filtering out unauthorized records at the data repository layer even though it is specified as a feature.");
            }
            return query;
        }

      #endregion
 
      #region IOcLayer<TRequest,TEntity,TKey> - Entity Functions

        /// <inheritdoc />
        TKey IOcLayer<TRequest, TEntity, TKey>.EntityKey(TEntity entity)
            => GetEntityKey(entity);

        /// <inheritdoc />
        bool IOcLayer<TRequest, TEntity, TKey>.EntityIsInactive(TEntity entity)
            => GetEntityIsInactive(entity);

        /// <inheritdoc />
        string IOcLayer<TRequest, TEntity, TKey>.EntityStatusSummary(TKey key)
            => EntityStatusSummary(key);

        /// <inheritdoc />
        string IOcLayer<TRequest, TEntity, TKey>.EntityStatusSummary(TEntity entity)
            => EntityStatusSummary(entity);

      #endregion

      #region Inherited Entity Functions

        /// <summary>
        /// Returns the primary key value for <paramref name="entity"/>.
        /// </summary>
        protected override TKey EntityKey(TEntity entity)
            => GetEntityKey(entity);

        /// <summary>
        /// Returns true if <paramref name="entity"/> is marked as inactive or deleted.
        /// For systems that hard delete data from the data store this can always return false.
        /// For systems that maintain deleted records by marking them deleted, override this to return if they are marked as deleted.
        /// </summary>
        protected override bool EntityIsInactive(TEntity entity)
            => GetEntityIsInactive(entity);

      #endregion
   }
}