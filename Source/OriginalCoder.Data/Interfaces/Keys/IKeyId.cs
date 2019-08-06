//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2011, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

namespace OriginalCoder.Data.Interfaces.Keys
{
    /// <summary>
    /// Standard interface for a unique Identifier (ID) of type <typeparamref name="T"/>.
    /// For consistency and ease of maintenance Unique Identifiers (IDs) should always be a whole number.
    /// If the ID is a 32 or 64 bit integer please use <see cref="IKeyId"/> or <see cref="IKeyId64"/> instead.
    /// Please use <see cref="IKeyUid"/> for unique keys that are GUIDs and <see cref="IKey"/> for unique keys that are strings.
    /// </summary>
    public interface IKeyId<T>
    {
        T Id { get; }
    }

    /// <summary>
    /// Standard interface for a unique Identifier (ID) that is an integer (32 bit signed number).
    /// </summary>
    public interface IKeyId : IKeyId<int>
    { }
}
