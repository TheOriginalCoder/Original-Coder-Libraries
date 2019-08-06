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

namespace OriginalCoder.Data.Interfaces.Crud
{
    /// <summary>
    /// Defines a set of operations that include the standard Create, Read, Update and Delete operations plus adds search operations
    /// </summary>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    /// <typeparam name="TKey">Data type of keys for <typeparamref name="TEntity"/>.</typeparam>
    /// <typeparam name="TSearchParams">Class type used to specify search parameters</typeparam>
    [PublicAPI]
    public interface IOcDataCrudSearch<TEntity, TKey, in TSearchParams> : IOcDataCrud<TEntity, TKey>,
                                                                          IOcDataReadSearch<TEntity, TKey, TSearchParams>
        where TEntity : class
        where TSearchParams : class
    { }
}