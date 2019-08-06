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
using OriginalCoder.Common.Extensions;

namespace OriginalCoder.Common
{
    [PublicAPI]
    public static class OcString
    {
        public static string Concatenate(params object[] items)
            => Concatenate(", ", items);

        public static string Concatenate(string separator, params object[] items)
        {
            var result = "";
            foreach (var item in items)
            {
                if (item != null)
                    result += (result.Length == 0 ? "" : separator) + item;
            }
            return result;
        }

        public static string Concatenate(IReadOnlyDictionary<string, object> properties, IReadOnlyList<(string name, object value)> items)
            => Concatenate(properties, ", ", items);

        public static string Concatenate(IReadOnlyDictionary<string, object> properties, string separator, IReadOnlyList<(string name, object value)> items)
        {
            if (items == null || items.Count == 0)
                return "";

            var result = "";
            foreach (var (name, value) in items.Where(i => !string.IsNullOrWhiteSpace(i.name)))
            {
                DoConcatenate(ref result, name, value, separator, properties);
            }
            return result;
        }

        public static string ConcatenateReverse(IReadOnlyDictionary<string, object> properties, IReadOnlyList<(string name, object value)> items)
            => ConcatenateReverse(properties, ", ", items);

        public static string ConcatenateReverse(IReadOnlyDictionary<string, object> properties, string separator, IReadOnlyList<(string name, object value)> items)
        {
            if (items == null || items.Count == 0)
                return "";

            var result = "";
            for (var i = items.Count-1; i >= 0; i--)
            {
                var (name, value) = items[i];
                DoConcatenate(ref result, name, value, separator, properties);
            }
            return result;
        }

        private static void DoConcatenate(ref string result, string name, object value, string separator, IReadOnlyDictionary<string, object> properties)
        {
            Debug.Assert(result != null);
            Debug.Assert(separator != null);
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (value == null && properties?.TryGetValue(name, out value) != true)
                return;

            if (!(value is string valueString))
                valueString = value.GetBestString();
            if (string.IsNullOrWhiteSpace(valueString))
                return;

            result += (result.Length == 0 ? "" : separator) + $"{name} [{valueString}]";
        }
    }
}