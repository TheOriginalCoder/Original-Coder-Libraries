//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2009, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;
using OriginalCoder.Common.Api;

namespace OriginalCoder.Data.Interfaces
{
    /// <summary>
    /// Interface that implements a standard method of validation
    /// </summary>
    [PublicAPI]
    public interface IOcValidation
    {
        /// <summary>
        /// Contains the messages that were generated the last time the <see cref="IsValid"/> method was called.
        /// Will be null if <see cref="IsValid"/> was never called.  If the collection is not null and contains
        /// no error messages then the object was valid the last time <see cref="IsValid"/> was called.
        /// </summary>
        IOcApiMessages ValidationMessages { get; }

        /// <summary>
        /// Causes the object to validate itself.  Populates <see cref="ValidationMessages"/> with a collection of
        /// <see cref="IOcApiMessage"/> that describe any issues found during the validation process.  Returns
        /// true if the validation process did not generate any errors.
        /// </summary>
        bool IsValid();
    }
}