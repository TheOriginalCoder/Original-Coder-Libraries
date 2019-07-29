//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2013, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using JetBrains.Annotations;
using OriginalCoder.Common.Interface.Property;

namespace OriginalCoder.Common.Extension
{
    /// <summary>
    /// Extension methods for obtaining information about objects using standard object property interfaces.
    /// </summary>
    [PublicAPI]
    public static class ObjectPropertyExtensions
    {
      #region Name

        /// <summary>
        /// Attempts to obtain the Name associated with the object.
        /// Null is returned if the object does not implement the standard <see cref="IName"/> interface.
        /// </summary>
        public static string GetName([NotNull] this object data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IName asName)
                return asName.Name;
            return null;
        }

        /// <summary>
        /// Attempts to obtain the Name associated with the object.
        /// If available <paramref name="name"/> will be populated and true will be returned. 
        /// False is returned and the value of <paramref name="name"/> is invalid if the object does not implement the standard <see cref="IName"/> interface.
        /// </summary>
        public static bool TryGetName([NotNull] this object data, out string name)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IName asName)
            {
                name = asName.Name;
                return true;
            }

            name = null;
            return false;
        }

      #endregion

      #region Description

        /// <summary>
        /// Attempts to obtain the Description associated with the object.
        /// Null is returned if the object does not implement the standard <see cref="IDescription"/> interface.
        /// </summary>
        public static string GetDescription([NotNull] this object data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IDescription asDescription)
                return asDescription.Description;
            return null;
        }

        /// <summary>
        /// Attempts to obtain the Description associated with the object.
        /// If available <paramref name="description"/> will be populated and true will be returned. 
        /// False is returned and the value of <paramref name="description"/> is invalid if the object does not implement the standard <see cref="IDescription"/> interface.
        /// </summary>
        public static bool TryGetDescription([NotNull] this object data, out string description)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IDescription asDescription)
            {
                description = asDescription.Description;
                return true;
            }

            description = null;
            return false;
        }

      #endregion

      #region Summary

        /// <summary>
        /// Attempts to obtain the Summary associated with the object.
        /// Null is returned if the object does not implement the standard <see cref="ISummary"/> interface.
        /// </summary>
        public static string GetSummary([NotNull] this object data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is ISummary asSummary)
                return asSummary.Summary;
            return null;
        }

        /// <summary>
        /// Attempts to obtain the Summary associated with the object.
        /// If available <paramref name="summary"/> will be populated and true will be returned. 
        /// False is returned and the value of <paramref name="summary"/> is invalid if the object does not implement the standard <see cref="ISummary"/> interface.
        /// </summary>
        public static bool TryGetSummary([NotNull] this object data, out string summary)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is ISummary asSummary)
            {
                summary = asSummary.Summary;
                return true;
            }

            summary = null;
            return false;
        }

      #endregion
    }
}
