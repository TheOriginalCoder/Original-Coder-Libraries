//=============================================================================
// Original Coder - Layer Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;
using OriginalCoder.Data.Interfaces;

namespace OriginalCoder.Layers.Requests
{
    /// <summary>
    /// Defines a Request to be handled by the Layer system utilizing a Unit of Work.
    /// </summary>
    [PublicAPI]
    public interface IOcWorkRequest<out TWork> : IOcRequest
        where TWork : class, IOcUnitOfWork
    {
        /// <summary>
        /// Unit of work to be used use when performing operations
        /// </summary>
        TWork UnitOfWork { get; }
    }
}