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
    /// Used to specify the type / purpose of a message
    /// </summary>
    [PublicAPI]
    public enum OcApiMessageType
    {
        /// <summary>
        /// This is not a valid message type.
        /// </summary>
        Unknown,

        /// <summary>
        /// The message represents an error that occurred
        /// </summary>
        Error,

        /// <summary>
        /// The message relates to authentication failures
        /// </summary>
        AuthenticationFailure,

        /// <summary>
        /// The message relates to authorization failures (lack of sufficient privlidges)
        /// </summary>
        AuthorizationFailure,
        
        /// <summary>
        /// The message relates to data validation failures
        /// </summary>
        ValidationFailure,

        /// <summary>
        /// Warning messages intended for internal system use (logging or record keeping)
        /// </summary>
        WarningSystem,

        /// <summary>
        /// Warning messages intended for internal system use (logging or record keeping)
        /// </summary>
        NotificationSystem,

        /// <summary>
        /// Warning messages intended for display to developers
        /// </summary>
        WarningDev,

        /// <summary>
        /// Notification messages intended for display to developers
        /// </summary>
        NotificationDev,

        /// <summary>
        /// Warning messages intended for display to end users
        /// </summary>
        WarningUser,

        /// <summary>
        /// Warning messages intended for display to end users
        /// </summary>
        NotificationUser,
    }
}
