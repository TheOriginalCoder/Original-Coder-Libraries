//=============================================================================
// Original Coder - Layer Library
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using JetBrains.Annotations;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Config.Factories
{
    /// <summary>
    /// Restricted factory for obtaining typed resources and resource layers with additional methods to more easily obtain layers that support Read, Read + Search, CRUD and CRUD + Search operations.
    /// Access will be restricted according to <see cref="IOcLayerRestrictedFactory{TRequest}.LayerType"/> and <see cref="IOcLayerRestrictedFactory{TRequest}.LayerTypeName"/> to enforce any configured restrictions for interaction between layers.
    /// </summary>
    [PublicAPI]
    public interface IOcLayerRestrictedCrudFactory<TRequest> : IOcLayerRestrictedFactory<TRequest>, IOcLayerCrudFactory<TRequest>
        where TRequest : class, IOcRequest
    { }
}