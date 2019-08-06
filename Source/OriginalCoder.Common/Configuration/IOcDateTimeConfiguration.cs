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

namespace OriginalCoder.Common.Configuration
{
    /// <summary>
    /// Specifies configuration information for converting dates and/or times to and from text.
    /// </summary>
    [PublicAPI]
    public interface IOcDateTimeConfiguration
    {
      #region For display to end users

        /// <summary>
        /// Culture to use when converting dates and/or times into text for display to users.
        /// </summary>
        IFormatProvider DisplayCulture { get; }

        /// <summary>
        /// Format to use when converting a date into text for display to users.
        /// </summary>
        string DisplayDateFormat { get; }

        /// <summary>
        /// Format to use when converting a time into text for display to users.
        /// </summary>
        string DisplayTimeFormat { get; }

        /// <summary>
        /// Format to use when converting a date and time into text for display to users.
        /// </summary>
        string DisplayDateTimeFormat { get; }

      #endregion

      #region For data storage output

        /// <summary>
        /// Culture to use when converting dates and/or times into text for storage or record keeping.
        /// </summary>
        IFormatProvider StorageCulture { get; }

        /// <summary>
        /// Format to use when converting a date into text for storage or record keeping.
        /// </summary>
        string StorageDateFormat { get; }

        /// <summary>
        /// Format to use when converting a time into text for storage or record keeping.
        /// </summary>
        string StorageTimeFormat { get; }

        /// <summary>
        /// Format to use when converting a date and time into text for storage or record keeping.
        /// </summary>
        string StorageDateTimeFormat { get; }

      #endregion

      #region Configuration for input (parsing)

        /// <summary>
        /// One or more <see cref="IFormatProvider"/> that are used, in order, when attempting to parse text into dates and/or times.
        /// </summary>
        IReadOnlyList<IFormatProvider> InputCultures { get; }

        /// <summary>
        /// One or more DateTime formats that are used, in order, when attempting to parse text into dates.
        /// </summary>
        string[] InputDateFormats { get; }

        /// <summary>
        /// One or more DateTime formats that are used, in order, when attempting to parse text into times.
        /// </summary>
        string[] InputTimeFormats { get; }

        /// <summary>
        /// One or more DateTime formats that are used, in order, when attempting to parse text into dates and times.
        /// </summary>
        string[] InputDateTimeFormats { get; }

      #endregion
    }
}