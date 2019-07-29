//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2011, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

namespace OriginalCoder.Data.Interface.Property
{
    /// <summary>
    /// Compatibility interface for objects that use a boolean value to indicate if they have been marked as deleted.
    /// </summary>
    public interface IIsActive
    {
        bool IsActive { get; }
    }
}