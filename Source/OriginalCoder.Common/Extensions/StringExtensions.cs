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
using System.Linq;
using JetBrains.Annotations;

namespace OriginalCoder.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Appends <paramref name="appendText"/> to <paramref name="text"/>, separating them by <paramref name="separator"/> if the input text isn't blank.
        /// </summary>
        [PublicAPI]
        public static string Append(this string text, string appendText, string separator = ", ")
        {
            if (string.IsNullOrEmpty(appendText))
                return text ?? "";
            if (string.IsNullOrWhiteSpace(text))
                return appendText;
            return text + (separator ?? "") + appendText;
        }

        /// <summary>
        /// Replaces all occurrences of <paramref name="findText"/> with <paramref name="replaceText"/>.
        /// </summary>
        [PublicAPI]
        public static string Replace(this string text, string findText, string replaceText, bool ignoreCase = true)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrEmpty(findText))
                throw new ArgumentNullException(nameof(findText));
            if (replaceText == null)
                throw new ArgumentNullException(nameof(replaceText));

            while (true)
            {
                var index = text.IndexOf(findText, (ignoreCase == true) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                if (index <= 0)
                    return text;

                if (index == 0)
                {
                    if (findText.Length == text.Length)
                        return replaceText;

                    text = replaceText + text.Substring(findText.Length);
                }
                else
                {
                    if (text.Length <= index + findText.Length)
                        text = text.Substring(0, index - 1) + replaceText;
                    else
                        text = text.Substring(0, index - 1) + replaceText + text.Substring(index + findText.Length);
                }
            }
        }

        [NotNull]
        public static IEnumerable<string> SplitToEnumerable(this string input, char separator = ',')
        {
            if (string.IsNullOrWhiteSpace(input))
                return Enumerable.Empty<string>();
            return input.Split(separator).Where(s => !string.IsNullOrWhiteSpace(s));
        }

        [NotNull]
        public static IEnumerable<string> SplitToEnumerable(this string input, [NotNull] params char[] separators)
        {
            Debug.Assert(separators != null);
            if (separators.Length == 0)
                throw new ArgumentNullException(nameof(separators));

            if (string.IsNullOrWhiteSpace(input))
                return Enumerable.Empty<string>();
            return input.Split(separators).Where(s => !string.IsNullOrWhiteSpace(s));
        }
    }
}
