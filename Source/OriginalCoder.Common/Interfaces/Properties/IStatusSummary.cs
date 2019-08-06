//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;

namespace OriginalCoder.Common.Interfaces.Properties
{
    /// <summary>
    /// Standard interface for obtaining a short summary for the object and its current status.
    /// </summary>
    [PublicAPI]
    public interface IStatusSummary
    {
        string StatusSummary { get; }
    }
}