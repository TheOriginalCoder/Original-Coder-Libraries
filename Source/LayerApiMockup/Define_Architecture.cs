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
using JetBrains.Annotations;
using OriginalCoder.Common.Api;
using OriginalCoder.Common.Interfaces.Properties;
using OriginalCoder.Data;
using OriginalCoder.Data.Interfaces;
using OriginalCoder.Data.Mapper;
using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Requests;

namespace LayerApiMockup
{
    // The layer configuration is application specific.

    public interface IMyLayerConfiguration : IOcLayerConfiguration<IMyRequest>
    { }

    public class MyLayerConfiguration : OcLayerConfiguration<IMyRequest>, IMyLayerConfiguration
    {
        public MyLayerConfiguration(IOcAuthorizationService<IMyRequest> authorizationService, IOcDataMapper dataMapper)
            : base(authorizationService, dataMapper)
        { }
    }

    //---------------------------------------------------------------------------------------------
    // The user class can be any combination of custom for the application or from libraries to support standard authorization systems.

    [PublicAPI]
    public interface IMyUser : IOcUser, IDescription
    {
        string Login { get; }
        int AccountNumber { get; }
        string FirstName { get; }
        string LastName { get; }
    }

    public class MyUser : IMyUser
    {
        public string Name { get; set; }
        public string UniqueIdentifier { get; set; }
        public string Description { get; set; }
        public string Login { get; set; }
        public int AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    //---------------------------------------------------------------------------------------------
    // A unit of work class will be provided by technology specific Repository libraries (such as for Entity Framework)
    // This is just a place holder for demonstration purposes

    public interface IMyUnitOfWork : IOcUnitOfWork
    { }

    public class MyUnitOfWork : OcUnitOfWorkBase, IMyUnitOfWork
    {
        public override void Commit()
            => throw new System.NotImplementedException();
        public override void Rollback()
            => throw new System.NotImplementedException();
    }

    //---------------------------------------------------------------------------------------------
    // Standard request classes will be provided by some front-end libraries (such as for working with ASP.NET MVC)
    // This is just a place holder for demonstration purposes

    public interface IMyRequest : IOcUserWorkRequest<IMyUser, IMyUnitOfWork>
    { }

    public class MyRequest : OcUserWorkRequest<IMyUser, IMyUnitOfWork>, IMyRequest
    {
        public MyRequest([NotNull] string name, IMyUser user, IMyUnitOfWork unitOfWork, OcRequestOptions options)
            : base(name, options, user, unitOfWork)
        { }
    }

    //---------------------------------------------------------------------------------------------
    // Service abstraction for handling data operation authorizations.  This implementation gives every application the freedom to make their own choices.
    // For real systems these would be implemented to handle authorization correctly

    public class MyAuthorizationService : IOcAuthorizationService<IMyRequest>
    {
        public bool IsAuthorized(IMyRequest request, string resourceName, OcDataOperationType operationType)
            => true;  // Its an example, allow everything
        public bool IsAuthorized(IMyRequest request, string resourceName, string operationName)
            => true;  // Its an example, allow everything
        public bool IsAuthorized<TEntity>(TEntity entity, IMyRequest request, string resourceName, OcDataOperationType operationType)
            => true;  // Its an example, allow everything
        public bool IsAuthorized<TEntity>(TEntity entity, IMyRequest request, string resourceName, string operationName)
            => true;  // Its an example, allow everything
    }

    //---------------------------------------------------------------------------------------------
    // Service abstraction for converting data between types.  This implementation gives every application the freedom to make their own choices.
    // There are many different ways available to do data mapping.
    // A standard data mapping library could be used and these written to interface to its API.
    // Or mappings can be coded by hand which execute fast, are easy to read & debug and generally only take a few minutes each to code.

    public class MyMapperService : IOcDataMapper
    {
        public bool CanConvert<TIn, TOut>() where TIn : class where TOut : class
            => throw new NotImplementedException();  // Future
        public TOut Convert<TIn, TOut>(TIn entity) where TIn : class where TOut : class
            => throw new NotImplementedException();  // Future
        public List<TOut> ConvertToList<TIn, TOut>(IEnumerable<TIn> entities) where TIn : class where TOut : class
            => throw new NotImplementedException();  // Future
        public OcApiResult<TOut> ConvertToApiResult<TIn, TOut>(IOcApiResult<TIn> apiResult) where TIn : class where TOut : class
            => throw new NotImplementedException();  // Future
        public OcApiResult<IReadOnlyList<TOut>> ConvertToApiResultList<TIn, TOut>(IOcApiResult<IReadOnlyList<TIn>> entityList) where TIn : class where TOut : class
            => throw new NotImplementedException();  // Future
    }
}
