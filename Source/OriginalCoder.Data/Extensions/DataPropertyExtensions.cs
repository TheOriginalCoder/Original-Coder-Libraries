//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2013, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using JetBrains.Annotations;
using OriginalCoder.Data.Interfaces.Properties;

namespace OriginalCoder.Data.Extensions
{
    /// <summary>
    /// Extension methods for obtaining information about objects using standard data property interfaces.
    /// </summary>
    [PublicAPI]
    public static class DataPropertyExtensions
    {
      #region When Created

        /// <summary>
        /// Attempts to obtain when the object was created.
        /// Null is returned if the object does not implement a standard interface for creation date.
        /// </summary>
        public static DateTime? GetWhenCreated([NotNull] this object data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IWhenCreated asWhenCreated)
                return asWhenCreated.WhenCreated;
            return null;
        }

        /// <summary>
        /// Attempts to obtain when the object was created.
        /// If available <paramref name="when"/> will be populated and true will be returned. 
        /// False is returned and the value of <paramref name="when"/> is invalid if the object does not implement a standard interface for indicating when it was created.
        /// </summary>
        public static bool TryGetWhenCreated([NotNull] this object data, out DateTime when)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IWhenCreated asWhenCreated)
            {
                when = asWhenCreated.WhenCreated;
                return true;
            }

            when = default;
            return false;
        }

      #endregion

      #region When Updated

        /// <summary>
        /// Attempts to obtain when the object was last updated.
        /// If available <paramref name="when"/> will be populated and true will be returned.
        /// If true is returned and <paramref name="when"/> is null the object has not been updated since being created.
        /// False is returned and the value of <paramref name="when"/> is invalid if the object does not implement a standard interface for indicating when it was updated.
        /// </summary>
        public static bool TryGetWhenUpdated([NotNull] this object data, out DateTime? when)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IWhenUpdated asWhenUpdated)
            {
                when = asWhenUpdated.WhenUpdated;
                return true;
            }

            when = null;
            return false;
        }

      #endregion

      #region When Deleted

        /// <summary>
        /// Attempts to obtain when the object was marked as deleted.
        /// If available <paramref name="when"/> will be populated and true will be returned.
        /// If true is returned and <paramref name="when"/> is null the object has not been marked as deleted.
        /// False is returned and the value of <paramref name="when"/> is invalid if the object does not implement a standard interface for indicating when it was deleted.
        /// </summary>
        public static bool TryGetWhenDeleted([NotNull] this object data, out DateTime? when)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IWhenDeleted asWhenDeleted)
            {
                when = asWhenDeleted.WhenDeleted;
                return true;
            }

            when = null;
            return false;
        }

      #endregion

      #region Is Deleted

        /// <summary>
        /// Attempts to determine if the object has been marked as deleted.
        /// Null is returned if the object does not implement a standard interface for deleted status.
        /// </summary>
        public static bool? GetIsDeleted([NotNull] this object data)
        {
            if (TryGetIsDeleted(data, out var isDeleted))
                return isDeleted;
            return null;
        }

        /// <summary>
        /// Attempts to obtain if the object has been marked as deleted.
        /// If available <paramref name="isDeleted"/> will be populated and true will be returned.
        /// False is returned and the value of <paramref name="isDeleted"/> is invalid if the object does not implement a standard interface for indicating deletion status.
        /// </summary>
        public static bool TryGetIsDeleted([NotNull] this object data, out bool isDeleted)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.TryGetWhenDeleted(out var whenDeleted))
            {
                isDeleted = whenDeleted.HasValue;
                return true;
            }

            if (data is IIsActive asIsActive)
            {
                isDeleted = !asIsActive.IsActive;
                return true;
            }

            isDeleted = false;
            return false;
        }

      #endregion
    }
}