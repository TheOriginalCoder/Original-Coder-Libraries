//=============================================================================
// Original Coder - Layer Library - Configuration & Implementation Example
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
using System.Threading;
using JetBrains.Annotations;
using OriginalCoder.Common.Interfaces.Properties;
using OriginalCoder.Data.Interfaces.Keys;
using OriginalCoder.Data.Interfaces.Properties;

namespace LayerApiMockup
{
    /// <summary>
    /// Data Transfer Object for Products to be used with the API
    /// </summary>
    [PublicAPI]
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
    }

    /// <summary>
    /// Domain object for Products to be used internally by the system
    /// </summary>
    [PublicAPI]
    public class ProductDomain : IKeyId, IName, IDescription, IWhenCreated, IWhenUpdated, IWhenDeleted, IIsActive
    {
        private static int _nextId;
        public int Id { get; } = Interlocked.Increment(ref _nextId);
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public DateTime WhenCreated { get; } = DateTime.UtcNow;
        public DateTime? WhenUpdated { get; } = DateTime.UtcNow;
        public DateTime? WhenDeleted { get; set; }
        public bool IsActive => WhenDeleted.HasValue;
    }

    /// <summary>
    /// API Controller (such as ASP.MVC Web API) that implements full Create, Read, Update and Delete functionality including paginated results
    /// NOTE: It is common for most API Controllers in this pattern to be this empty, because the work is being done by inheritance
    /// </summary>
    public class MyProductApiController : MyApiControllerCrudBase<ProductDto, MyProductApiAdapter>
    {
        public MyProductApiController([NotNull] IMyLayerConfiguration configuration)
            : base(configuration, SystemConstants.Resource_Product)
        { }

        public MyProductApiController([NotNull] IMyLayerConfiguration configuration, MyProductApiAdapter nextLayer)
            : base(configuration, SystemConstants.Resource_Product, nextLayer)
        { }
    }

    /// <summary>
    /// Converts between the Data Transfer Objects and Domain objects
    /// NOTE: It is common for most Adapters in this pattern to be this empty, because the work is being done by inheritance and configured, plug-in services
    /// </summary>
    public class MyProductApiAdapter : MyApiAdapter<ProductDto, ProductDomain, MyProductBusinessLogic>
    {
        public MyProductApiAdapter([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request)
            : base(configuration, SystemConstants.Resource_Product, request)
        { }

        public MyProductApiAdapter([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request, MyProductBusinessLogic nextLayer)
            : base(configuration, SystemConstants.Resource_Product, request, nextLayer)
        { }
    }

    /// <summary>
    /// Layer that handles business rules & logic for Products
    /// NOTE: It is common for most Business/Service layers in this pattern to be this empty, because the work is being done by inheritance and configured, plug-in services
    /// </summary>
    public class MyProductBusinessLogic : MyBusinessLogicCrudBase<ProductDomain, MyProductRepository>
    {
        public MyProductBusinessLogic([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request)
            : base(configuration, SystemConstants.Resource_Product, request)
        { }

        public MyProductBusinessLogic([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request, MyProductRepository nextLayer)
            : base(configuration, SystemConstants.Resource_Product, request, nextLayer)
        { }
    }

    /// <summary>
    /// Data Repository for loading & saving Products in the underlying data store
    /// NOTE: Most Repositories in this pattern can be fully operational after overriding a few simple, abstract methods from the base class.
    /// </summary>
    public class MyProductRepository : MyRepositoryCrudBase<ProductDomain>
    {
        public MyProductRepository([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request)
            : base(configuration, SystemConstants.Resource_Product, request)
        { }

      #region These are applicaiton specific and must be implemented by the developer for each Repository

        protected override int GetEntityKey(ProductDomain entity) => entity.Id;
        protected override bool GetEntityIsInactive(ProductDomain entity) => entity.IsActive;
        protected override IQueryable<ProductDomain> OrderByDefault(IQueryable<ProductDomain> query) => query.OrderBy(c => c.Name);
        protected override ProductDomain DoFindSingle(IQueryable<ProductDomain> query, int key) => query.FirstOrDefault(c => c.Id == key);
        protected override IQueryable<ProductDomain> FindMany(IQueryable<ProductDomain> query, IEnumerable<int> keys) => query.Where(c => keys.Contains(c.Id));

      #endregion

      #region Future - These will be implemented by standard library base classes (such as for working with Entity Framework)

        protected override IQueryable<ProductDomain> DataSource()
            => new List<ProductDomain>().AsQueryable();  // This will typically be implemented in conjunction with a specialized Repository base class (such as for working with Entity Framework).

        protected override ProductDomain DoCreate(ProductDomain entity)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

        protected override ProductDomain DoUpdate(ProductDomain entity)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

        protected override bool DoDelete(ProductDomain entity)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

        protected override int DoDelete(IEnumerable<ProductDomain> entities)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

      #endregion
    }
}
