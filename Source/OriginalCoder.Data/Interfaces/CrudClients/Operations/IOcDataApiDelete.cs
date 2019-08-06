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
using OriginalCoder.Common.Api;

namespace OriginalCoder.Data.Interfaces.CrudClients.Operations
{
    /// <summary>
    /// Defines standard operations for deleting entities.
    /// </summary>
    /// <typeparam name="TKey">Data type of keys supported.</typeparam>
    [PublicAPI]
    public interface IOcDataApiDelete<in TKey>
    {
        IOcApiResult<bool> Delete(TKey key);
        IOcApiResult<int> Delete(IEnumerable<TKey> keys);        
    }
}