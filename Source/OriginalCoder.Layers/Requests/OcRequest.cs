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
using OriginalCoder.Common;
using OriginalCoder.Common.Api;
using OriginalCoder.Common.Interfaces;

namespace OriginalCoder.Layers.Requests
{
    /// <summary>
    /// Concrete implementation of <see cref="IOcRequest"/>
    /// </summary>
    [PublicAPI]
    public class OcRequest : OcDisposableBase, IOcRequest
    {
      #region Constructors

        public OcRequest([NotNull] string name, OcRequestOptions options)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Options = options;
        }

        public OcRequest([NotNull] string name, OcRequestOptions options, IOcTextLog log)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            Options = options;
            Log = log;
        }

      #endregion

      #region ILayerRequest

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public virtual string StatusSummary => $"Request {Name} (Options: {Options}) (Messages:{ResponseMessages?.StatusSummary})";

        /// <inheritdoc />
        public OcRequestOptions Options { get; }

        /// <inheritdoc />
        public OcApiMessages ResponseMessages { get; } = new OcApiMessages();

        /// <inheritdoc />
        public IOcTextLog Log { get; }

      #endregion
    }
}