//=============================================================================
// Original Coder - Layer Library - Configuration & Implementation Example
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;
using OriginalCoder.Layers.Adapters;
using OriginalCoder.Layers.Base.DataApi;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Interfaces.Data;
using OriginalCoder.Layers.Logic;
using OriginalCoder.Layers.Repositories;
using OriginalCoder.Layers.Requests;

namespace LayerApiMockup
{
    //---------------------------------------------------------------------------------------------
    // It is helpful to define layer specific base classes for use in the application.
    // - Simplifies choices and makes it easier for application developers to choose the best base class when adding new resources.
    // - Known configuration options can be specified which then doesn't require application developers to supply them when coding.
    // - Additional custom behaviors specific to the application can be added centrally.

    public class MyApiControllerCrudBase<TEntity, TNextLayer> : OcLayerApiCrudBase<IMyRequest, TEntity, int, TNextLayer>
        where TEntity : class
        where TNextLayer : class, IOcLayerCrud<IMyRequest, TEntity, int>
    {
      #region Constructors

        protected MyApiControllerCrudBase([NotNull] IMyLayerConfiguration configuration, string resourceName)
            : base(configuration, OcLayerType.Api, resourceName, BuildRequest(resourceName), OcLayerType.Adapter)
        { }

        protected MyApiControllerCrudBase([NotNull] IMyLayerConfiguration configuration, string resourceName, TNextLayer nextLayer)
            : base(configuration, OcLayerType.Api, resourceName, BuildRequest(resourceName), nextLayer)
        { }

      #endregion

        /// <summary>
        /// Construction of the Request will be implemented by technology specific libraries (such as for ASP.NET MVC)
        /// This is a place holder for demonstration.
        /// </summary>
        protected static MyRequest BuildRequest(string resourceName)
        {
            var user = new MyUser { Name = "Someone" };
            var uow = new MyUnitOfWork();
            return new MyRequest($"Request for Resource [{resourceName}]", user, uow, OcRequestOptions.Default);
        }
    }

    public class MyApiAdapter<TEntity, TNextEntity, TNextLayer> : OcLayerAdapterCrudBase<IMyRequest, TEntity, int, TNextEntity, TNextLayer>
        where TEntity : class
        where TNextEntity : class
        where TNextLayer : class, IOcLayerCrud<IMyRequest, TNextEntity, int>
    {
      #region Constructors

        protected MyApiAdapter([NotNull] IMyLayerConfiguration configuration, string resourceName, [NotNull] IMyRequest request)
            : base(configuration, OcLayerType.Adapter, resourceName, request, OcLayerType.Business)
        { }

        protected MyApiAdapter([NotNull] IMyLayerConfiguration configuration, string resourceName, [NotNull] IMyRequest request, TNextLayer nextLayer)
            : base(configuration, OcLayerType.Adapter, resourceName, request, nextLayer)
        { }

      #endregion
    }

    public class MyBusinessLogicCrudBase<TEntity, TNextLayer> : OcLayerLogicCrudBase<IMyRequest, TEntity, int, TNextLayer>
        where TEntity : class
        where TNextLayer : class, IOcLayerCrud<IMyRequest, TEntity, int>
    {
      #region Constructors

        protected MyBusinessLogicCrudBase([NotNull] IMyLayerConfiguration configuration, string resourceName, [NotNull] IMyRequest request)
            : base(configuration, OcLayerType.Business, resourceName, request, OcLayerType.Repository)
        { }

        protected MyBusinessLogicCrudBase([NotNull] IMyLayerConfiguration configuration, string resourceName, [NotNull] IMyRequest request, TNextLayer nextLayer)
            : base(configuration, OcLayerType.Business, resourceName, request, nextLayer)
        { }

      #endregion
    }


    public abstract class MyRepositoryCrudBase<TEntity> : OcLayerRepositoryCrudBase<IMyRequest, TEntity, int>
        where TEntity : class
    {
      #region Constructors

        protected MyRepositoryCrudBase([NotNull] IMyLayerConfiguration configuration, string resourceName, [NotNull] IMyRequest request)
            : base(configuration, OcLayerType.Repository, resourceName, request)
        { }

      #endregion
    }
}
