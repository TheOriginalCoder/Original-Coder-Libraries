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
using OriginalCoder.Common.Interfaces.Properties;

namespace OriginalCoder.Layers.Interfaces
{
    /// <summary>
    /// Base interface for representing an authenticated user
    /// </summary>
    [PublicAPI]
    public interface IOcUser : IName
    {
        string UniqueIdentifier { get; }
    }
}