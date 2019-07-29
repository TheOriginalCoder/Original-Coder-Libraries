//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2010, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;

namespace OriginalCoder.Common.Exceptions
{
    /// <summary>
    /// Original Coder base class for Abort Exceptions.
    /// </summary>
    [PublicAPI]
    public class OcAbortException : OcApplicationException
    {
      #region Constructors 

        public OcAbortException(string message)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Abort" : message)
        { }

        public OcAbortException(System.Exception exception)
            : base("Unspecified Abort", exception)
        { }

        public OcAbortException(string message, System.Exception exception)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Abort" : message, exception)
        { }

      #endregion
    }
}