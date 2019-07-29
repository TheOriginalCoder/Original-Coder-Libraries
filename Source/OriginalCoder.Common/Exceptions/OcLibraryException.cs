//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2013, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;

namespace OriginalCoder.Common.Exceptions
{
    /// <summary>
    /// Base class for Exceptions related to the Original Coder libraries.
    /// </summary>
    [PublicAPI]
    public class OcLibraryException : OcException
    {
      #region Constructors 

        public OcLibraryException(string message)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Library Error" : message)
        { }

        public OcLibraryException(System.Exception exception)
            : base("Unspecified Library Error", exception)
        { }

        public OcLibraryException(string message, System.Exception exception)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Library Error" : message, exception)
        { }

      #endregion
    }
}