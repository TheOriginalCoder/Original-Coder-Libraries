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
using OriginalCoder.Common.Interfaces;
using OriginalCoder.Layers.Interfaces;

namespace OriginalCoder.Layers.Requests
{
    [PublicAPI]
    public class OcUserRequest<TUser> : OcRequest, IOcUserRequest<TUser>
        where TUser : class, IOcUser
    {
      #region Constructors

        public OcUserRequest([NotNull] string name, OcRequestOptions options, TUser user)
            : base(name, options)
        {
            User = user;
        }

        public OcUserRequest([NotNull] string name, OcRequestOptions options, TUser user, IOcTextLog log)
            : base(name, options, log)
        {
            User = user;
        }

      #endregion

        /// <summary>
        /// User the request is being performed for.
        /// </summary>
        public TUser User { get; }
    }
}