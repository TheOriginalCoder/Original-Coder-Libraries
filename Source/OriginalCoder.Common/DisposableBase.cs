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

namespace OriginalCoder.Common
{
	/// <summary>
	/// Abstract base class that implements IDisposable.
	/// Provides protected virtual methods for cleaning up managed 
	/// (<see cref="DisposeManaged"/>) and unmanaged (<see cref="DisposeUnManaged"/>) resources.
	/// </summary>
	/// <seealso cref="DisposableHierarchyBase"/>
	[PublicAPI]
	public class DisposableBase : IDisposable
	{
	    /// <summary>
	    /// Called to dispose of managed resources the instance owns.
	    /// This only gets called if the object's Dispose() method is
	    /// invoked by the application developer.  This gets called
	    /// before DisposeUnManaged when applicable.
	    /// </summary>
	    protected virtual void DisposeManaged()
	    {
	        // To be overridden as needed.
	    }

	    /// <summary>
	    /// Called to dispose of any unmanaged (outside of .NET)
	    /// resources owned by the instance.  This method will always get
	    /// called, either by an explicit call to Dispose() or via the
	    /// finalizer when the object is eventually garbage collected.
	    /// </summary>
	    protected virtual void DisposeUnManaged()
	    {
	        // To be overridden as needed.
	    }

	  #region IDisposable

		private bool _disposed;

		/// <summary>
		/// Disposes of the object immediately and frees any resources it own.
		/// Same effect as calling Close() for this class.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Called to cleanup resources owned by the object.  
		/// </summary>
		/// <param name="disposing">If dispose is true all
		/// resources, including .NET managed objects, should be cleaned up.  If
		/// disposing is false we were called by the finalizer and NO managed
		/// objects should be referenced!</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
				DisposeManaged();
			DisposeUnManaged();

			_disposed = true;
		}

		// Finalizer that eventually gets called by the runtime if Dispose() is never called explicitly.
		~DisposableBase()
		{
			Dispose(false);
		}

	  #endregion
	}
}
