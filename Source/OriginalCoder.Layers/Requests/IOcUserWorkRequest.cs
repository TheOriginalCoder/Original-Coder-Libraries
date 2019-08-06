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
using OriginalCoder.Layers.Interfaces;

namespace OriginalCoder.Layers.Requests
{
    /// <summary>
    /// Defines a Request to be handled by the Layer system utilizing a Unit of Work on behalf of a user.
    /// </summary>
    [PublicAPI]
    public interface IOcUserWorkRequest<out TUser, out TWork> : IOcUserRequest<TUser>, IOcWorkRequest<TWork>
        where TWork : class, IOcUnitOfWork
        where TUser : class, IOcUser
    { }

    /// <summary>
    /// Defines a Request to be handled by the Layer system utilizing a Unit of Work on behalf of a user.
    /// </summary>
    [PublicAPI]
    public interface IOcUserWorkRequest : IOcUserWorkRequest<IOcUser, IOcUnitOfWork>
    { }
}