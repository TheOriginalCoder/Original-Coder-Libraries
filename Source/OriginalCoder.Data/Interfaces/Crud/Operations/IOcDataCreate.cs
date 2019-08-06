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

namespace OriginalCoder.Data.Interfaces.Crud.Operations
{
    /// <summary>
    /// Defines standard operations for Creating new entities
    /// </summary>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    [PublicAPI]
    public interface IOcDataCreate<TEntity>
        where TEntity : class
    {
        TEntity CreateAndReturn(TEntity entity);
    }

    /// <summary>
    /// Defines standard operations for Creating new entities for those that support using keys.
    /// </summary>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    /// <typeparam name="TKey">Data type of keys for <typeparamref name="TEntity"/>.</typeparam>
    [PublicAPI]
    public interface IOcDataCreate<in TEntity, out TKey>
        where TEntity : class
    {
        TKey Create(TEntity entity);
    }
}