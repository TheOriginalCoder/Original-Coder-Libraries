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
using System.Collections.Generic;
using JetBrains.Annotations;

namespace OriginalCoder.Common.Api
{
    /// <summary>
    /// Concrete implementation of <see cref="IOcApiResult"/>.
    /// </summary>
    [PublicAPI]
    public class OcApiResult : IOcApiResult
    {
      #region Constructors

        /// <summary>
        /// Constructs an instance of <see cref="IOcApiResult"/> for returning the results of an operation.
        /// </summary>
        /// <param name="name">[Required] Name of the operation performed that these results are for</param>
        public OcApiResult([NotNull] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "API Results must have a name");
            Name = name.Trim();
        }

        /// <summary>
        /// Constructs an instance of <see cref="IOcApiResult"/> for returning the results of an operation.
        /// </summary>
        /// <param name="name">[Required] Name of the operation performed that these results are for</param>
        /// <param name="messages">Optional collection of <see cref="IOcApiMessage"/> to be added to <see cref="Messages"/>.</param>
        public OcApiResult([NotNull] string name, [CanBeNull] params IOcApiMessage[] messages)
            : this(name)
        {
            Add(messages);
        }

        /// <summary>
        /// Constructs an instance of <see cref="IOcApiResult"/> for returning the results of an operation.
        /// </summary>
        /// <param name="name">[Required] Name of the operation performed that these results are for</param>
        /// <param name="messages">Optional collection of <see cref="IOcApiMessage"/> to be added to <see cref="Messages"/>.</param>
        public OcApiResult([NotNull] string name, [CanBeNull] IEnumerable<IOcApiMessage> messages)
            : this(name)
        {
            Add(messages);
        }

      #endregion

        public string Name { get; }
        public string Summary => ToString();

        /// <inheritdoc/>
        public IOcApiMessages Messages { get; }
        private OcApiMessages _apiMessages;

        /// <inheritdoc/>
        public bool Success => Messages?.HasErrors != true;

      #region Add Messages

        /// <summary>
        /// Adds a message to <see cref="Messages"/>.  If <see cref="Messages"/> is null it will be instantiated before adding.
        /// Has no effect if <paramref name="message"/> is null.
        /// </summary>
        public void Add([CanBeNull] IOcApiMessage message)
        {
            if (message == null)
                return;

            if (_apiMessages == null)
                _apiMessages = new OcApiMessages();
            _apiMessages.Add(message);
        }

        /// <summary>
        /// Adds zero or more messages to <see cref="Messages"/>.  If <see cref="Messages"/> is null it will be instantiated before adding.
        /// Has no effect if <paramref name="messages"/> is null or length 0.
        /// </summary>
        public void Add([CanBeNull] params IOcApiMessage[] messages)
        {
            if (messages == null || messages.Length == 0)
                return;

            if (_apiMessages == null)
                _apiMessages = new OcApiMessages();
            _apiMessages.Add(messages);
        }

        /// <summary>
        /// Adds zero or more messages to <see cref="Messages"/>.  If <see cref="Messages"/> is null it will be instantiated before adding.
        /// Has no effect if <paramref name="messages"/> is null.
        /// </summary>
        public void Add([CanBeNull] IEnumerable<IOcApiMessage> messages)
        {
            if (messages == null)
                return;

            if (_apiMessages == null)
                _apiMessages = new OcApiMessages();
            _apiMessages.Add(messages);
        }

      #endregion

        public override string ToString()
        {
            return $"API Operation [{Name}] " + (Success ? "was successful" : "FAILED") + $" with {Messages?.CountErrors ?? 0} errors and {Messages?.Count ?? 0} total messages";
        }
    }

    /// <summary>
    /// Concrete implementation of <see cref="IOcApiResult{T}"/>.
    /// </summary>
    /// <typeparam name="TData">Type of data returned by the operation.</typeparam>
    [PublicAPI]
    public class OcApiResult<TData> : OcApiResult, IOcApiResult<TData>
    {
      #region Constructors

        /// <summary>
        /// Constructs an instance of <see cref="IOcApiResult"/> for returning the results of an operation that returns data of type <typeparamref name="TData"/>.
        /// </summary>
        /// <param name="name">[Required] Name of the operation performed that these results are for</param>
        public OcApiResult([NotNull] string name)
            : base(name)
        { }

        /// <summary>
        /// Constructs an instance of <see cref="IOcApiResult"/> for returning the results of an operation that returns data of type <typeparamref name="TData"/>.
        /// </summary>
        /// <param name="name">[Required] Name of the operation performed that these results are for</param>
        /// <param name="data">Optional data for assigning to <see cref="Data"/>.</param>
        public OcApiResult([NotNull] string name, TData data)
            : base(name)
        {
            Data = data;
        }

        /// <summary>
        /// Constructs an instance of <see cref="IOcApiResult"/> for returning the results of an operation that returns data of type <typeparamref name="TData"/>.
        /// </summary>
        /// <param name="name">[Required] Name of the operation performed that these results are for</param>
        /// <param name="messages">Optional collection of <see cref="IOcApiMessage"/> to be added to <see cref="OcApiResult.Messages"/>.</param>
        public OcApiResult([NotNull] string name, [CanBeNull] params IOcApiMessage[] messages)
            : base(name, messages)
        { }

        /// <summary>
        /// Constructs an instance of <see cref="IOcApiResult"/> for returning the results of an operation that returns data of type <typeparamref name="TData"/>.
        /// </summary>
        /// <param name="name">[Required] Name of the operation performed that these results are for</param>
        /// <param name="data">Optional data for assigning to <see cref="Data"/>.</param>
        /// <param name="messages">Optional collection of <see cref="IOcApiMessage"/> to be added to <see cref="OcApiResult.Messages"/>.</param>
        public OcApiResult([NotNull] string name, TData data, [CanBeNull] params IOcApiMessage[] messages)
            : base(name, messages)
        {
            Data = data;
        }

        /// <summary>
        /// Constructs an instance of <see cref="IOcApiResult"/> for returning the results of an operation that returns data of type <typeparamref name="TData"/>.
        /// </summary>
        /// <param name="name">[Required] Name of the operation performed that these results are for</param>
        /// <param name="messages">Optional collection of <see cref="IOcApiMessage"/> to be added to <see cref="OcApiResult.Messages"/>.</param>
        public OcApiResult([NotNull] string name, [CanBeNull] IEnumerable<IOcApiMessage> messages)
            : base(name, messages)
        { }

        /// <summary>
        /// Constructs an instance of <see cref="IOcApiResult"/> for returning the results of an operation that returns data of type <typeparamref name="TData"/>.
        /// </summary>
        /// <param name="name">[Required] Name of the operation performed that these results are for</param>
        /// <param name="data">Optional data for assigning to <see cref="Data"/>.</param>
        /// <param name="messages">Optional collection of <see cref="IOcApiMessage"/> to be added to <see cref="OcApiResult.Messages"/>.</param>
        public OcApiResult([NotNull] string name, TData data, [CanBeNull] IEnumerable<IOcApiMessage> messages)
            : base(name, messages)
        {
            Data = data;
        }

      #endregion

        /// <inheritdoc/>
        public TData Data { get; private set; }

        /// <summary>
        /// Assigns <paramref name="data"/> to <see cref="Data"/>.
        /// </summary>
        public virtual void AssignData(TData data)
        {
            Data = data;
        }
    }
}
