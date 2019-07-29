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
    /// Base class for Original Coder Exceptions (other than <see cref="OcApplicationException"/>).
    /// </summary>
    [PublicAPI]
    public class OcException : Exception
    {
      #region Constructors 

        public OcException(string message)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Error" : message)
        { }

        public OcException(Exception exception)
            : base("Unspecified Error", exception)
        { }

        public OcException(string message, Exception exception)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Error" : message, exception)
        { }

      #endregion

      #region ToString

        public static string ExceptionToString(Exception ex)
        {
            if (ex == null)
                return "";

            if (ex is AggregateException asAggregate)
            {
                return $"Aggregate containing {asAggregate.InnerExceptions.Count} exceptions";
            }
            return $"{ex.GetType().Name}: {ex.Message}";
        }

        public static string HierarchyToString(Exception ex)
        {
            if (ex == null)
                return "";

            var result = ExceptionToString(ex);
            var next = ex.InnerException;
            while (next != null)
            {
                result = result + " | INNER " + ExceptionToString(ex);
                next = next.InnerException;
            }
            return result;
        }

        public override string ToString()
        {
            return HierarchyToString(this);
        }

      #endregion
    }
}
