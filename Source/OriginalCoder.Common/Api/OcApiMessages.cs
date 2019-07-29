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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace OriginalCoder.Common.Api
{
    /// <summary>
    /// Specialized list of <see cref="IOcApiMessage"/> that implements <see cref="IOcApiMessages"/>.
    /// </summary>
    /// <remarks>
    /// This object is NOT THREAD SAFE.
    /// </remarks>
    [PublicAPI]
    public class OcApiMessages : IOcApiMessages
    {
      #region Static

        public static void Build([CanBeNull] ref OcApiMessages messageList, [CanBeNull] params IOcApiMessage[] messagesToAdd)
        {
            if (messagesToAdd == null || messagesToAdd.Length == 0)
                return;

            if (messageList == null)
                messageList = new OcApiMessages();
            messageList.Add(messagesToAdd);
        }

        public static void Build([CanBeNull] ref OcApiMessages messageList, [CanBeNull] IEnumerable<IOcApiMessage> messagesToAdd)
        {
            if (messagesToAdd == null)
                return;

            if (messageList == null)
                messageList = new OcApiMessages();
            messageList.Add(messagesToAdd);
        }

      #endregion

      #region Constructors

        public OcApiMessages([CanBeNull] params IOcApiMessage[] messages)
        {
            Add(messages);
        }

        public OcApiMessages([CanBeNull] IEnumerable<IOcApiMessage> messages)
        {
            Add(messages);
        }

        #endregion

        private readonly List<IOcApiMessage> _messages = new List<IOcApiMessage>();

      #region Add Messages

        public void Add([CanBeNull] IOcApiMessage message)
        {
            if (message == null)
                return;

            _messages.Add(message);
        }

        public void Add([CanBeNull] params IOcApiMessage[] messages)
        {
            if (messages == null || messages.Length == 0)
                return;

            _messages.AddRange(messages.Where(m => m != null));
        }

        public void Add([CanBeNull] IEnumerable<IOcApiMessage> messages)
        {
            if (messages == null)
                return;

            _messages.AddRange(messages.Where(m => m != null));
        }

      #endregion

      #region IReadOnlyList<IOcApiMessage>

        /// <inheritdoc/>
        public IEnumerator<IOcApiMessage> GetEnumerator() => _messages.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => _messages.GetEnumerator();

        /// <inheritdoc/>
        public int Count => _messages.Count;

        /// <inheritdoc/>
        public IOcApiMessage this[int index] => _messages[index];

      #endregion

      #region Content Information

        /// <inheritdoc/>
        public bool Has([NotNull] params OcApiMessageType[] apiMessageTypes)
        {
            if (apiMessageTypes == null)
                throw new ArgumentNullException(nameof(apiMessageTypes));
            if (apiMessageTypes.Length == 0)
                throw new ArgumentNullException(nameof(apiMessageTypes));

            return _messages.Any(m => apiMessageTypes.Contains(m.ApiMessageType));
        }

        /// <inheritdoc/>
        public int CountByType(params OcApiMessageType[] apiMessageTypes)
        {
            if (apiMessageTypes == null)
                throw new ArgumentNullException(nameof(apiMessageTypes));
            if (apiMessageTypes.Length == 0)
                throw new ArgumentNullException(nameof(apiMessageTypes));

            return _messages.Count(m => apiMessageTypes.Contains(m.ApiMessageType));
        }

        /// <inheritdoc/>
        [NotNull] public IEnumerable<IOcApiMessage> Filter(params OcApiMessageType[] apiMessageTypes)
        {
            if (apiMessageTypes == null)
                throw new ArgumentNullException(nameof(apiMessageTypes));
            if (apiMessageTypes.Length == 0)
                throw new ArgumentNullException(nameof(apiMessageTypes));

            return _messages.Where(m => apiMessageTypes.Contains(m.ApiMessageType));
        }

      #endregion

      #region Error Messages

        protected readonly OcApiMessageType[] ErrorMessageTypes = { OcApiMessageType.Error, OcApiMessageType.AuthenticationFailure, OcApiMessageType.AuthorizationFailure, OcApiMessageType.ValidationFailure };

        /// <inheritdoc/>
        public bool HasErrors
            => Has(ErrorMessageTypes);

        /// <inheritdoc/>
        public int CountErrors 
            => CountByType(ErrorMessageTypes);

        /// <inheritdoc/>
        [NotNull] public IEnumerable<IOcApiMessage> ErrorMessages()
            => Filter(ErrorMessageTypes);

      #endregion

        protected OcApiMessageType[] MessageTypes(bool includeErrors, bool includeWarnings, bool includeNotifications, bool forUser, bool forDev, bool forSystem)
        {
            var types = new List<OcApiMessageType>();

            if (includeErrors)
                types.AddRange(ErrorMessageTypes);

            if (forUser && includeWarnings)
                types.Add(OcApiMessageType.WarningUser);
            if (forUser && includeNotifications)
                types.Add(OcApiMessageType.NotificationUser);

            if (forDev && includeWarnings)
                types.Add(OcApiMessageType.WarningDev);
            if (forDev && includeNotifications)
                types.Add(OcApiMessageType.NotificationDev);

            if (forSystem && includeWarnings)
                types.Add(OcApiMessageType.WarningSystem);
            if (forSystem && includeNotifications)
                types.Add(OcApiMessageType.NotificationSystem);

            return types.ToArray();
        }

      #region User Messages

        /// <inheritdoc/>
        public bool HasUserMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true)
            => Has(MessageTypes(includeErrors, includeWarnings, includeNotifications, true, false, false));

        /// <inheritdoc/>
        public int UserMessageCount(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true)
            => CountByType(MessageTypes(includeErrors, includeWarnings, includeNotifications, true, false, false));

        /// <inheritdoc/>
        [NotNull] public IEnumerable<IOcApiMessage> UserMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true)
            => Filter(MessageTypes(includeErrors, includeWarnings, includeNotifications, true, false, false));

      #endregion

      #region Developer Messages

        /// <inheritdoc/>
        public bool HasDeveloperMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true)
            => Has(MessageTypes(includeErrors, includeWarnings, includeNotifications, false, true, false));

        /// <inheritdoc/>
        public int DeveloperMessageCount(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true)
            => CountByType(MessageTypes(includeErrors, includeWarnings, includeNotifications, false, true, false));

        /// <inheritdoc/>
        [NotNull] public IEnumerable<IOcApiMessage> DeveloperMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true)
            => Filter(MessageTypes(includeErrors, includeWarnings, includeNotifications, false, true, false));

      #endregion

      #region System Messages

        /// <inheritdoc/>
        public bool HasSystemMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true)
            => Has(MessageTypes(includeErrors, includeWarnings, includeNotifications, false, false, true));

        /// <inheritdoc/>
        public int SystemMessageCount(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true)
            => CountByType(MessageTypes(includeErrors, includeWarnings, includeNotifications, false, false, true));

        /// <inheritdoc/>
        [NotNull] public IEnumerable<IOcApiMessage> SystemMessages(bool includeErrors = true, bool includeWarnings = true, bool includeNotifications = true)
            => Filter(MessageTypes(includeErrors, includeWarnings, includeNotifications, false, false, true));

      #endregion

        /// <inheritdoc/>
        [NotNull] public string MessagesToString([CanBeNull] IEnumerable<IOcApiMessage> messages)
        {
            if (messages == null)
                return "";

            var result = "";
            foreach (var message in messages.Where(m => m != null))
            {
                result = result + (result.Length > 0 ? "\n" : "") + message;
            }
            return result;
        }

        [NotNull] public override string ToString()
        {
            if (Count == 0)
                return "Messages (EMPTY)";
            return $"Messages: Qty [{Count}] HasErrors [{HasErrors}]";
        }
    }
}