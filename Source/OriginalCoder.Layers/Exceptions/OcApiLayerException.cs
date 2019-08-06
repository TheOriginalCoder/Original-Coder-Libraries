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
using OriginalCoder.Layers.Interfaces;

namespace OriginalCoder.Layers.Exceptions
{
    [PublicAPI]
    public class OcApiLayerException : OcLayerException
    {
      #region Constructors 

        public OcApiLayerException(string message, IOcLayer layer, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, layer, entity, entityKey, callerName, callerFile, callerLine)
        { }

        public OcApiLayerException(string message, [CanBeNull] Exception exception, IOcLayer layer, [CanBeNull] object entity = null, [CanBeNull] object entityKey = null, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFile = null, [CallerLineNumber] int callerLine = 0)
            : base(message, exception, layer, entity, entityKey, callerName, callerFile, callerLine)
        { }

      #endregion

      #region Summary

        /// <inheritdoc />
        public override string Summary => SummaryBuild("Api Layer Error");

      #endregion
    }
}