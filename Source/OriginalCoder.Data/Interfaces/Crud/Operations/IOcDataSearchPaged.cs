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

namespace OriginalCoder.Data.Interfaces.Crud.Operations
{
    /// <summary>
    /// Defines standard operations for performing a search of <typeparamref name="TEntity"/> and returns the results a single page at a time.
    /// Search criteria to be used are specified by <typeparamref name="TSearchParams"/>.
    /// </summary>
    /// <typeparam name="TEntity">Data type of the individual entities</typeparam>
    /// <typeparam name="TSearchParams">Class type used to specify search parameters</typeparam>
    [PublicAPI]
    public interface IOcDataSearchPaged<out TEntity, in TSearchParams>
        where TEntity : class
        where TSearchParams : class
    {
        IReadOnlyList<TEntity> Get(TSearchParams searchParams, int qtySkip, int qtyReturn, IReadOnlyList<(string columnName, bool ascending)> sortOrder = null);
    }
}