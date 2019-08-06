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
using OriginalCoder.Common.Api;

namespace OriginalCoder.Data.Interfaces.CrudClients.Operations
{
    /// <summary>
    /// Defines standard operations for updating existing entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    [PublicAPI]
    public interface IOcDataApiUpdate<TEntity>
        where TEntity : class
    {
        IOcApiResult<bool> Update(TEntity entity);
        IOcApiResult<TEntity> UpdateAndReturn(TEntity entity);
    }
}