//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System.Collections.Generic;
using JetBrains.Annotations;
using OriginalCoder.Common.Api;

namespace OriginalCoder.Data.Interfaces.CrudClients.Operations
{
    /// <summary>
    /// Defines standard operations for reading entities of type <typeparamref name="TEntity"/> one page at a time.
    /// </summary>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    [PublicAPI]
    public interface IOcDataApiReadPaged<out TEntity>
        where TEntity : class
    {
        IOcApiResult<IReadOnlyList<TEntity>> Get(int qtySkip, int qtyReturn, IReadOnlyList<(string columnName, bool ascending)> sortOrder = null);
    }
}