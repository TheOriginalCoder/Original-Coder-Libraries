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
using System.Diagnostics;
using JetBrains.Annotations;
using OriginalCoder.Common.Interface;

namespace OriginalCoder.Common
{
    /// <summary>
    /// Adds hierarchy support to <see cref="DisposableBase"/> so that children can be 
    /// added which are then disposed of automatically in reverse order.
    /// </summary>
    [PublicAPI]
    public abstract class DisposableHierarchyBase : DisposableBase, IDisposableHierarchy
    {
      #region IDisposableHierarchy

        /// <summary>
        /// Children are stored in a stack since they need to be disposed of in reverse order.
        /// </summary>
        private readonly Stack<IDisposable> _children = new Stack<IDisposable>();

        /// <summary>
        /// Adds a child that will be automatically disposed of when this object is disposed.
        /// Children are disposed of in reverse order (most recently added first).
        /// </summary>
        public void AddChild(IDisposable child)
        {
            if (!_children.Contains(child))
                _children.Push(child);
        }

      #endregion

      #region IDisposable

        protected void DisposeChildren()
        {
            // Dispose of children in reverse order (newest first)
            while (_children.Count > 0)
                _children.Pop().Dispose();
            Debug.Assert(_children.Count == 0);
        }

        protected override void DisposeManaged()
        {
            DisposeChildren();
            base.DisposeManaged();
        }

      #endregion
    }
}