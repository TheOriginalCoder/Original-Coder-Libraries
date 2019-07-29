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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;

namespace OriginalCoder.Common.Configuration
{
    /// <summary>
    /// Concrete implementation of <see cref="IDateTimeConfiguration"/>.
    /// Default configuration options are specified by the static <see cref="GlobalDefaults"/> property.
    /// </summary>
    [PublicAPI]
    public class DateTimeConfiguration : IDateTimeConfiguration
    {
      #region Global Configuration (defaults used by extension methods)

        /// <summary>
        /// Specifies the <see cref="IDateTimeConfiguration"/> that will be used by
        /// default for operations that don't specify an <see cref="IDateTimeConfiguration"/>.
        /// </summary>
        public static IDateTimeConfiguration GlobalDefaults { get; private set; } = new DateTimeConfiguration();

        /// <summary>
        /// Should be called ONCE during program startup and initialization to specify
        /// the date & time configuration options that should be used by default for the application.
        /// </summary>
        /// <param name="configuration"></param>
        public static void SetGlobalDefaults(IDateTimeConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (configuration != GlobalDefaults)
            {
                Debug.WriteLine($"GLOBAL-CONFIG: Changing {nameof(DateTimeConfiguration)}.{nameof(GlobalDefaults)} to {configuration}");
                GlobalDefaults = configuration;
            }
        }

      #endregion

      #region Constructors

        /// <summary>
        /// Instantiates an object that implements <see cref="IDateTimeConfiguration"/> and uses the defaults built into the OriginalCoder library.
        /// </summary>
        public DateTimeConfiguration()
            : this(
                CultureInfo.CurrentUICulture, 
                CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern, 
                CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern, 
                CultureInfo.CurrentUICulture.DateTimeFormat.FullDateTimePattern, 
                CultureInfo.InvariantCulture, 
                DateTimeConstants.StorageDateFormat, 
                DateTimeConstants.StorageTimeFormat, 
                DateTimeConstants.StorageDateTimeFormat, 
                new List<IFormatProvider> {CultureInfo.InvariantCulture}, 
                DateTimeConstants.ParserDateFormats, 
                DateTimeConstants.ParserTimeFormats, 
                DateTimeConstants.ParserDateTimeFormats
            )
        { }

        /// <summary>
        /// Instantiates an object that implements <see cref="IDateTimeConfiguration"/> with a custom configuration.
        /// </summary>
        public DateTimeConfiguration(
            IFormatProvider displayCulture, 
            string displayDateFormat, 
            string displayTimeFormat, 
            string displayDateTimeFormat, 
            IFormatProvider storageCulture, 
            string storageDateFormat, 
            string storageTimeFormat, 
            string storageDateTimeFormat,
            IEnumerable<IFormatProvider> inputCultures,
            IEnumerable<string> inputDateFormats,
            IEnumerable<string> inputTimeFormats,
            IEnumerable<string> inputDateTimeFormats)
        {
            // Configuration for display output
            if (displayCulture == null)
                throw new ArgumentException($"{nameof(displayCulture)} must contain a value", nameof(displayCulture));
            DisplayCulture = displayCulture;

            if (string.IsNullOrWhiteSpace(displayDateFormat))
                throw new ArgumentException($"{nameof(displayDateFormat)} must contain a value", nameof(displayDateFormat));
            DisplayDateFormat = displayDateFormat;

            if (string.IsNullOrWhiteSpace(displayTimeFormat))
                throw new ArgumentException($"{nameof(displayTimeFormat)} must contain a value", nameof(displayTimeFormat));
            DisplayTimeFormat = displayTimeFormat;

            if (string.IsNullOrWhiteSpace(displayDateTimeFormat))
                throw new ArgumentException($"{nameof(displayDateTimeFormat)} must contain a value", nameof(displayDateTimeFormat));
            DisplayDateTimeFormat = displayDateTimeFormat;

            // Configuration for storage output
            if (storageCulture == null)
                throw new ArgumentException($"{nameof(storageCulture)} must contain a value", nameof(storageCulture));
            StorageCulture = storageCulture;

            if (string.IsNullOrWhiteSpace(storageDateFormat))
                throw new ArgumentException($"{nameof(storageDateFormat)} must contain a value", nameof(storageDateFormat));
            StorageDateFormat = storageDateFormat;

            if (string.IsNullOrWhiteSpace(storageTimeFormat))
                throw new ArgumentException($"{nameof(storageTimeFormat)} must contain a value", nameof(storageTimeFormat));
            StorageTimeFormat = storageTimeFormat;

            if (string.IsNullOrWhiteSpace(storageDateTimeFormat))
                throw new ArgumentException($"{nameof(storageDateTimeFormat)} must contain a value", nameof(storageDateTimeFormat));
            StorageDateTimeFormat = storageDateTimeFormat;

            // Configuration for input (parsing) from storage
            var cultureList = inputCultures?.ToList();
            if (cultureList == null || cultureList.Count == 0)
                throw new ArgumentException($"{nameof(inputCultures)} must contain at least one value", nameof(inputCultures));
            InputCultures = cultureList;

            var stringArray = inputDateFormats?.ToArray();
            if (stringArray == null || stringArray.Length == 0)
                throw new ArgumentException($"{nameof(inputCultures)} must contain at least one value", nameof(inputCultures));
            InputDateFormats = stringArray;

            stringArray = inputTimeFormats?.ToArray();
            if (stringArray == null || stringArray.Length == 0)
                throw new ArgumentException($"{nameof(inputCultures)} must contain at least one value", nameof(inputCultures));
            InputTimeFormats = stringArray;

            stringArray = inputDateTimeFormats?.ToArray();
            if (stringArray == null || stringArray.Length == 0)
                throw new ArgumentException($"{nameof(inputCultures)} must contain at least one value", nameof(inputCultures));
            InputDateTimeFormats = stringArray;
        }

      #endregion

      #region Configuration for display to end users

        /// <inheritdoc />
        public IFormatProvider DisplayCulture { get; }

        /// <inheritdoc />
        public string DisplayDateFormat { get; }

        /// <inheritdoc />
        public string DisplayTimeFormat { get; }

        /// <inheritdoc />
        public string DisplayDateTimeFormat { get; }

      #endregion

      #region Configuration for data storage output

        /// <inheritdoc />
        public IFormatProvider StorageCulture { get; }

        /// <inheritdoc />
        public string StorageDateFormat { get; }

        /// <inheritdoc />
        public string StorageTimeFormat { get; }

        /// <inheritdoc />
        public string StorageDateTimeFormat { get; }

      #endregion

      #region Configuration for input (parsing) from data storage

        /// <inheritdoc />
        public IReadOnlyList<IFormatProvider> InputCultures { get; }

        /// <inheritdoc />
        public string[] InputDateFormats { get; }

        /// <inheritdoc />
        public string[] InputTimeFormats { get; }

        /// <inheritdoc />
        public string[] InputDateTimeFormats { get; }

      #endregion
    }
}