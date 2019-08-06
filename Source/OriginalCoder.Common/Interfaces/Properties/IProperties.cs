//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System.Collections.Generic;

namespace OriginalCoder.Common.Interfaces.Properties
{
    /// <summary>
    /// Standard interface for accessing an optional dictionary of properties.
    /// </summary>
    public interface IProperties
    {
        IReadOnlyDictionary<string, object> Properties { get; }
    }
}