//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2010, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System.Collections.Generic;
using JetBrains.Annotations;
using OriginalCoder.Common.Api;

namespace OriginalCoder.Data.Mapper
{
    [PublicAPI]
    public interface IOcDataMapper
    {
        /// <summary>
        /// Returns true if the data adapter can convert <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TIn">Type of the input, must be a class</typeparam>
        /// <typeparam name="TOut">Type of the output, must be a class</typeparam>
        bool CanConvert<TIn, TOut>() where TIn : class where TOut : class;

        /// <summary>
        /// Converts <typeparamref name="TIn"/> into <typeparamref name="TOut"/> and returns it.
        /// Throws a <see cref="OcDataMappingException"/> if <typeparamref name="TIn"/> can not be converted into <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TIn">Type of the input, must be a class</typeparam>
        /// <typeparam name="TOut">Type of the output, must be a class</typeparam>
        TOut Convert<TIn, TOut>(TIn entity) where TIn : class where TOut : class;

        /// <summary>
        /// Converts a list of <typeparamref name="TIn"/> objects into a list of <typeparamref name="TOut"/> objects.
        /// Throws a <see cref="OcDataMappingException"/> if <typeparamref name="TIn"/> can not be converted into <typeparamref name="TOut"/>.
        /// </summary>
        List<TOut> ConvertToList<TIn, TOut>(IEnumerable<TIn> entities) where TIn : class where TOut : class;

        /// <summary>
        /// Converts a typed <see cref="OcApiResult"/> containing a <typeparamref name="TIn"/> into one containing a <typeparamref name="TOut"/>.
        /// Throws a <see cref="OcDataMappingException"/> exception if <typeparamref name="TIn"/> can not be converted into <typeparamref name="TOut"/>.
        /// </summary>
        OcApiResult<TOut> ConvertToApiResult<TIn, TOut>(IOcApiResult<TIn> apiResult) where TIn : class where TOut : class;

        /// <summary>
        /// Converts a typed <see cref="OcApiResult"/> containing a list of <typeparamref name="TIn"/> objects into one containing a list of <typeparamref name="TOut"/> objects.
        /// Throws a <see cref="OcDataMappingException"/> exception if <typeparamref name="TIn"/> can not be converted into <typeparamref name="TOut"/>.
        /// </summary>
        OcApiResult<IReadOnlyList<TOut>> ConvertToApiResultList<TIn, TOut>(IOcApiResult<IReadOnlyList<TIn>> entityList) where TIn : class where TOut : class;
    }
}