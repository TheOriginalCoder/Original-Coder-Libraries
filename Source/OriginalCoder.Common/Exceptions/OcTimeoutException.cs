//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2014, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;

namespace OriginalCoder.Common.Exceptions
{
    /// <summary>
    /// Base class for Timeout Exceptions thrown by Original Coder libraries.
    /// </summary>
    [PublicAPI]
    public class OcTimeoutException : OcApplicationException
    {
      #region Constructors 

        public OcTimeoutException(string message)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Timeout" : message)
        { }

        public OcTimeoutException(System.Exception exception)
            : base("Unspecified Timeout", exception)
        { }

        public OcTimeoutException(string message, System.Exception exception)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Timeout" : message, exception)
        { }

      #endregion
    }
}