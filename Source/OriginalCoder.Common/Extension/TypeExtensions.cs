//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2018, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using JetBrains.Annotations;
using OriginalCoder.Common.Exceptions;

namespace OriginalCoder.Common.Extension
{
    /// <summary>
    /// Extension methods related to <see cref="Type"/>.
    /// </summary>
    [PublicAPI]
	public static class TypeExtensions
	{
		/// <summary>
		/// Returns a newly created instance of <paramref name="type"/> using the default no parameter constructor if one exists.
		/// If a suitable constructor can not be found an <see cref="OcLibraryException"/> is thrown.
		/// </summary>
		public static object CreateInstance([NotNull] this Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			// Look for a public constructor that has no parameters (the default constructor)
			var constructor = type.GetConstructor(Type.EmptyTypes);
			if (constructor == null)
				throw new OcLibraryException($"Constructor taking no parameters not found for Type [{type.FullName}]");

			// Instantiate the type using the constructor
			var parameters = new object[0];
            try
            {
                return constructor.Invoke(parameters);
            }
            catch (Exception ex)
            {
                throw new OcLibraryException($"Exception constructing an instance of [{type.FullName}] using the default (no parameter) constructor", ex);
            }
		}

        /// <summary>
        /// Attempts to file and return the <see cref="Type"/> that given the type's name in <paramref name="typeName"/>.
        /// If found true is returned and <paramref name="type"/> is populated.  Otherwise false is returned and <paramref name="type"/> will be invalid.
        /// </summary>
        public static bool TryParseTypeName([NotNull] this string typeName, out Type type)
		{
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentNullException(nameof(typeName));

			type = null;
			try
			{
				// Try to find the type without using version information (strip out the version information)
				var index = typeName.IndexOf(", Version=", StringComparison.Ordinal);
				if (index > 0)
				{
					var withoutVersion = typeName.Substring(0, index).Trim();
					type = Type.GetType(withoutVersion, false, true);
					if (type != null)
						return true;
				}

				// Try to find the type using the typeName exactly as it was passed in.
				type = Type.GetType(typeName, false, true);
				if (type != null)
					return true;

				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
