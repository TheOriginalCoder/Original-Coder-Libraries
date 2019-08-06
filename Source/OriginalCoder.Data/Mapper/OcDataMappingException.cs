//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2010, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OriginalCoder.Common.Extensions;
using OriginalCoder.Data.Exceptions;

// ReSharper disable ExplicitCallerInfoArgument
namespace OriginalCoder.Data.Mapper
{
    /// <summary>
    /// Original Coder exception that is related to data mapping (conversion).
    /// </summary>
    [PublicAPI]
    public class OcDataMappingException : OcDataException
    {
      #region Constructors 

        public OcDataMappingException(string message, [NotNull] Type typeIn, [NotNull] Type typeOut, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Data Mapping Error" : message, callerName, callerFile, callerLine)
        {
            TypeIn = typeIn;
            if (TypeIn != null)
                PropertySet(nameof(TypeIn), TypeIn);
            TypeOut = typeOut;
            if (TypeOut != null)
                PropertySet(nameof(TypeOut), TypeOut);
        }

        public OcDataMappingException(string message, [CanBeNull] Exception exception, [NotNull] Type typeIn, [NotNull] Type typeOut, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(string.IsNullOrWhiteSpace(message) ? "Data Mapping Error" : message, exception, callerName, callerFile, callerLine)
        {
            TypeIn = typeIn;
            if (TypeIn != null)
                PropertySet(nameof(TypeIn), TypeIn);
            TypeOut = typeOut;
            if (TypeOut != null)
                PropertySet(nameof(TypeOut), TypeOut);
        }

        public OcDataMappingException(string message, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, properties, callerName, callerFile, callerLine)
        { }

        public OcDataMappingException(string message, [CanBeNull] Exception exception, IReadOnlyDictionary<string, object> properties, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
                : base(message, exception, properties, callerName, callerFile, callerLine)
        { }

      #endregion

        /// <summary>
        /// Type of the source input
        /// </summary>
        public Type TypeIn { get; }

        /// <summary>
        /// Type that was supposed to be output
        /// </summary>
        public Type TypeOut { get; }

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Data Mapping Error");

        protected override void SummaryBuildProperties()
        {
            base.SummaryBuildProperties();
            SummaryAddProperty("TypeOut", TypeOut?.FriendlyName());
            SummaryAddProperty("TypeIn", TypeIn?.FriendlyName());
        }

      #endregion
    }
}