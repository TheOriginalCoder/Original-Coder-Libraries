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

namespace OriginalCoder.Layers.Requests
{
    /// <summary>
    /// Specifies the options to apply when processing a request
    /// </summary>
    [Flags] [PublicAPI]
    public enum OcRequestOptions
    {
        // Options Sets
        None                = 0x0000,
        Default             = 0x0103,
        DefaultFindAny      = 0x0160,
        DefaultQueryAll     = 0x0140,
        DefaultQuery        = 0x0143,
        All                 = 0xFFFF,

        // Individual Options

        ExcludeInactive     = 0x0001, 
        ExcludeUnauthorized = 0x0002, 

        VerboseMessages     = 0x0010,
        MessageExcluded     = 0x0020,
        MessageUnauthorized = 0x0040,

        ValidateEntities    = 0x0100,

        SelectDetails       = 0x1000,
        SelectReporting     = 0x2000,
        SelectSummary       = 0x4000,
        SelectMinimal       = 0x8000,
    }
}