//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2008, 2012, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;
using OriginalCoder.Common.Exceptions;

namespace OriginalCoder.Common.Extension
{
	public enum XmlLookupMode { ReadOptional, ReadRequired, CreateOrRead }

	/// <summary>
	/// Extension methods for working with LINQ to XML.
	/// </summary>
	[PublicAPI]
	public static class XmlLinqExtensions
	{
      #region XContainer Information

		/// <summary>
		/// Returns the name of the node or "DOCUMENT" for XDocuments.
		/// </summary>
		public static string GetName([NotNull] this XContainer node)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));

			return node is XDocument ? "DOCUMENT" : node is XElement element ? element.Name.ToString() : "";
		}

		/// <summary>
		/// Returns a string with the hierarchy of element names.
		/// </summary>
		public static string GetNameHierarchy([NotNull] this XContainer node)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));

			var result = "";
			do
			{
				result = (result.Length == 0) ? node.GetName() : node.GetName() + @"\" + result;
				node = node.Parent;
			} while (node != null);
			return result;
		}

      #endregion

      #region Find, Create & Remove (Elements & Attributes)

		/// <summary>
		/// Returns the child XML Element with matching <paramref name="name"/> if one exists.
		/// If an element with that name does not exist <paramref name="mode"/> determines the action taken.
		/// <see cref="XmlLookupMode.CreateOrRead"/> will create and return a new (empty) XML Element if one does not exist.
		/// <see cref="XmlLookupMode.ReadRequired"/> will throw an exception if an element is not found.
		/// </summary>
		public static XElement GetElement([NotNull] this XContainer node, [NotNull] string name, XmlLookupMode mode = XmlLookupMode.ReadOptional)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			var element = node.Element(name);
			if (element != null)
				return element;

			if (mode == XmlLookupMode.CreateOrRead)
			{
				node.Add(element = new XElement(name));
				return element;
			}

			if (mode == XmlLookupMode.ReadRequired)
				throw new OcException("Required XML Element [" + name + "] not found.");

			return null;
		}

        /// <summary>
        /// Removes all child elements with <paramref name="name"/>.
        /// </summary>
        /// <param name="node">XML node to remove children from.</param>
        /// <param name="name">Name of elements to be removed.</param>
	    public static void RemoveElements([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        node.Elements(name).Remove();
	    }

	    /// <summary>
	    /// Returns the child XML Attribute with matching <paramref name="name"/> if one exists.
	    /// If an attribute with that name does not exist <paramref name="mode"/> determines the action taken.
	    /// <see cref="XmlLookupMode.CreateOrRead"/> will create and return a new (empty) XML Attribute if one does not exist.
	    /// <see cref="XmlLookupMode.ReadRequired"/> will throw an exception if an attribute is not found.
	    /// </summary>
	    public static XAttribute GetAttribute([NotNull] this XElement node, [NotNull] string name, XmlLookupMode mode = XmlLookupMode.ReadOptional)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

            var attribute = node.Attribute(name);
	        if (attribute != null)
	            return attribute;


	        if (mode == XmlLookupMode.CreateOrRead)
	        {
	            node.Add(attribute = new XAttribute(name, ""));
	            return attribute;
	        }

	        if (mode == XmlLookupMode.ReadRequired)
	            throw new OcException("Required XML Attribute [" + name + "] not found.");

	        return null;
	    }

	    /// <summary>
	    /// Removes all child elements with <paramref name="name"/>.
	    /// </summary>
	    /// <param name="node">XML node to remove children from.</param>
	    /// <param name="name">Name of elements to be removed.</param>
	    public static void RemoveAttributes([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        node.Attributes(name).Remove();
	    }

      #endregion

      #region Element - Get Value (single)

	    public static bool? GetElementBool([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetElementString(name);
	        return (text != null && bool.TryParse(text, out var value)) ? value : (bool?) null;
	    }

		public static bool GetElementBool([NotNull] this XElement node, [NotNull] string name, bool defaultValue)
		{
		    return GetElementBool(node, name) ?? defaultValue;
		}

	    public static byte? GetElementByte([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetElementString(name);
	        return (text != null && byte.TryParse(text, out var value)) ? value : (byte?) null;
	    }

	    public static byte GetElementByte([NotNull] this XElement node, [NotNull] string name, byte defaultValue)
	    {
	        return GetElementByte(node, name) ?? defaultValue;
	    }

	    public static Color? GetElementColor([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        var valueElement = node.GetElement(name);
	        var r = valueElement.GetAttributeInt("Red", 0);
	        var g = valueElement.GetAttributeInt("Green", 0);
	        var b = valueElement.GetAttributeInt("Blue", 0);
	        var a = valueElement.GetAttributeInt("Alpha", 0);
            if (a < 0 || a > 255 || r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
	            return null;
	        return Color.FromArgb(a, r, g, b);
	    }

	    public static Color GetElementColor([NotNull] this XElement node, [NotNull] string name, Color defaultValue)
	    {
	        return GetElementColor(node, name) ?? defaultValue;
	    }

	    public static DateTime? GetElementDate(this XElement node, string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        var text = node.GetElementString(name);
	        return text != null && text.TryToDate(out var value) ? value : (DateTime?) null;
	    }

		public static DateTime GetElementDate([NotNull] this XElement node, [NotNull] string name, DateTime defaultValue)
		{
		    return GetElementDate(node, name) ?? defaultValue;
		}

	    public static DateTime? GetElementTime([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        var text = node.GetElementString(name);
	        return text != null && text.TryToTimeAsUtc(out var value) ? value : (DateTime?) null;
	    }

		public static DateTime GetElementTime([NotNull] this XElement node, [NotNull] string name, DateTime defaultValue)
		{
		    return GetElementTime(node, name) ?? defaultValue;
		}

	    public static DateTime? GetElementDateTime([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        var text = node.GetElementString(name);
	        return text != null && text.TryToDateTimeAsUtc(out var value) ? value : (DateTime?) null;
	    }

	    public static DateTime GetElementDateTime([NotNull] this XElement node, [NotNull] string name, DateTime defaultValue)
	    {
	        return GetElementDateTime(node, name) ?? defaultValue;
	    }

	    public static decimal? GetElementDecimal([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetElementString(name);
	        return (text != null && decimal.TryParse(text, out var value)) ? value : (decimal?) null;
	    }

	    public static decimal GetElementDecimal([NotNull] this XElement node, [NotNull] string name, decimal defaultValue)
	    {
	        return GetElementInt(node, name) ?? defaultValue;
	    }

	    public static double? GetElementDouble([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetElementString(name);
	        return (text != null && double.TryParse(text, out var value)) ? value : (double?) null;
	    }

	    public static double GetElementDouble([NotNull] this XElement node, [NotNull] string name, double defaultValue)
	    {
	        return GetElementInt(node, name) ?? defaultValue;
	    }

	    public static T? GetElementEnum<T>([NotNull] this XElement node, [NotNull] string name) where T : struct, IConvertible
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        if (!typeof(T).IsEnum)
	            throw new ArgumentException("T in SetElementValue<T> must be an enum.");
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        var text = node.GetElementString(name);
	        return text != null && Enum.TryParse(text, true, out T result) ? result : (T?)null;
	    }

	    public static T GetElementEnum<T>([NotNull] this XElement node, [NotNull] string name, T defaultValue) where T : struct, IConvertible
	    {
	        return GetElementEnum<T>(node, name) ?? defaultValue;
	    }

	    public static Guid? GetElementGuid([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetElementString(name);
	        return text != null && Guid.TryParse(text, out var value) ? value : (Guid?) null;
	    }

		public static Guid GetElementGuid([NotNull] this XElement node, [NotNull] string name, Guid defaultValue)
		{
		    return GetElementGuid(node, name) ?? defaultValue;
		}

	    public static int? GetElementInt([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetElementString(name);
	        return (text != null && int.TryParse(text, out var value)) ? value : (int?) null;
	    }

	    public static int GetElementInt([NotNull] this XElement node, [NotNull] string name, int defaultValue)
	    {
	        return GetElementInt(node, name) ?? defaultValue;
	    }                              

	    public static short? GetElementShort([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetElementString(name);
	        return (text != null && short.TryParse(text, out var value)) ? value : (short?) null;
	    }

	    public static short GetElementShort([NotNull] this XElement node, [NotNull] string name, short defaultValue)
	    {
	        return GetElementShort(node, name) ?? defaultValue;
	    }

	    public static string GetElementString([NotNull] this XElement node, [NotNull] string name, string defaultValue = null)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var result = node.Element(name)?.Value;
	        return string.IsNullOrWhiteSpace(result) ? defaultValue : result;
	    }
                                                                                                   
	    [CanBeNull]
	    public static Type GetElementValueType([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        if (name.IndexOf('.') > 0)
	        {
                // Try easy search first
	            var result = Type.GetType(name);
	            if (result != null)
	                return result;

                // Search in available assemblies if needed
	            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
	            {
	                result = assembly.GetType(name);
	                if (result != null)
	                    return result;
	            }
	        }

            // Perform an exhaustive search if needed
	        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault(x => x.Name == name);
	    }

		public static Type GetElementValueType([NotNull] this XElement node, [NotNull] string name, Type defaultValue)
		{
            return GetElementValueType(node, name) ?? defaultValue;
		}

      #endregion

      #region Element - Set Value (single)

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, bool value)
	    {
	        SetElementValue(node, name, value.ToString(CultureInfo.InvariantCulture));
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, bool? value)
	    {
            if (value == null)
                node.RemoveElements(name);
            else
	            SetElementValue(node, name, value.Value);
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, Color value)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        node.RemoveElements(name);

	        var valueElement = node.GetElement(name, XmlLookupMode.CreateOrRead);
	        valueElement.RemoveAll();

	        valueElement.SetAttributeValue("Alpha", value.A);
	        valueElement.SetAttributeValue("Red", value.R);      
	        valueElement.SetAttributeValue("Green", value.G);
	        valueElement.SetAttributeValue("Blue", value.B);
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, Color? value)
	    {
	        if (value == null)
	            node.RemoveElements(name);
	        else
	            SetElementValue(node, name, value.Value);
	    }

		public static void SetElementValueDateOnly([NotNull] this XElement node, [NotNull] string name, DateTime value)
		{
		    SetElementValue(node, name, value.ToStorageDate());
		}

	    public static void SetElementValueDateOnly([NotNull] this XElement node, [NotNull] string name, DateTime? value)
	    {
	        if (value == null)
	            node.RemoveElements(name);
	        else
	            SetElementValueDateOnly(node, name, value.Value);
	    }

		public static void SetElementValueTimeOnly([NotNull] this XElement node, [NotNull] string name, DateTime value)
		{
		    SetElementValue(node, name, value.ToStorageTime());
		}

	    public static void SetElementValueTimeOnly([NotNull] this XElement node, [NotNull] string name, DateTime? value)
	    {
	        if (value == null)
	            node.RemoveElements(name);
	        else
	            SetElementValueTimeOnly(node, name, value.Value);
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, DateTime value)
	    {
	        SetElementValue(node, name, value.ToStorageDateTime());
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, DateTime? value)
	    {
	        if (value == null)
	            node.RemoveElements(name);
	        else
	            SetElementValue(node, name, value.Value);
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, decimal value)
	    {
	        SetElementValue(node, name, value.ToString(CultureInfo.InvariantCulture));
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, decimal? value)
	    {
	        if (value == null)
	            node.RemoveElements(name);
	        else
	            SetElementValue(node, name, value.Value);
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, double value)
	    {
	        SetElementValue(node, name, value.ToString(CultureInfo.InvariantCulture));
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, double? value)
	    {
	        if (value == null)
	            node.RemoveElements(name);
	        else
	            SetElementValue(node, name, value.Value);
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, Enum value)
	    {
	        SetElementValue(node, name, value.ToString());
	    }

		public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, Guid value)
		{
		    SetElementValue(node, name, value.ToString());
		}

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, Guid? value)
	    {
	        if (value == null)
	            node.RemoveElements(name);
	        else
	            SetElementValue(node, name, value.Value);
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, int value)
	    {
            SetElementValue(node, name, value.ToString(CultureInfo.InvariantCulture));
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, int? value)
	    {
	        if (value == null)
	            node.RemoveElements(name);
	        else
	            SetElementValue(node, name, value.Value);
	    }

	    public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, string value)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        node.RemoveElements(name);
            if (string.IsNullOrWhiteSpace(value))
                return;  // Remove any existing elements but don't add if there is no new value

	        var valueElement = node.GetElement(name, XmlLookupMode.CreateOrRead);
	        valueElement.RemoveAll();
	        valueElement.Value = value;
	    }

		public static void SetElementValue([NotNull] this XElement node, [NotNull] string name, Type value)
		{
		    SetElementValue(node, name, value?.FullName);
		}

      #endregion

      #region Eleemnts - Get Value (multiple)

	    /// <summary>
	    /// Attempts to find all elements with the specified <paramref name="name"/> and returns all values found as an enumerable.
	    /// Returns an empty enumerable if no elements are found (does not return null).
	    /// </summary>
	    public static IEnumerable<string> GetElementValues(this XElement node, string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        return node.Elements(name).Select(e => e.Value);
	    }

      #endregion

      #region Elements - Set Value (multiple)

	    /// <summary>
	    /// Adds multiple <paramref name="values"/> as individual XML Elements.
	    /// Removes any existing Elements with <paramref name="name"/>.
	    /// </summary>
	    public static void GetElementValues(this XElement node, string name, IEnumerable<string> values)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        node.RemoveElements(name);
	        if (values == null)
	            return; // Remove any existing elements but don't add if there is no new value

	        foreach (var value in values.Where(v => v != null))
	        {
                var element = new XElement(name) {Value = value};
                node.Add(element);
	        }
	    }

      #endregion

      #region Attribute - Get Value (single)

	    public static bool? GetAttributeBool([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetAttributeString(name);
	        return (text != null && bool.TryParse(text, out var value)) ? value : (bool?) null;
	    }

		public static bool GetAttributeBool([NotNull] this XElement node, [NotNull] string name, bool defaultValue)
		{
		    return GetAttributeBool(node, name) ?? defaultValue;
		}

	    public static byte? GetAttributeByte([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetAttributeString(name);
	        return (text != null && byte.TryParse(text, out var value)) ? value : (byte?) null;
	    }

	    public static byte GetAttributeByte([NotNull] this XElement node, [NotNull] string name, byte defaultValue)
	    {
	        return GetAttributeByte(node, name) ?? defaultValue;
	    }

	    public static DateTime? GetAttributeDate(this XElement node, string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        var text = node.GetAttributeString(name);
	        return text != null && text.TryToDate(out var value) ? value : (DateTime?) null;
	    }

		public static DateTime GetAttributeDate([NotNull] this XElement node, [NotNull] string name, DateTime defaultValue)
		{
		    return GetAttributeDate(node, name) ?? defaultValue;
		}

	    public static DateTime? GetAttributeTime([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        var text = node.GetAttributeString(name);
	        return text != null && text.TryToTimeAsUtc(out var value) ? value : (DateTime?) null;
	    }

		public static DateTime GetAttributeTime([NotNull] this XElement node, [NotNull] string name, DateTime defaultValue)
		{
		    return GetAttributeTime(node, name) ?? defaultValue;
		}

	    public static DateTime? GetAttributeDateTime([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        var text = node.GetAttributeString(name);
	        return text != null && text.TryToDateTimeAsUtc(out var value) ? value : (DateTime?) null;
	    }

	    public static DateTime GetAttributeDateTime([NotNull] this XElement node, [NotNull] string name, DateTime defaultValue)
	    {
	        return GetAttributeDateTime(node, name) ?? defaultValue;
	    }

	    public static decimal? GetAttributeDecimal([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetAttributeString(name);
	        return (text != null && decimal.TryParse(text, out var value)) ? value : (decimal?) null;
	    }

	    public static decimal GetAttributeDecimal([NotNull] this XElement node, [NotNull] string name, decimal defaultValue)
	    {
	        return GetAttributeInt(node, name) ?? defaultValue;
	    }

	    public static double? GetAttributeDouble([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetAttributeString(name);
	        return (text != null && double.TryParse(text, out var value)) ? value : (double?) null;
	    }

	    public static double GetAttributeDouble([NotNull] this XElement node, [NotNull] string name, double defaultValue)
	    {
	        return GetAttributeInt(node, name) ?? defaultValue;
	    }

	    public static T? GetAttributeEnum<T>([NotNull] this XElement node, [NotNull] string name) where T : struct, IConvertible
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        if (!typeof(T).IsEnum)
	            throw new ArgumentException("T in SetAttributeValue<T> must be an enum.");
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        var text = node.GetAttributeString(name);
	        return text != null && Enum.TryParse(text, true, out T result) ? result : (T?)null;
	    }

	    public static T GetAttributeEnum<T>([NotNull] this XElement node, [NotNull] string name, T defaultValue) where T : struct, IConvertible
	    {
	        return GetAttributeEnum<T>(node, name) ?? defaultValue;
	    }

	    public static Guid? GetAttributeGuid([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetAttributeString(name);
	        return text != null && Guid.TryParse(text, out var value) ? value : (Guid?) null;
	    }

		public static Guid GetAttributeGuid([NotNull] this XElement node, [NotNull] string name, Guid defaultValue)
		{
		    return GetAttributeGuid(node, name) ?? defaultValue;
		}

	    public static int? GetAttributeInt([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetAttributeString(name);
	        return (text != null && int.TryParse(text, out var value)) ? value : (int?) null;
	    }

	    public static int GetAttributeInt([NotNull] this XElement node, [NotNull] string name, int defaultValue)
	    {
	        return GetAttributeInt(node, name) ?? defaultValue;
	    }

	    public static short? GetAttributeShort([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var text = node.GetAttributeString(name);
	        return (text != null && short.TryParse(text, out var value)) ? value : (short?) null;
	    }

	    public static short GetAttributeShort([NotNull] this XElement node, [NotNull] string name, short defaultValue)
	    {
	        return GetAttributeShort(node, name) ?? defaultValue;
	    }

	    public static string GetAttributeString([NotNull] this XElement node, [NotNull] string name, string defaultValue = null)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        var result = node.Attribute(name)?.Value;
	        return string.IsNullOrWhiteSpace(result) ? defaultValue : result;
	    }
                                                                                                   
	    [CanBeNull]
	    public static Type GetAttributeValueType([NotNull] this XElement node, [NotNull] string name)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (name == null)
	            throw new ArgumentNullException(nameof(name));

	        if (name.IndexOf('.') > 0)
	        {
                // Try easy search first
	            var result = Type.GetType(name);
	            if (result != null)
	                return result;

                // Search in available assemblies if needed
	            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
	            {
	                result = assembly.GetType(name);
	                if (result != null)
	                    return result;
	            }
	        }

            // Perform an exhaustive search if needed
	        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault(x => x.Name == name);
	    }

		public static Type GetAttributeValueType([NotNull] this XElement node, [NotNull] string name, Type defaultValue)
		{
            return GetAttributeValueType(node, name) ?? defaultValue;
		}

      #endregion

      #region Attribute - Set Value (single)

		public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, bool value)
		{
		    SetAttributeValue(node, name, value.ToString(CultureInfo.InvariantCulture));
		}

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, bool? value)
	    {
	        if (value == null)
	            node.RemoveAttributes(name);
	        else
	            SetAttributeValue(node, name, value.Value);
	    }

		public static void SetAttributeValueDate([NotNull] this XElement node, [NotNull] string name, DateTime value)
		{
		    SetAttributeValue(node, name, value.ToStorageDate());
		}

	    public static void SetAttributeValueDate([NotNull] this XElement node, [NotNull] string name, DateTime? value)
	    {
	        if (value == null)
	            node.RemoveAttributes(name);
	        else
	            SetAttributeValue(node, name, value.Value);
	    }

		public static void SetAttributeValueTime([NotNull] this XElement node, [NotNull] string name, DateTime value)
		{
		    SetAttributeValue(node, name, value.ToStorageTime());
		}

	    public static void SetAttributeValueTime([NotNull] this XElement node, [NotNull] string name, DateTime? value)
	    {
	        if (value == null)
	            node.RemoveAttributes(name);
	        else
	            SetAttributeValue(node, name, value.Value);
	    }

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, DateTime value)
	    {
	        SetAttributeValue(node, name, value.ToStorageDateTime());
	    }

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, DateTime? value)
	    {
	        if (value == null)
	            node.RemoveAttributes(name);
	        else
	            SetAttributeValue(node, name, value.Value);
	    }

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, decimal value)
	    {
	        SetAttributeValue(node, name, value.ToString(CultureInfo.InvariantCulture));
	    }

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, decimal? value)
	    {
	        if (value == null)
	            node.RemoveAttributes(name);
	        else
	            SetAttributeValue(node, name, value.Value);
	    }

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, double value)
	    {
	        SetAttributeValue(node, name, value.ToString(CultureInfo.InvariantCulture));
	    }

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, double? value)
	    {
	        if (value == null)
	            node.RemoveAttributes(name);
	        else
	            SetAttributeValue(node, name, value.Value);
	    }

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, Enum value)
	    {
	        SetAttributeValue(node, name, value.ToString());
	    }

		public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, Guid value)
		{
		    SetAttributeValue(node, name, value.ToString());
		}

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, Guid? value)
	    {
	        if (value == null)
	            node.RemoveAttributes(name);
	        else
	            SetAttributeValue(node, name, value.Value);
	    }

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, int value)
	    {
            SetAttributeValue(node, name, value.ToString(CultureInfo.InvariantCulture));
	    }

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, int? value)
	    {
	        if (value == null)
	            node.RemoveAttributes(name);
	        else
	            SetAttributeValue(node, name, value.Value);
	    }

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, string value)
	    {
	        if (node == null)
	            throw new ArgumentNullException(nameof(node));
	        if (string.IsNullOrWhiteSpace(name))
	            throw new ArgumentNullException(name);

	        node.RemoveAttributes(name);
            if (string.IsNullOrWhiteSpace(value))
                return;  // Remove any existing attributes but don't add if there is no new value

	        var valueAttribute = node.GetAttribute(name, XmlLookupMode.CreateOrRead);
	        valueAttribute.Value = value;
	    }

	    public static void SetAttributeValue([NotNull] this XElement node, [NotNull] string name, Type value)
	    {
	        SetAttributeValue(node, name, value?.FullName);
	    }

      #endregion
	}
}