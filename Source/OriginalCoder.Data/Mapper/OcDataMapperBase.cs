//=============================================================================
// Original Coder - Data Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2010, 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using OriginalCoder.Common.Api;
using OriginalCoder.Common.Extensions;

namespace OriginalCoder.Data.Mapper
{
    public abstract class OcDataMapperBase : IOcDataMapper
    {
        /// <inheritdoc />
        public abstract bool CanConvert<TIn, TOut>() where TIn : class where TOut : class;

        /// <inheritdoc />
        public abstract TOut Convert<TIn, TOut>(TIn entity) where TIn : class where TOut : class;

        /// <inheritdoc />
        public List<TOut> ConvertToList<TIn, TOut>(IEnumerable<TIn> listIn) where TIn : class where TOut : class
        {
            if (listIn == null)
                return null;

            try
            {
                var listOut = listIn.Where(i => i != null).Select(Convert<TIn, TOut>).Where(converted => converted != null).ToList();
                return listOut.Count == 0 ? null : listOut;
            }
            catch (OcDataMappingException)
            { throw; }
            catch (Exception ex)
            { throw new OcDataMappingException($"Exception trying to convert data from IEnumerable<{typeof(TIn).FriendlyName()}> to List<{typeof(TOut).FriendlyName()}>", ex, typeof(TIn), typeof(TOut)); }
        }

        /// <inheritdoc />
        public OcApiResult<TOut> ConvertToApiResult<TIn, TOut>(IOcApiResult<TIn> apiResultIn) where TIn : class where TOut : class
        {
            if (apiResultIn == null)
                return null;
            try
            {
                return new OcApiResult<TOut>(apiResultIn.Name, Convert<TIn, TOut>(apiResultIn.Data), apiResultIn.Messages);
            }
            catch (OcDataMappingException)
            { throw; }
            catch (Exception ex)
            { throw new OcDataMappingException($"Exception trying to convert data from OcApiResult<{typeof(TIn).FriendlyName()}> to OcApiResult<{typeof(TOut).FriendlyName()}>", ex, typeof(TIn), typeof(TOut)); }
        }

        /// <inheritdoc />
        public OcApiResult<IReadOnlyList<TOut>> ConvertToApiResultList<TIn, TOut>(IOcApiResult<IReadOnlyList<TIn>> apiResultList) where TIn : class where TOut : class
        {
            if (apiResultList == null)
                return null;

            try
            {
                var listOut = apiResultList.Data.Where(i => i != null).Select(Convert<TIn, TOut>).Where(converted => converted != null).ToList();
                return new OcApiResult<IReadOnlyList<TOut>>(apiResultList.Name, listOut.Count == 0 ? null : listOut, apiResultList.Messages);
            }
            catch (OcDataMappingException)
            { throw; }
            catch (Exception ex)
            { throw new OcDataMappingException($"Exception trying to convert data from IOcApiResult<IReadOnlyList<{typeof(TIn).FriendlyName()}> to OcApiResult<IReadOnlyList<{typeof(TOut).FriendlyName()}>", ex, typeof(TIn), typeof(TOut)); }
        }
    }
}