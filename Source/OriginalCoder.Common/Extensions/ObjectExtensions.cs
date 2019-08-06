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
using System.Reflection;
using JetBrains.Annotations;

namespace OriginalCoder.Common.Extensions
{
    [PublicAPI]
    public static class ObjectExtensions
    {
        public static object PropertyValueGet([NotNull] this object data, [NotNull] string propertyName)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            propertyName = propertyName.Trim();

            var dataType = data.GetType();
            var property = dataType.GetProperty(propertyName, BindingFlags.IgnoreCase.Add(BindingFlags.FlattenHierarchy));
            if (property == null ||   !property.CanRead)
                return null;

            return property.GetValue(data);
        }

        public static bool TryPropertyValueGet<T>([NotNull] this object data, [NotNull] string propertyName, out T value)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            propertyName = propertyName.Trim();

            var dataType = data.GetType();
            var property = dataType.GetProperty(propertyName, BindingFlags.IgnoreCase.Add(BindingFlags.FlattenHierarchy));
            if (property == null || !property.CanRead || !typeof(T).IsAssignableFrom(property.PropertyType))
            {
                value = default;
                return false;
            }

            value = (T)property.GetValue(data);
            return true;
        }

        public static bool PropertyValueSet([NotNull] this object data, [NotNull] string propertyName, object value)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            propertyName = propertyName.Trim();

            var dataType = data.GetType();
            var property = dataType.GetProperty(propertyName, BindingFlags.IgnoreCase.Add(BindingFlags.FlattenHierarchy));
            if (property == null || !property.CanWrite)
                return false;

            try
            {
                property.SetValue(data, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}