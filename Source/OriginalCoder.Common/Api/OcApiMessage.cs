//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using JetBrains.Annotations;

namespace OriginalCoder.Common.Api
{
    /// <summary>
    /// Implementation of <see cref="IOcApiMessage"/> which describes a single message
    /// </summary>
    [PublicAPI]
    public class OcApiMessage : IOcApiMessage
    {
      #region Constructors

        public OcApiMessage(OcApiMessageType type, [NotNull] string message)
        {
            if (type == OcApiMessageType.Unknown)
                throw new ArgumentOutOfRangeException(nameof(type), $"PI Messages can not be of type {nameof(OcApiMessageType.Unknown)}");
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), $"{nameof(message)} must have a non-blank value");

            ApiMessageType = type;
            Message = message.Trim();
        }

        public OcApiMessage(OcApiMessageType type, [NotNull] string message, string referenceType, string referenceKey)
            : this(type, message)
        {
            if (string.IsNullOrWhiteSpace(referenceType) && string.IsNullOrWhiteSpace(referenceKey))
            {
                ReferenceType = null;
                ReferenceKey = null;
                return;
            }

            if (string.IsNullOrWhiteSpace(referenceType))
                throw new ArgumentNullException(nameof(referenceType), $"{nameof(referenceType)} must have a non-blank value if {nameof(referenceKey)} is specified");
            if (string.IsNullOrWhiteSpace(referenceKey))
                throw new ArgumentNullException(nameof(referenceKey), $"{nameof(referenceKey)} must have a non-blank value if {nameof(referenceType)} is specified");

            ReferenceType = referenceType.Trim();
            ReferenceKey = referenceKey.Trim();
        }

      #endregion

        /// <inheritdoc/>
        public OcApiMessageType ApiMessageType { get; }

        /// <inheritdoc/>
        public string Message { get; }

        /// <inheritdoc/>
        public string ReferenceType { get; }

        /// <inheritdoc/>
        public string ReferenceKey { get; }

        public override string ToString()
        {
            return $"{ApiMessageType}: {Message}" + (string.IsNullOrWhiteSpace(ReferenceType) ? "" : $" [RefType:{ReferenceType}]") + (string.IsNullOrWhiteSpace(ReferenceKey) ? "" : $" [RefKey:{ReferenceKey}]");
        }
    }
}