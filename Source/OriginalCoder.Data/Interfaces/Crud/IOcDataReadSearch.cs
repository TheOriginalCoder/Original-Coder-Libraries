//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;
using OriginalCoder.Data.Interfaces.Crud.Operations;

namespace OriginalCoder.Data.Interfaces.Crud
{
    /// <summary>
    /// Defines a set of Read operations that includes the standard set plus search operations
    /// </summary>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    /// <typeparam name="TKey">Data type of keys for <typeparamref name="TEntity"/>.</typeparam>
    /// <typeparam name="TSearchParams">Class type used to specify search parameters</typeparam>
    [PublicAPI]
    public interface IOcDataReadSearch<out TEntity, in TKey, in TSearchParams> : IOcDataRead<TEntity, TKey>, 
                                                                                 IOcDataSearch<TEntity, TSearchParams>, 
                                                                                 IOcDataSearchPaged<TEntity, TSearchParams>
        where TEntity : class
        where TSearchParams : class
    { }
}