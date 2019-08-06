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
using OriginalCoder.Layers.Interfaces;

namespace OriginalCoder.Layers.Requests
{
    /// <summary>
    /// Defines a Request to be handled by the Layer system on behalf of a user.
    /// </summary>
    [PublicAPI]
    public interface IOcUserRequest<out TUser> : IOcRequest
        where TUser : class, IOcUser
    {
        /// <summary>
        /// Authorized user the work is being performed for.
        /// </summary>
        TUser User { get; }
    }
}