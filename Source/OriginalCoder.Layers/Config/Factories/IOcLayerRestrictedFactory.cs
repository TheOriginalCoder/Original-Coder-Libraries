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
using OriginalCoder.Layers.Interfaces;
using OriginalCoder.Layers.Requests;

namespace OriginalCoder.Layers.Config.Factories
{
    /// <summary>
    /// Restricted factory for obtaining typed resources and resource layers.
    /// Access will be restricted according to <see cref="LayerType"/> and <see cref="LayerTypeName"/> to enforce any configured restrictions for interaction between layers.
    /// </summary>
    [PublicAPI]
    public interface IOcLayerRestrictedFactory<TRequest> : IOcLayerFactory<TRequest>
        where TRequest : class, IOcRequest
    {
        /// <summary>
        /// The Layer Type assigned to this factory.  Used to impose restrictions between layer interactions.
        /// If this is assigned <see cref="OcLayerType.Other"/> then <see cref="LayerTypeName"/> will specify the layer type.
        /// </summary>
        OcLayerType LayerType { get; }

        /// <summary>
        /// The Layer Type Name assigned to this factory.  Used to impose restrictions between layer interactions.
        /// This will match <see cref="LayerType"/> unless it is assigned <see cref="OcLayerType.Other"/>.
        /// </summary>
        string LayerTypeName { get; }
    }
}