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
using OriginalCoder.Data.Interfaces;
using OriginalCoder.Layers.Interfaces;

namespace OriginalCoder.Layers.Requests
{
    [PublicAPI]
    public class OcUserWorkRequest<TUser, TWork> : OcWorkRequest<TWork>, IOcUserWorkRequest<TUser, TWork>
        where TUser : class, IOcUser
        where TWork : class, IOcUnitOfWork
    {
      #region Constructors

        public OcUserWorkRequest([NotNull] string name, OcRequestOptions options, TUser user, TWork unitOfWork)
            : base(name, options, unitOfWork)
        {
            User = user;            
        }

        public OcUserWorkRequest([NotNull] string name, OcRequestOptions options, TUser user, TWork unitOfWork, IOcTextLog log)
            : base(name, options, unitOfWork, log)
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