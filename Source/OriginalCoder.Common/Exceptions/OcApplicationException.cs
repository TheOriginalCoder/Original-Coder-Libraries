//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2009, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OriginalCoder.Common.Extensions;
using OriginalCoder.Common.Interfaces.Properties;

namespace OriginalCoder.Common.Exceptions
{
    /// <summary>
    /// Original Coder base class for Application Exceptions.
    /// </summary>
    [PublicAPI]
    public class OcApplicationException : ApplicationException, ISummary, IProperties
    {
      #region Constructors 

        public OcApplicationException(string message, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Application Error" : message)
        {
            (CallerName, CallerFile, CallerLine) = PopulateCallerInfo(callerName, callerFile, callerLine);
        }

        public OcApplicationException(string message, [CanBeNull] Exception exception, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Application Error" : message, exception)
        {
            (CallerName, CallerFile, CallerLine) = PopulateCallerInfo(callerName, callerFile, callerLine);
        }

        public OcApplicationException(string message, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Application Error" : message)
        {
            (CallerName, CallerFile, CallerLine) = PopulateCallerInfo(callerName, callerFile, callerLine);
            if (properties?.Count > 0)
            {
                foreach (var kv in properties.Where(kv => !string.IsNullOrWhiteSpace(kv.Key)))
                    PropertySet(kv.Key, kv.Value);
            }
        }

        public OcApplicationException(string message, [CanBeNull] Exception exception, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Unspecified Application Error" : message, exception)
        {
            (CallerName, CallerFile, CallerLine) = PopulateCallerInfo(callerName, callerFile, callerLine);
            if (properties?.Count > 0)
            {
                foreach (var kv in properties.Where(kv => !string.IsNullOrWhiteSpace(kv.Key)))
                    PropertySet(kv.Key, kv.Value);
            }
        }

        private (string callerName, string callerFile, int? callerLine) PopulateCallerInfo(string callerName, string callerFile, int? callerLine)
        {
            callerName = string.IsNullOrWhiteSpace(callerName) ? null : callerName;
            callerFile = string.IsNullOrWhiteSpace(callerFile) ? null : Path.GetFileName(callerFile);
            callerLine = callerLine <= 0 ? null : callerLine;
            if (!string.IsNullOrWhiteSpace(callerName) && callerName.IndexOf("exception", StringComparison.OrdinalIgnoreCase) <= 0)
            {
                PropertySet(nameof(CallerName), callerName);
                if (!string.IsNullOrWhiteSpace(callerFile))
                    PropertySet(nameof(CallerFile), callerFile);
                if (callerLine.HasValue)
                    PropertySet(nameof(CallerLine), callerLine.Value);
            }
            return (callerName, callerFile, callerLine);
        }

      #endregion

        public string CallerName { get; }
        public string CallerFile { get; }
        public int? CallerLine { get; }

      #region Summary

        /// <inheritdoc />
        public virtual string Summary => SummaryBuild("Application Error");

        protected virtual void SummaryBuildProperties()
        { }

        protected string SummaryBuild(string errorName)
        {
            SummaryClearProperties();
            SummaryBuildProperties();
            var propertyText = OcString.ConcatenateReverse(Properties, _summaryProperties);

            var result = (string.IsNullOrWhiteSpace(errorName) ? GetType().Name.Replace("Oc", "").Replace("Exception", "") + " Error" : errorName.Trim()) + ": ";
            if (propertyText.Length > 0 && !string.IsNullOrWhiteSpace(Message))
                result = result + propertyText + " -- " + Message;
            else if (propertyText.Length > 0)
                result = result + propertyText;
            else
                result = result + Message;
            return result;
        }

        private List<(string name, object value)> _summaryProperties;

        protected void SummaryClearProperties()
        {
            _summaryProperties = new List<(string name, object value)>();
        }

        protected void SummaryAddProperty(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                _summaryProperties.Add((name.Trim(), null));
        }

        protected void SummaryAddProperty(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            _summaryProperties.Add((name.Trim(), value));
        }

        protected void SummaryAddProperties(params (string name, object value)[] properties)
        {
            if (properties == null || properties.Length == 0)
                return;

            foreach (var property in properties)
                SummaryAddProperty(property.name, property.value);
        }

      #endregion

      #region Properties

        /// <inheritdoc />
        public IReadOnlyDictionary<string, object> Properties => _properties;
        private Dictionary<string, object> _properties;

        private static readonly HashSet<string> _excludeObjectProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Data", "InnerException", "Message", "StackTrace", "Source" };

        protected void PropertySet(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            name = name.Trim();
            if (value == null && _properties == null)
                return;  // Don't need to remove if it doesn't exist

            if (_properties == null)
                _properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (value == null)
            {
                _properties.Remove(name);
                return;
            }

            _properties[name] = value;

            // If there is a Property on this Exception instance with this name, attempt to set its value
            if (!_excludeObjectProperties.Contains(name))
                this.PropertyValueSet(name, value);
        }

        protected void PropertyAddRange(IEnumerable<(string name, object value)> properties)
        {
            if (properties == null)
                return;

            foreach (var property in properties)
                PropertySet(property.name, property.value);
        }

      #endregion

        public override string ToString() => Summary;
    }
}