//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace OriginalCoder.Common.Configuration
{
    /// <summary>
    /// Specifies default values for date and time operations
    /// </summary>
    [PublicAPI]
    public static class OcDateTimeConstants
    {
        // ReSharper disable StringLiteralTypo

      #region For data storage output

        /// <summary>
        /// Default format string used for output of Dates.
        /// </summary>
        public static readonly string StorageDateFormat = "yyyy-mm-dd";

        /// <summary>
        /// Default format string used for output of Time data with full accuracy and explicitly specifying the time zone.
        /// </summary>
        public static readonly string StorageTimeFormat = "hh:mm:ss.FFFFFFFK";

        /// <summary>
        /// Default format string used for output of both a Date and Time with full accuracy and explicitly specifying the time zone.
        /// </summary>
        public static readonly string StorageDateTimeFormat = StorageDateFormat + " " + StorageTimeFormat;

      #endregion

      #region For input (parsing)

        /// <summary>
        /// Default list of date formats to use when attempting to parse a string into a DateTime that only contains date info.
        /// NOTE: Since it is often impossible to differentiate mm/dd and dd/mm the only unambiguous common date format is year/month/date.
        /// </summary>
        public static readonly IReadOnlyList<string> ParserDateFormats = new[]
        {
            "yyyy-mm-dd",
            "yyyy/mm/dd",
        };

        /// <summary>
        /// Default list of time formats to use when attempting to parse a string into a DateTime that only contains time of day info.
        /// NOTE: This list has been specifically chosen and ordered to optimize successful
        ///       parsing of various formatting variants with minimal risk of parsing the data incorrectly.
        /// </summary>
        public static readonly IReadOnlyList<string> ParserTimeFormats = new[]
        {
            "h:m:s.FFFFFFFttK", // 12 Hour AM/PM - with Seconds and optional Fractions - with Kind (time zone info)
            "h:m:s.FFFFFFFtK", // 12 Hour A/P - with Seconds and optional Fractions - with Kind (time zone info)
            "H:m:s.FFFFFFFK", // 24 Hour - with Seconds and optional Fractions - with Kind (time zone info)

            "h:mttK", // 12 Hour AM/PM - no Seconds - with Kind (time zone info)
            "h:mtK", // 12 Hour A/P - no Seconds - with Kind (time zone info)
            "H:mK", // 24 Hour - no Seconds - with Kind (time zone info)

            "h:m:s.FFFFFFFtt", // 12 Hour AM/PM - with Seconds and optional Fractions - no time zone
            "h:m:s.FFFFFFFt", // 12 Hour A/P - with Seconds and optional Fractions - no time zone
            "H:m:s.FFFFFFF", // 24 Hour - with Seconds and optional Fractions - no time zone

            "h:mtt", // 12 Hour AM/PM - no Seconds - no time zone
            "h:mt", // 12 Hour A/P - no Seconds - no time zone
            "H:m", // 24 Hour - no Seconds - no UTC offset
        };

        /// <summary>
        /// Default list of date and time formats to use when attempting to parse a string into a DateTime.
        /// NOTE: This list has been specifically chosen and ordered to optimize successful
        ///       parsing of various formatting variants with minimal risk of parsing the data incorrectly.
        /// </summary>
        public static readonly IReadOnlyList<string> ParserDateTimeFormats = ParserTimeFormats.Select(f => StorageDateFormat + " " + f).ToList();

      #endregion

        // ReSharper restore StringLiteralTypo
    }
}