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
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; }
        string CompanyName { get; set; }
        string Address { get; set; }
        string AddressCity { get; set; }
        string AddressState { get; set; }
        string AddressZipCode { get; set; }
        string Phone { get; set; }
    }

    /// <summary>
    /// Domain object for Products to be used internally by the system
    /// </summary>
    [PublicAPI]
    public class CustomerDomain : IKeyId, IName, IDescription, IWhenCreated, IWhenUpdated, IWhenDeleted, IIsActive
    {
        private static int _nextId;
        public int Id { get; } = Interlocked.Increment(ref _nextId);
        public string Name { get; set; }
        public string Description { get; }
        string CompanyName { get; set; }
        string Address { get; set; }
        string AddressCity { get; set; }
        string AddressState { get; set; }
        string AddressZipCode { get; set; }
        string Phone { get; set; }
        public DateTime WhenCreated { get; } = DateTime.UtcNow;
        public DateTime? WhenUpdated { get; } = DateTime.UtcNow;
        public DateTime? WhenDeleted { get; }
        public bool IsActive => WhenDeleted.HasValue;
    }

    public static class SystemConstants
    {
        public const string Resource_Customer = "Customer";
        public const string Resource_Product = "Product";
        public const string Resource_Order = "Order";
    }

    /// <summary>
    /// API Controller (such as ASP.MVC Web API) that implements full Create, Read, Update and Delete functionality including paginated results
    /// NOTE: It is common for most API Controllers in this pattern to be this empty, because the work is being done by inheritance
    /// </summary>
    public class MyCustomerApiController : MyApiControllerCrudBase<CustomerDto, MyCustomerApiAdapter>
    {
        public MyCustomerApiController([NotNull] IMyLayerConfiguration configuration)
            : base(configuration, SystemConstants.Resource_Customer)
        { }

        public MyCustomerApiController([NotNull] IMyLayerConfiguration configuration, MyCustomerApiAdapter nextLayer)
            : base(configuration, SystemConstants.Resource_Customer, nextLayer)
        { }
    }

    /// <summary>
    /// Converts between the Data Transfer Objects and Domain objects
    /// NOTE: It is common for most Adapters in this pattern to be this empty, because the work is being done by inheritance and configured, plug-in services
    /// </summary>
    public class MyCustomerApiAdapter : MyApiAdapter<CustomerDto, CustomerDomain, MyCustomerBusinessLogic>
    {
        public MyCustomerApiAdapter([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request)
            : base(configuration, SystemConstants.Resource_Customer, request)
        { }

        public MyCustomerApiAdapter([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request, MyCustomerBusinessLogic nextLayer)
            : base(configuration, SystemConstants.Resource_Customer, request, nextLayer)
        { }
    }

    /// <summary>
    /// Layer that handles business rules & logic for Products
    /// NOTE: It is common for most Business/Service layers in this pattern to be this empty, because the work is being done by inheritance and configured, plug-in services
    /// </summary>
    public class MyCustomerBusinessLogic : MyBusinessLogicCrudBase<CustomerDomain, MyCustomerRepository>
    {
        public MyCustomerBusinessLogic([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request)
            : base(configuration, SystemConstants.Resource_Customer, request)
        { }

        public MyCustomerBusinessLogic([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request, MyCustomerRepository nextLayer)
            : base(configuration, SystemConstants.Resource_Customer, request, nextLayer)
        { }
    }

    /// <summary>
    /// Data Repository for loading & saving Products in the underlying data store
    /// NOTE: Most Repositories in this pattern can be fully operational after overriding a few simple, abstract methods from the base class.
    /// </summary>
    public class MyCustomerRepository : MyRepositoryCrudBase<CustomerDomain>
    {
        public MyCustomerRepository([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request)
            : base(configuration, SystemConstants.Resource_Customer, request)
        { }

      #region These are applicaiton specific and must be implemented by the developer for each Repository

        protected override int GetEntityKey(CustomerDomain entity) => entity.Id;
        protected override bool GetEntityIsInactive(CustomerDomain entity) => entity.IsActive;
        protected override IQueryable<CustomerDomain> OrderByDefault(IQueryable<CustomerDomain> query) => query.OrderBy(c => c.Name);
        protected override CustomerDomain DoFindSingle(IQueryable<CustomerDomain> query, int key) => query.FirstOrDefault(c => c.Id == key);
        protected override IQueryable<CustomerDomain> FindMany(IQueryable<CustomerDomain> query, IEnumerable<int> keys) => query.Where(c => keys.Contains(c.Id));

      #endregion

      #region Future - These will be implemented by standard library base classes (such as for working with Entity Framework)

        protected override IQueryable<CustomerDomain> DataSource()
            => new List<CustomerDomain>().AsQueryable();  // This will typically be implemented in conjunction with a specialized Repository base class (such as for working with Entity Framework).

        protected override CustomerDomain DoCreate(CustomerDomain entity)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

        protected override CustomerDomain DoUpdate(CustomerDomain entity)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

        protected override bool DoDelete(CustomerDomain entity)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

        protected override int DoDelete(IEnumerable<CustomerDomain> entities)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

      #endregion
    }
}
