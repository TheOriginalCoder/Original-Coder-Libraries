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

namespace OriginalCoder.Common.Api
{
    /// <summary>
    /// Response message for indicating an issue or outcome of a request
    /// </summary>
    [PublicAPI]
    public interface IOcApiMessage
    {
        /// <summary>
        /// Indicates the type of message
        /// </summary>
        OcApiMessageType ApiMessageType { get; }

        /// <summary>
        /// Text of the message
        /// </summary>
        [NotNull] string Message { get; }

        /// <summary>
        /// (Optional) Specifies the type of key provided in <see cref="ReferenceKey"/>.
        /// Note that both <see cref="ReferenceType"/> and <see cref="ReferenceKey"/> must be specified or both must be null/blank.
        /// </summary>
        string ReferenceType { get; }

        /// <summary>
        /// (Optional) Key value related to the message (formatted as a string).
        /// Note that both <see cref="ReferenceType"/> and <see cref="ReferenceKey"/> must be specified or both must be null/blank.
        /// </summary>
        string ReferenceKey { get; }
    }
}