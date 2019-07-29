//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

namespace OriginalCoder.Common.Interface.Property
{
    /// <summary>
    /// Standard interface for obtaining a short summary of what the object represents.
    /// </summary>
    public interface ISummary
    {
        string Summary { get; }
    }
}