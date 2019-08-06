//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2013, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;
using OriginalCoder.Data.Interfaces.CrudClients.Operations;

namespace OriginalCoder.Data.Interfaces.CrudClients
{
    /// <summary>
    /// Defines a set of standard Read operations
    /// </summary>
    /// <typeparam name="TKey">Data type of keys for <typeparamref name="TEntity"/>.</typeparam>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    [PublicAPI]
    public interface IOcDataApiRead<out TEntity, in TKey> : IOcDataApiRead<TEntity>, Operations.IOcDataApiRead<TEntity, TKey>, 
                                                            IOcDataApiReadPaged<TEntity>
        where TEntity : class
    { }
}