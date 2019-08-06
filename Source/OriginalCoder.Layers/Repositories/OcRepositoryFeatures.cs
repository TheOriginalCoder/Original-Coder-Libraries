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

namespace OriginalCoder.Layers.Repositories
{
    [Flags]
    [PublicAPI]
    public enum OcRepositoryFeatures
    {
        None                = 0x00000,

        ExcludeInactive     = 0x00001,
        ExcludeUnauthorized = 0x00002,

        VerboseMessages     = 0x00010,
        MessageExcluded     = 0x00020,
        MessageUnauthorized = 0x00040,

        ValidateEntities    = 0x00100,

        SelectDetails       = 0x01000,
        SelectReporting     = 0x02000,
        SelectSummary       = 0x04000,
        SelectMinimal       = 0x08000,

        ColumnList          = 0x10000,
        CustomOrderBy       = 0x20000,
        SelectCustom  = 0x40000,
    }
}