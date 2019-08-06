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
using JetBrains.Annotations;
using OriginalCoder.Common.Interfaces.Properties;

namespace OriginalCoder.Common.Api
{
    /// <summary>
    /// Specialized collection of <see cref="IOcApiMessage"/>
    /// </summary>
    [PublicAPI]
    public interface IOcApiMessages : IReadOnlyList<IOcApiMessage>, IStatusSummary
    {
      #region Content Information

        /// <summary>
        /// Returns true if the collection contains any messages with a <see cref="OcApiMessageType"/> specified in <paramref name="apiMessageTypes"/>.
        /// </summary>
        bool Has(params OcApiMessageType[] apiMessageTypes);

        /// <summary>
        /// Returns the number of messages in the collection that have a <see cref="OcApiMessageType"/> specified in <paramref name="apiMessageTypes"/>.
        /// </summary>
        int CountByType(params OcApiMessageType[] apiMessageTypes);

        /// <summary>
        /// Returns string containing message counts for <paramref name="messageTypes"/> that are greater than 0 and
        /// prefixed by the total count if <paramref name="includeTotal"/> is true.
        /// </summary>
        string CountsByType(bool includeTotal, params OcApiMessageType[] messageTypes);

        /// <summary>
        /// Returns true if the collection contains any messages with a <see cref="OcApiMessageType"/> specified in <paramref name="apiMessageTypes"/>.
        /// </summary>
        [NotNull] IEnumerable<IOcApiMessage> Filter(params OcApiMessageType[] apiMessageTypes);

      #endregion

      #region Error Messages

        /// <summary>
        /// Returns true if the collection contains any messages that indicate an error or failure.
        /// Errors are message with a type of <see cref="OcApiMessageType.Error"/>, <see cref="OcApiMessageType.AuthenticationFailure"/> or <see cref="OcApiMessageType.AuthorizationFailure"/>.
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// Returns the number of error messages in the collection
        /// Errors are message with a type of <see cref="OcApiMessageType.Error"/>, <see cref="OcApiMessageType.AuthenticationFailure"/> or <see cref="OcApiMessageType.AuthorizationFailure"/>.
        /// </summary>
        int CountErrors { get; }

        /// <summary>
        /// Returns a list that only containing error messages 
        /// Errors are message with a type of <see cref="OcApiMessageType.Error"/>, <see cref="OcApiMessageType.AuthenticationFailure"/> or <see cref="OcApiMessageType.AuthorizationFailure"/>.
        /// </summary>
        [NotNull] IEnumerable<IOcApiMessage> ErrorMessages();

      #endregion

      #region User Messages

        /// <summary>
        /// Returns true if there are any messages suitable for display to an end user.
        /// </summary>
        bool HasUserMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true);

        /// <summary>
        /// Returns the number of messages suitable for display to an end user.
        /// </summary>
        int UserMessageCount(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true);

        /// <summary>
        /// Returns the messages that are suitable for display to an end user.
        /// </summary>
        [NotNull] IEnumerable<IOcApiMessage> UserMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true);

      #endregion

      #region Developer Messages

        /// <summary>
        /// Returns true if there are any messages suitable for display to a developer.
        /// </summary>
        bool HasDeveloperMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true);

        /// <summary>
        /// Returns the number of messages suitable for display to a developer.
        /// </summary>
        int DeveloperMessageCount(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true);

        /// <summary>
        /// Returns the messages that are suitable for display to a developer.
        /// </summary>
        [NotNull] IEnumerable<IOcApiMessage> DeveloperMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true);

      #endregion

      #region System Messages

        /// <summary>
        /// Returns true if there are any messages intended for system use (such as logging or record keeping).
        /// </summary>
        bool HasSystemMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true);

        /// <summary>
        /// Returns the number of messages intended for system use (such as logging or record keeping).
        /// </summary>
        int SystemMessageCount(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true);

        /// <summary>
        /// Returns the messages that are intended for system use (such as logging or record keeping).
        /// </summary>
        [NotNull] IEnumerable<IOcApiMessage> SystemMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true);

      #endregion

        /// <summary>
        /// Returns <paramref name="messages"/> formatted as text into a single string.  Returns a blank string if <paramref name="messages"/> is null or empty.
        /// </summary>
        string MessagesToString([CanBeNull] IEnumerable<IOcApiMessage> messages);
    }
}