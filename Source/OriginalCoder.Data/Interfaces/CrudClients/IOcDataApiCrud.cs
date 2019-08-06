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
    /// Defines a set of standard Create, Read, Update and Delete operations
    /// </summary>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    /// <typeparam name="TKey">Data type of keys for <typeparamref name="TEntity"/>.</typeparam>
    [PublicAPI]
    public interface IOcDataApiCrud<TEntity, TKey> : IOcDataApiCreate<TEntity>,
                                                     IOcDataApiCreate<TEntity, TKey>,
                                                     IOcDataApiRead<TEntity, TKey>,
                                                     IOcDataApiUpdate<TEntity>,
                                                     IOcDataApiDelete<TKey>
        where TEntity : class
    { }
}