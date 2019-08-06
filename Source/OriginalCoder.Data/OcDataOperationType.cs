//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2011, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;

namespace OriginalCoder.Data
{
    /// <summary>
    /// Defines a standard set of operation types
    /// </summary>
    [PublicAPI]
    public enum OcDataOperationType
    {
        Read,
        Create,
        Update,
        Delete,
        Other
    }
}