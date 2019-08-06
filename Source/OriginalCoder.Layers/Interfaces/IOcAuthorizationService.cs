//=============================================================================
// Original Coder - Layer Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using OriginalCoder.Data;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Interfaces
{
    public interface IOcAuthorizationService<in TRequest>
        where TRequest : class, IOcRequest
    {
        /// <summary>
        /// Returns true if <paramref name="request"/> is allowed to perform <paramref name="operationType"/> on <paramref name="resourceName"/>.
        /// </summary>
        bool IsAuthorized(TRequest request, string resourceName, OcDataOperationType operationType);

        /// <summary>
        /// Returns true if <paramref name="request"/> is allowed to perform <paramref name="operationName"/> on <paramref name="resourceName"/>.
        /// </summary>
        bool IsAuthorized(TRequest request, string resourceName, string operationName);

        /// <summary>
        /// Returns true if <paramref name="request"/> is allowed to perform <paramref name="operationType"/> on <paramref name="entity"/> in <paramref name="resourceName"/>.
        /// </summary>
        bool IsAuthorized<TEntity>(TEntity entity, TRequest request, string resourceName, OcDataOperationType operationType);

        /// <summary>
        /// Returns true if <paramref name="request"/> is allowed to perform <paramref name="operationName"/> on <paramref name="entity"/> in <paramref name="resourceName"/>.
        /// </summary>
        bool IsAuthorized<TEntity>(TEntity entity, TRequest request, string resourceName, string operationName);
    }
}