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
using OriginalCoder.Common.Api;
using OriginalCoder.Common.Interfaces;
using OriginalCoder.Common.Interfaces.Properties;

namespace OriginalCoder.Layers.Requests
{
    /// <summary>
    /// Defines a Request to be handled by the Layer system.
    /// </summary>
    [PublicAPI]
    public interface IOcRequest : IDisposable, IName, IStatusSummary
    {
        OcRequestOptions Options { get; }
        OcApiMessages ResponseMessages { get; }
        IOcTextLog Log { get; }
    }
}