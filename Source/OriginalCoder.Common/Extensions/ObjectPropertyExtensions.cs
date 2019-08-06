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
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using OriginalCoder.Common.Interfaces.Properties;

namespace OriginalCoder.Common.Extensions
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

            return data.PropertyValueGet("Name")?.ToString() ?? data.PropertyValueGet("_name")?.ToString();
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

            name = data.PropertyValueGet("Name")?.ToString() ?? data.PropertyValueGet("_name")?.ToString();
            return name != null;
        }

        /// <summary>
        /// Returns either the name associated with the object (if available) or the type name.
        /// </summary>
        public static string GetNameOrType([NotNull] this object data)
        {
            if (TryGetName(data, out var name))
                return name;
            return data.GetType().FriendlyName();
        }

        /// <summary>
        /// Returns the best string available that describes <paramref name="data"/>.
        /// Options are searched in this order: <see cref="IStatusSummary"/>, <see cref="ISummary"/>, <see cref="IDescription"/>, <see cref="IName"/> then <see cref="object.ToString"/>
        /// </summary>
        public static string GetBestString([NotNull] this object data)
        {
            Debug.Assert(data != null);
            return data.GetStatusSummary() ?? data.GetSummary() ?? data.GetDescription() ?? data.GetName() ?? data.ToString();
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

      #region Properties

        /// <properties>
        /// Attempts to obtain the Properties associated with the object.
        /// Null is returned if the object does not implement the standard <see cref="IProperties"/> interface.
        /// </properties>
        public static IReadOnlyDictionary<string, object> GetProperties([NotNull] this object data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IProperties asProperties)
                return asProperties.Properties;
            return null;
        }

        /// <properties>
        /// Attempts to obtain the Properties associated with the object.
        /// If available <paramref name="properties"/> will be populated and true will be returned. 
        /// False is returned and the value of <paramref name="properties"/> is invalid if the object does not implement the standard <see cref="IProperties"/> interface.
        /// </properties>
        public static bool TryGetProperties([NotNull] this object data, out IReadOnlyDictionary<string, object> properties)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IProperties asProperties)
            {
                properties = asProperties.Properties;
                return true;
            }

            properties = null;
            return false;
        }

      #endregion

      #region Status Summary

        /// <statusStatusSummary>
        /// Attempts to obtain the StatusSummary associated with the object.
        /// Null is returned if the object does not implement the standard <see cref="IStatusSummary"/> interface.
        /// </statusStatusSummary>
        public static string GetStatusSummary([NotNull] this object data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IStatusSummary asStatusSummary)
                return asStatusSummary.StatusSummary;
            return null;
        }

        /// <statusStatusSummary>
        /// Attempts to obtain the StatusSummary associated with the object.
        /// If available <paramref name="statusStatusSummary"/> will be populated and true will be returned. 
        /// False is returned and the value of <paramref name="statusStatusSummary"/> is invalid if the object does not implement the standard <see cref="IStatusSummary"/> interface.
        /// </statusStatusSummary>
        public static bool TryGetStatusSummary([NotNull] this object data, out string statusStatusSummary)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IStatusSummary asStatusSummary)
            {
                statusStatusSummary = asStatusSummary.StatusSummary;
                return true;
            }

            statusStatusSummary = null;
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
