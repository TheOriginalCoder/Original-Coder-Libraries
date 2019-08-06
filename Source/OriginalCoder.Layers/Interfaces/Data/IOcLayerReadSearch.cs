//=============================================================================
// Original Coder - Layer Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;
using OriginalCoder.Data.Interfaces.Crud;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Interfaces.Data
{
    /// <summary>
    /// Layer implementation of Read and Search data operations using the interface <see cref="IOcDataReadSearch{TEntity,TKey,TSearchParams}"/>
    /// </summary>
    /// <typeparam name="TRequest">Type that encapsulates the incoming request and its processing.</typeparam>
    /// <typeparam name="TEntity">The type of data/entity/domain object the layer operates on.</typeparam>
    /// <typeparam name="TKey">Type for the unique key used by <typeparamref name="TEntity"/>.</typeparam>
    /// <typeparam name="TSearch">Type that specifies the parameters to use for search operations.</typeparam>
    [PublicAPI]
    public interface IOcLayerReadSearch<out TRequest, out TEntity, in TKey, in TSearch> : IOcDataReadSearch<TEntity, TKey, TSearch>, IOcLayerRead<TRequest, TEntity, TKey>
        where TRequest : class, IOcRequest
        where TEntity : class
        where TSearch : class
    { }
}