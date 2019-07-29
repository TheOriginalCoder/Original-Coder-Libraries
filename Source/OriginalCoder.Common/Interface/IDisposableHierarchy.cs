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
using JetBrains.Annotations;

namespace OriginalCoder.Common.Interface
{
    /// <summary>
    /// <see cref="IDisposable"/> interface that adds support for automatically cleaning up child objects.
    /// </summary>
    [PublicAPI]
    public interface IDisposableHierarchy : IDisposable
    {
        /// <summary>
        /// Adds a <paramref name="child"/> that will be automatically disposed when this object is disposed.
        /// </summary>
        void AddChild(IDisposable child);
    }
}
