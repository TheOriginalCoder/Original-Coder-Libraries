//=============================================================================
// Original Coder - Layer Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using OriginalCoder.Common.Extensions;
using OriginalCoder.Layers.Interfaces;

// ReSharper disable ExplicitCallerInfoArgument
namespace OriginalCoder.Layers.Exceptions
{
    [PublicAPI]
    public class OcAdapterLayerException : OcLayerException
    {
      #region Constructors 

        public OcAdapterLayerException(string message, IOcLayer layer, Type typeIn, Type typeOut, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, layer, null, null, callerName, callerFile, callerLine)
        {
            TypeIn = typeIn;
            if (TypeIn != null)
                PropertySet(nameof(TypeIn), TypeIn);
            TypeOut = typeOut;
            if (TypeOut != null)
                PropertySet(nameof(TypeOut), TypeOut);
        }

        public OcAdapterLayerException(string message, [CanBeNull] Exception exception, IOcLayer layer, Type typeIn, Type typeOut, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, exception, layer, null, null, callerName, callerFile, callerLine)
        {
            TypeIn = typeIn;
            if (TypeIn != null)
                PropertySet(nameof(TypeIn), TypeIn);
            TypeOut = typeOut;
            if (TypeOut != null)
                PropertySet(nameof(TypeOut), TypeOut);
        }

      #endregion

        /// <summary>
        /// Type of the source input to the adapter
        /// </summary>
        public Type TypeIn { get; }

        /// <summary>
        /// Type that was supposed to be output by the adapter
        /// </summary>
        public Type TypeOut { get; }

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Adapter Layer Error");

        protected override void SummaryBuildProperties()
        {
            base.SummaryBuildProperties();
            SummaryAddProperty("TypeOut", TypeOut?.FriendlyName());
            SummaryAddProperty("TypeIn", TypeIn?.FriendlyName());
        }

      #endregion
    }
}