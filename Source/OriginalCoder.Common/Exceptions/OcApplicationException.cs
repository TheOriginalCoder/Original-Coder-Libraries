//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2009, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using JetBrains.Annotations;

namespace OriginalCoder.Common.Exceptions
{
    /// <summary>
    /// Original Coder base class for Application Exceptions.
    /// </summary>
    [PublicAPI]
    public class OcApplicationException : ApplicationException
    {
      #region Constructors 

        public OcApplicationException(string message)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Application Error" : message)
        { }

        public OcApplicationException(Exception exception)
            : base("Unspecified Application Error", exception)
        { }

        public OcApplicationException(string message, Exception exception)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Application Error" : message, exception)
        { }

      #endregion

        public override string ToString()
        {
            return OcException.HierarchyToString(this);
        }
    }
}