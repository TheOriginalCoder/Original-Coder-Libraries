//=============================================================================
// Original Coder - Common Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2013, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using OriginalCoder.Common.Exceptions;

namespace OriginalCoder.Common.Extensions
{
	/// <summary>
	/// Extensions for working with enumeration types.
	/// </summary>
	[PublicAPI]
	public static class EnumerationExtensions
	{
		/// <summary>
		/// Returns true if the flags enum contains <paramref name="value"/>.
		/// </summary>
		public static bool Has<T>(this Enum enumeration, T value)
		{
			try
			{
				return (((int)(object) enumeration & (int)(object)value) == (int)(object)value);
			}
			catch (Exception ex)
		    {
		        throw new ArgumentException($"Exception determining if Value [{value}] is present in Enum of type [{typeof(T).Name}].", ex);
		    }
		}

	    /// <summary>
	    /// Returns true if the flags enum contains all of the specified <paramref name="values"/>.
	    /// </summary>
		public static bool HasAll<T>(this Enum enumeration, [NotNull] IEnumerable<T> values)
		{
		    if (values == null)
		        throw new ArgumentNullException(nameof(values));
            return values.Aggregate(true, (current, value) => current && enumeration.Has(value));
		}

	    /// <summary>
	    /// Returns true if the flags enum contains any of the specified <paramref name="values"/>.
	    /// </summary>
	    public static bool HasAny<T>(this Enum enumeration, [NotNull] IEnumerable<T> values)
	    {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            return values.Any(value => enumeration.Has(value));
        }

        /// <summary>
        /// Adds the specified <paramref name="value"/> to <paramref name="enumeration"/> and returns the result.
        /// </summary>
		public static T Add<T>(this Enum enumeration, T value)
		{
			try
			{
				return (T)(object)(((int)(object) enumeration | (int)(object)value));
			}
			catch (Exception ex)
		    { throw new ArgumentException($"Exception adding value [{value}] to Enum of type [{typeof(T).Name}].", ex); }
		}

	    /// <summary>
	    /// Adds all of the specified <paramref name="values"/> to <paramref name="enumeration"/> and returns the result.
	    /// </summary>
		public static T Add<T>(this Enum enumeration, IEnumerable<T> values)
		{
			try
			{
                var resultValue = (int) (object) enumeration;
			    if (values != null)
			    {
			        foreach (var value in values)
			            resultValue |= (int) (object) value;
			    }
			    return (T) (object) resultValue;
			}
			catch (OcException)
			{ throw; }
			catch (Exception ex)
		    { throw new ArgumentException($"Exception adding values to Enum of type [{typeof(T).Name}].", ex); }
		}

	    /// <summary>
	    /// Adds the specified <paramref name="conditionalValues"/> to <paramref name="enumeration"/> and returns the result.
	    /// For each tuple the Value will be added if IncludeCondition is true.
	    /// </summary>
	    public static T Add<T>(this Enum enumeration, params (bool IncludeCondition, T Value)[] conditionalValues)
	    {
            return Add(enumeration, conditionalValues.Where(tv => tv.IncludeCondition).Select(tv => tv.Value));
	    }

	    /// <summary>
	    /// Removes the specified <paramref name="value"/> from <paramref name="enumeration"/> and returns the result.
	    /// </summary>
		public static T Remove<T>(this Enum enumeration, T value)
		{
			try
			{
				return (T)(object)(((int)(object) enumeration & ~(int)(object)value));
			}
			catch (Exception ex)
		    { throw new ArgumentException($"Exception removing value [{value}] from Enum of type [{typeof(T).Name}].", ex); }
		}

	    /// <summary>
	    /// Removes all of the specified <paramref name="values"/> from <paramref name="enumeration"/> and returns the result.
	    /// </summary>
		public static T Remove<T>(this Enum enumeration, IEnumerable<T> values)
		{
			try
			{
			    var resultValue = (int) (object) enumeration;
			    if (values != null)
			    {
			        foreach (var value in values)
			            resultValue &= ~(int) (object) value;
			    }
			    return (T) (object) resultValue;
			}
			catch (OcException)
			{ throw; }
			catch (Exception ex)
		    { throw new ArgumentException($"Exception removing values from Enum of type [{typeof(T).Name}].", ex); }
		}

	    /// <summary>
	    /// Removes the specified <paramref name="conditionalValues"/> from <paramref name="enumeration"/> and returns the result.
	    /// For each tuple Value will be removed if IncludeCondition is false.
	    /// </summary>
	    public static T Remove<T>(this Enum enumeration, params (bool IncludeCondition, T Value)[] conditionalValues)
	    {
	        return Remove(enumeration, conditionalValues.Where(tv => !tv.IncludeCondition).Select(tv => tv.Value));
	    }
	}
}
