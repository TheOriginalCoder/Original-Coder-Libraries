//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2013, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using JetBrains.Annotations;
using OriginalCoder.Data.Interfaces.Keys;

namespace OriginalCoder.Data.Extensions
{
    /// <summary>
    /// Extension methods for obtaining unique keys of objects using standard data key interfaces.
    /// </summary>
    [PublicAPI]
    public static class DataKeyExtensions
    {
      #region Identifier (ID) - A whole number that uniquely identifies to the object.  Ideal for use in databases.

        /// <summary>
        /// Attempts to obtain the Unique Identifier (ID) for the object.
        /// Null is returned if the object does not implement a standard ID interface.
        /// </summary>
        public static long? GetId([NotNull] this object data)
        {
            if (TryGetId(data, out var id))
                return id;
            return null;
        }

        /// <summary>
        /// Attempts to obtain the Unique Identifier (ID) for the object.
        /// If available <paramref name="id"/> will be assigned the unique identifier and true will be returned. 
        /// False is returned and the value of <paramref name="id"/> is invalid if the object does not implement a standard ID interface.
        /// </summary>
        public static bool TryGetId([NotNull] this object data, out long id)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IKeyId asKeyId)
            {
                id = asKeyId.Id;
                return true;
            }

            if (data is IKeyId64 asKeyId64)
            {
                id = asKeyId64.Id;
                return true;
            }

            if (data is IKeyId<byte> asKeyIdByte)
            {
                id = asKeyIdByte.Id;
                return true;
            }

            if (data is IKeyId<short> asKeyIdShort)
            {
                id = asKeyIdShort.Id;
                return true;
            }

            if (data is IKeyId<ushort> asKeyIdUShort)
            {
                id = asKeyIdUShort.Id;
                return true;
            }

            if (data is IKeyId<uint> asKeyIdUInt)
            {
                id = asKeyIdUInt.Id;
                return true;
            }

            id = default;
            return false;
        }

      #endregion

      #region Universal Identifier (UID) - A GUID that uniquely identifies the object.  Ideal for referency by or communicating with external systems.

        /// <summary>
        /// Attempts to obtain the Universal Identifier (UID) for the object.
        /// Null is returned if the object does not implement a standard UID interface.
        /// </summary>
        public static Guid? GetUid([NotNull] this object data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IKeyUid asKeyUid)
                return asKeyUid.Uid;
            return null;
        }

        /// <summary>
        /// Attempts to obtain the Universal Identifier (UID) for the object.
        /// If available <paramref name="uid"/> will be assigned the unique identifier and true will be returned. 
        /// False is returned and the value of <paramref name="uid"/> is invalid if the object does not implement a standard UID interface.
        /// </summary>
        public static bool TryGetUid([NotNull] this object data, out Guid uid)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IKeyUid asKeyUid)
            {
                uid = asKeyUid.Uid;
                return true;
            }

            uid = default;
            return false;
        }

      #endregion

      #region Unique Key (key) - A string that uniquely identifies the object.

        /// <summary>
        /// Attempts to obtain the Unique Key (Key) for the object.
        /// Null is returned if the object does not implement a standard Key interface.
        /// </summary>
        public static string GetKey([NotNull] this object data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IKey asKey)
                return asKey.Key;
            return null;
        }

        /// <summary>
        /// Attempts to obtain the Unique Key (Key) for the object.
        /// If available <paramref name="key"/> will be assigned the unique identifier and true will be returned. 
        /// False is returned and the value of <paramref name="key"/> is invalid if the object does not implement a standard Key interface.
        /// </summary>
        public static bool TryGetKey([NotNull] this object data, out string key)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IKey asKey)
            {
                key = asKey.Key;
                return true;
            }

            key = null;
            return false;
        }

      #endregion
    }
}