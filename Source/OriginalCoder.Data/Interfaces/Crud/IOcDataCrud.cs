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
using OriginalCoder.Data.Interfaces.Crud.Operations;

namespace OriginalCoder.Data.Interfaces.Crud
{
    /// <summary>
    /// Defines a set of standard Create, Read, Update and Delete operations
    /// </summary>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    /// <typeparam name="TKey">Data type of keys for <typeparamref name="TEntity"/>.</typeparam>
    [PublicAPI]
    public interface IOcDataCrud<TEntity, TKey> : IOcDataCreate<TEntity>, 
                                                  IOcDataCreate<TEntity, TKey>,
                                                  IOcDataRead<TEntity, TKey>,
                                                  IOcDataUpdate<TEntity>, 
                                                  IOcDataDelete<TKey>
        where TEntity : class
    { }
}