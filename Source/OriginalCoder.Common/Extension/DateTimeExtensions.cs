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
using System.Globalization;
using JetBrains.Annotations;
using OriginalCoder.Common.Configuration;

namespace OriginalCoder.Common.Extension
{
    [PublicAPI]
    public static class DateTimeExtensions
    {
      #region Output to display to user

        /// <summary>
        /// Converts a DateTime into text for displaying a date to the user.
        /// </summary>
        /// <param name="value">DateTime value to be converted into text.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A string containing the formatted date.</returns>
        public static string ToDisplayDate(this DateTime value, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            return value.ToLocalTime().ToString(configuration.DisplayDateFormat, configuration.DisplayCulture);
        }

        /// <summary>
        /// Converts a DateTime into text for displaying a time to the user.
        /// </summary>
        /// <param name="value">DateTime value to be converted into text.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A string containing the formatted time.</returns>
        public static string ToDisplayTime(this DateTime value, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            return value.ToLocalTime().ToString(configuration.DisplayTimeFormat, configuration.DisplayCulture);
        }

        /// <summary>
        /// Converts a DateTime into text for displaying a date and time to the user.
        /// </summary>
        /// <param name="value">DateTime value to be converted into text.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A string containing the formatted date and time.</returns>
        public static string ToDisplayDateTime(this DateTime value, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            return value.ToLocalTime().ToString(configuration.DisplayDateTimeFormat, configuration.DisplayCulture);
        }

      #endregion

      #region Output to storage

        /// <summary>
        /// Converts a DateTime into a text date format intended for writing to storage or sending to other systems.
        /// </summary>
        /// <param name="value">DateTime value to be converted into text.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A string containing the formatted date.</returns>
        public static string ToStorageDate(this DateTime value, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            return value.ToUniversalTime().ToString(configuration.StorageDateFormat, configuration.StorageCulture);
        }

        /// <summary>
        /// Converts a DateTime into a text time format intended for writing to storage or sending to other systems.
        /// </summary>
        /// <param name="value">DateTime value to be converted into text.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A string containing the formatted time.</returns>
        public static string ToStorageTime(this DateTime value, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            return value.ToUniversalTime().ToString(configuration.StorageTimeFormat, configuration.StorageCulture);
        }

        /// <summary>
        /// Converts a DateTime into a text date and time format intended for writing to storage or sending to other systems.
        /// </summary>
        /// <param name="value">DateTime value to be converted into text.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A string containing the formatted date and time.</returns>
        public static string ToStorageDateTime(this DateTime value, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            return value.ToUniversalTime().ToString(configuration.StorageDateTimeFormat, configuration.StorageCulture);
        }

      #endregion

      #region Parse text into a DateTime (text format should be similar to the storage foramt)

        /// <summary>
        /// Parses a string containing a date into a DateTime.
        /// Throws a <see cref="FormatException"/> if the text can not be successfully parsed.
        /// </summary>
        /// <param name="text">Text to be parsed.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A DateTime that only contains date information.</returns>
        public static DateTime ToDate(this string text, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            foreach (var culture in configuration.InputCultures)
                if (DateTime.TryParseExact(text, configuration.InputDateFormats, culture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault, out var value))
                    return value.Date;
            throw new FormatException($"Input text of [{text}] was not recognized as a valid date.");
        }

        /// <summary>
        /// Parses a string containing a time into a DateTime configured for the UTC time zone.
        /// Throws a <see cref="FormatException"/> if the text can not be successfully parsed.
        /// </summary>
        /// <param name="text">Text to be parsed.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A DateTime that only contains time information.</returns>
        public static DateTime ToTimeAsUtc(this string text, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            foreach (var culture in configuration.InputCultures)
                if (DateTime.TryParseExact(text, configuration.InputTimeFormats, culture, DateTimeStyles.AllowWhiteSpaces & DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var value))
                    return default(DateTime).Add(value.TimeOfDay);
            throw new FormatException($"Input text of [{text}] was not recognized as a valid universal (UTC) time.");
        }

        /// <summary>
        /// Parses a string containing a date and time into a DateTime configured for the UTC time zone.
        /// Throws a <see cref="FormatException"/> if the text can not be successfully parsed.
        /// </summary>
        /// <param name="text">Text to be parsed.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A DateTime that contains both date and time information.</returns>
        public static DateTime ToDateTimeAsUtc(this string text, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            foreach (var culture in configuration.InputCultures)
                if (DateTime.TryParseExact(text, configuration.InputDateTimeFormats, culture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var value))
                    return value;
            throw new FormatException($"Input text of [{text}] was not recognized as a valid date and universal (UTC) time.");
        }

        /// <summary>
        /// Parses a string containing a time into a DateTime configured for the local time zone.
        /// Throws a <see cref="FormatException"/> if the text can not be successfully parsed.
        /// </summary>
        /// <param name="text">Text to be parsed.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A DateTime that only contains time information.</returns>
        public static DateTime ToTimeAsLocal(this string text, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            foreach (var culture in configuration.InputCultures)
                if (DateTime.TryParseExact(text, configuration.InputTimeFormats, culture, DateTimeStyles.AllowWhiteSpaces & DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AssumeLocal, out var value))
                    return default(DateTime).Add(value.ToLocalTime().TimeOfDay);
            throw new FormatException($"Input text of [{text}] was not recognized as a valid local time.");
        }

        /// <summary>
        /// Parses a string containing a date and time into a DateTime configured for the local time zone.
        /// Throws a <see cref="FormatException"/> if the text can not be successfully parsed.
        /// </summary>
        /// <param name="text">Text to be parsed.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A DateTime that contains both date and time information.</returns>
        public static DateTime ToDateTimeAsLocal(this string text, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            foreach (var culture in configuration.InputCultures)
                if (DateTime.TryParseExact(text, configuration.InputDateTimeFormats, culture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault, out var value))
                    return value.ToLocalTime();
            throw new FormatException($"Input text of [{text}] was not recognized as a valid date and local time.");
        }

      #endregion

      #region Attempt parsing of text into a DateTime (text format should be similar to the storage foramt)

        /// <summary>
        /// Parses a string containing a date into a DateTime.
        /// Throws a <see cref="FormatException"/> if the text can not be successfully parsed.
        /// </summary>
        /// <param name="text">Text to be parsed.</param>
        /// <param name="result">If successful contains the parsed date and time.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A DateTime that only contains date information.</returns>
        public static bool TryToDate(this string text, out DateTime result, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            foreach (var culture in configuration.InputCultures)
            {
                if (DateTime.TryParseExact(text, configuration.InputDateFormats, culture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault, out var value))
                {
                    result = value.Date;
                    return true;
                }
            }
            result = default;
            return false;
        }

        /// <summary>
        /// Parses a string containing a time into a DateTime configured for the UTC time zone.
        /// Throws a <see cref="FormatException"/> if the text can not be successfully parsed.
        /// </summary>
        /// <param name="text">Text to be parsed.</param>
        /// <param name="result">If successful contains the parsed date and time.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A DateTime that only contains time information.</returns>
        public static bool TryToTimeAsUtc(this string text, out DateTime result, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            foreach (var culture in configuration.InputCultures)
            {
                if (DateTime.TryParseExact(text, configuration.InputTimeFormats, culture, DateTimeStyles.AllowWhiteSpaces & DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var value))
                {
                    result = default(DateTime).Add(value.TimeOfDay);
                    return true;
                }
            }
            result = default;
            return false;
        }

        /// <summary>
        /// Parses a string containing a date and time into a DateTime configured for the UTC time zone.
        /// Throws a <see cref="FormatException"/> if the text can not be successfully parsed.
        /// </summary>
        /// <param name="text">Text to be parsed.</param>
        /// <param name="result">If successful contains the parsed date and time.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A DateTime that contains both date and time information.</returns>
        public static bool TryToDateTimeAsUtc(this string text, out DateTime result, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            foreach (var culture in configuration.InputCultures)
            {
                if (DateTime.TryParseExact(text, configuration.InputDateTimeFormats, culture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var value))
                {
                    result = value;
                    return true;
                }
            }
            result = default;
            return false;
        }

        /// <summary>
        /// Parses a string containing a time into a DateTime configured for the local time zone.
        /// Throws a <see cref="FormatException"/> if the text can not be successfully parsed.
        /// </summary>
        /// <param name="text">Text to be parsed.</param>
        /// <param name="result">If successful contains the parsed date and time.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A DateTime that only contains time information.</returns>
        public static bool TryToTimeAsLocal(this string text, out DateTime result, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            foreach (var culture in configuration.InputCultures)
            {
                if (DateTime.TryParseExact(text, configuration.InputTimeFormats, culture, DateTimeStyles.AllowWhiteSpaces & DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AssumeLocal, out var value))
                {
                    result = default(DateTime).Add(value.ToLocalTime().TimeOfDay);
                    return true;
                }
            }
            result = default;
            return false;
        }

        /// <summary>
        /// Parses a string containing a date and time into a DateTime configured for the local time zone.
        /// Throws a <see cref="FormatException"/> if the text can not be successfully parsed.
        /// </summary>
        /// <param name="text">Text to be parsed.</param>
        /// <param name="result">If successful contains the parsed date and time.</param>
        /// <param name="configuration">Configuration options to use.  If null <see cref="DateTimeConfiguration.GlobalDefaults"/> will be used. </param>
        /// <returns>A DateTime that contains both date and time information.</returns>
        public static bool TryToDateTimeAsLocal(this string text, out DateTime result, IDateTimeConfiguration configuration = null)
        {
            configuration = configuration ?? DateTimeConfiguration.GlobalDefaults;
            foreach (var culture in configuration.InputCultures)
            {
                if (DateTime.TryParseExact(text, configuration.InputDateTimeFormats, culture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault, out var value))
                {
                    result = value.ToLocalTime();
                    return true;
                }
            }
            result = default;
            return false;
        }

      #endregion
    }
}
