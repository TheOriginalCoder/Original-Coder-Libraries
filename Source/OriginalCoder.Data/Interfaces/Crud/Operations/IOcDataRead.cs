//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2013, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System.Collections.Generic;
using JetBrains.Annotations;

namespace OriginalCoder.Data.Interfaces.Crud.Operations
{
    /// <summary>
    /// Defines standard operations for reading entities of type <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    [PublicAPI]
    public interface IOcDataRead<out TEntity>
        where TEntity : class
    {
        IReadOnlyList<TEntity> Get();
    }

    /// <summary>
    /// Defines standard operations for reading entities of type <typeparamref name="TEntity"/> that support using keys.
    /// </summary>
    /// <typeparam name="TKey">Data type of keys for <typeparamref name="TEntity"/>.</typeparam>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    [PublicAPI]
    public interface IOcDataRead<out TEntity, in TKey>
        where TEntity : class
    {
        TEntity Get(TKey key);
        IReadOnlyList<TEntity> Get(IEnumerable<TKey> keys);
    }
}