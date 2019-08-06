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
    public class OrderApi
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime OrderStatusWhen { get; set; }
        public Decimal TotalPrice { get; set; }
        public int CustomerId { get; }
        public IList<int> ProductIds { get; } = new List<int>();
    }

    public enum OrderStatus { InCart, Submitted, Paid, Packing, Shipped, Delivered, Cancelled, Refunded }

    /// <summary>
    /// Domain object for Products to be used internally by the system
    /// </summary>
    [PublicAPI]
    public class OrderDomain : IKeyId, IDescription, IWhenCreated, IWhenUpdated, IWhenDeleted, IIsActive
    {
        private static int _nextId;
        public int Id { get; } = Interlocked.Increment(ref _nextId);

        public string Description => $"Order ID {Id} {Status} on {OrderStatusWhen} for {Customer?.Name} totaling {TotalPrice}";
        public OrderStatus Status { get; set; }
        public DateTime OrderStatusWhen { get; set; }
        public Decimal TotalPrice { get; set; }

        public int CustomerId { get; }
        public CustomerDomain Customer { get; set; }

        public IList<int> ProductIds { get; } = new List<int>();

        public DateTime WhenCreated { get; } = DateTime.UtcNow;
        public DateTime? WhenUpdated { get; } = DateTime.UtcNow;
        public DateTime? WhenDeleted { get; }
        public bool IsActive => WhenDeleted.HasValue;
    }

    /// <summary>
    /// API Controller (such as ASP.MVC Web API) that implements full Create, Read, Update and Delete functionality including paginated results
    /// NOTE: It is common for most API Controllers in this pattern to be this empty, because the work is being done by inheritance
    /// </summary>
    public class MyOrderApiController : MyApiControllerCrudBase<OrderApi, MyOrderApiAdapter>
    {
        public MyOrderApiController([NotNull] IMyLayerConfiguration configuration)
            : base(configuration, SystemConstants.Resource_Order)
        { }

        public MyOrderApiController([NotNull] IMyLayerConfiguration configuration, MyOrderApiAdapter nextLayer)
            : base(configuration, SystemConstants.Resource_Order, nextLayer)
        { }
    }

    /// <summary>
    /// Converts between the Data Transfer Objects and Domain objects
    /// NOTE: It is common for most Adapters in this pattern to be this empty, because the work is being done by inheritance and configured, plug-in services
    /// </summary>
    public class MyOrderApiAdapter : MyApiAdapter<OrderApi, OrderDomain, MyOrderBusinessLogic>
    {
        public MyOrderApiAdapter([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request)
            : base(configuration, SystemConstants.Resource_Order, request)
        { }

        public MyOrderApiAdapter([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request, MyOrderBusinessLogic nextLayer)
            : base(configuration, SystemConstants.Resource_Order, request, nextLayer)
        { }
    }

    /// <summary>
    /// Layer that handles business rules & logic for Products
    /// NOTE: It is common for most Business/Service layers in this pattern to be this empty, because the work is being done by inheritance and configured, plug-in services
    /// </summary>
    public class MyOrderBusinessLogic : MyBusinessLogicCrudBase<OrderDomain, MyOrderRepository>
    {
        public MyOrderBusinessLogic([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request)
            : base(configuration, SystemConstants.Resource_Order, request)
        { }

        public MyOrderBusinessLogic([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request, MyOrderRepository nextLayer)
            : base(configuration, SystemConstants.Resource_Order, request, nextLayer)
        { }
    }

    /// <summary>
    /// Data Repository for loading & saving Products in the underlying data store
    /// NOTE: Most Repositories in this pattern can be fully operational after overriding a few simple, abstract methods from the base class.
    /// </summary>
    public class MyOrderRepository : MyRepositoryCrudBase<OrderDomain>
    {
        public MyOrderRepository([NotNull] IMyLayerConfiguration configuration, [NotNull] IMyRequest request)
            : base(configuration, SystemConstants.Resource_Order, request)
        { }

      #region These are applicaiton specific and must be implemented by the developer for each Repository

        protected override int GetEntityKey(OrderDomain entity) => entity.Id;
        protected override bool GetEntityIsInactive(OrderDomain entity) => entity.IsActive;
        protected override IQueryable<OrderDomain> OrderByDefault(IQueryable<OrderDomain> query) => query.OrderBy(c => c.Id);
        protected override OrderDomain DoFindSingle(IQueryable<OrderDomain> query, int key) => query.FirstOrDefault(c => c.Id == key);
        protected override IQueryable<OrderDomain> FindMany(IQueryable<OrderDomain> query, IEnumerable<int> keys) => query.Where(c => keys.Contains(c.Id));

      #endregion

      #region Future - These will be implemented by standard library base classes (such as for working with Entity Framework)

        protected override IQueryable<OrderDomain> DataSource()
            => new List<OrderDomain>().AsQueryable();  // This will typically be implemented in conjunction with a specialized Repository base class (such as for working with Entity Framework).

        protected override OrderDomain DoCreate(OrderDomain entity)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

        protected override OrderDomain DoUpdate(OrderDomain entity)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

        protected override bool DoDelete(OrderDomain entity)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

        protected override int DoDelete(IEnumerable<OrderDomain> entities)
            => throw new NotImplementedException();  // This will typically be automatically handled by a specialized Repository base class (such as for working with Entity Framework).

      #endregion
    }
}
