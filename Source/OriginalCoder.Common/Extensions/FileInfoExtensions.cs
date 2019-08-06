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
using System.IO;
using System.Security.Cryptography;
using JetBrains.Annotations;

namespace OriginalCoder.Common.Extensions
{
    [PublicAPI]
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Computes the MD5 hash of <paramref name="file"/> and returns it formatted as a string.
        /// </summary>
        public static string ComputeHashMd5([NotNull] this FileInfo file)
        {                                    
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            return ComputeHash(file, new MD5CryptoServiceProvider());
        }

        /// <summary>
        /// Computes the SHA-1 hash of <paramref name="file"/> and returns it formatted as a string.
        /// </summary>
        public static string ComputeHashSha1([NotNull] this FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            return ComputeHash(file, new SHA1CryptoServiceProvider());
        }

        /// <summary>
        /// Computes the hash of <paramref name="file"/> using the specified <paramref name="hashAlgorithm"/> and returns it formatted as a string.
        /// </summary>
        public static string ComputeHash([NotNull] this FileInfo file, HashAlgorithm hashAlgorithm)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            using (var stream = file.OpenRead())
            {
                var hash = hashAlgorithm.ComputeHash(stream);
                var computed = BitConverter.ToString(hash).Replace("-", "");
                return computed;
            }
        }
    }
}
