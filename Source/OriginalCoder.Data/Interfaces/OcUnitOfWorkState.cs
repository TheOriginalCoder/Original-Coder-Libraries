//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2012, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;

namespace OriginalCoder.Data.Interfaces
{
    [PublicAPI]
    public enum OcUnitOfWorkState
    {
        /// <summary>
        /// Indicates that the unit of work is active and tracking operations
        /// </summary>
        InProgress,

        /// <summary>
        /// Indicates that tracked changes were committed (saved) and the unit of work
        /// is no longer active and should be discarded.
        /// Operations should not be performed when the unit of work is in this state.
        /// </summary>
        Saved,

        /// <summary>
        /// Indicates that tracked changes were rolled back / undone and the unit of work
        /// is no longer active and should be discarded.
        /// Operations should not be performed when the unit of work is in this state.
        /// </summary>
        Cancelled,

        /// <summary>
        /// Indicates that an error has occurred and the the unit of work has become invalid.
        /// Operations should not be performed when the unit of work is in this state.
        /// </summary>
        Error
    }
}