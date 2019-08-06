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
    /// Defines standard operations for performing a search of <typeparamref name="TEntity"/> and only returns
    /// those that match the search criteria specified in <typeparamref name="TSearchCriteria"/>.
    /// </summary>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    /// <typeparam name="TSearchCriteria">Class type used to specify search parameters</typeparam>
    [PublicAPI]
    public interface IOcDataApiSearch<out TEntity, in TSearchCriteria>
        where TEntity : class
        where TSearchCriteria : class
    {
        IOcApiResult<IReadOnlyList<TEntity>> Get(TSearchCriteria searchParams);
    }
}