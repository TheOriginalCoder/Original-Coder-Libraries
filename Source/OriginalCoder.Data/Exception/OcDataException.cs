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
using OriginalCoder.Common;
using OriginalCoder.Common.Exceptions;

namespace OriginalCoder.Data.Exception
{
    [PublicAPI]
    public class OcDataException : OcLibraryException
    {
      #region Constructors 

        public OcDataException(string message)
            : base(message)
        { }

        public OcDataException(System.Exception exception)
            : base("Unspecified Data Error", exception)
        { }

        public OcDataException(string message, System.Exception exception)
            : base(message, exception)
        { }

      #endregion
    }
}