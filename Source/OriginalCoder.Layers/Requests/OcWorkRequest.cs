//=============================================================================
// Original Coder - Layer Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using JetBrains.Annotations;
using OriginalCoder.Common.Interfaces;
using OriginalCoder.Data.Interfaces;

namespace OriginalCoder.Layers.Requests
{
    [PublicAPI]
    public class OcWorkRequest<TWork> : OcRequest, IOcWorkRequest<TWork>
        where TWork : class, IOcUnitOfWork
    {
      #region Constructors

        public OcWorkRequest([NotNull] string name, OcRequestOptions options, TWork unitOfWork)
            : base(name, options)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
            if (unitOfWork.State != OcUnitOfWorkState.InProgress)
                throw new ArgumentException($"Unit of work must be in state InProgress but is [{unitOfWork.State}]", nameof(unitOfWork));

            UnitOfWork = unitOfWork;
        }

        public OcWorkRequest([NotNull] string name, OcRequestOptions options, TWork unitOfWork, IOcTextLog log)
            : base(name, options, log)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
            if (unitOfWork.State != OcUnitOfWorkState.InProgress)
                throw new ArgumentException($"Unit of work must be in state InProgress but is [{unitOfWork.State}]", nameof(unitOfWork));

            UnitOfWork = unitOfWork;
        }

      #endregion

        /// <summary>
        /// Unit of work to be used use when performing operations
        /// </summary>
        public TWork UnitOfWork { get; }
    }
}