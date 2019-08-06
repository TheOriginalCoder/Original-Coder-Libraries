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

namespace OriginalCoder.Layers.Interfaces
{
    /// <summary>
    /// Enumeration which provides a broad assortment of layer types that might be found in a software system.
    /// Systems are expected to use only layer types which they need and make sense for their design and purpose.
    /// </summary>
    [PublicAPI]
    public enum OcLayerType
    {
        Unknown, 

        // Layers related to security or validation
        Authentication, Authorization, Security, Validation,

        // Layers which are generally the top-layer responsible for handling incoming requests or user interactions
        Api, Controller, Presentation, UserInterface, View,

        // Layers for modifying or adapting the data passed between other layers
        Adapter, Consolidation, Translation,
        
        // Layers which perform some type of work
        Analysis, Application, Batch, Business, Domain, Facade, Indirection, Infrastructure, Job, Logic, Logging, Process, Report, Service,

        // Layers which handle data (reading and/or writing)
        Cache, DataAccess, Persistence, Export, Import, InOut, Repository, Storage,

        // Layers which interact with remote or networked systems
        Network, Remote,

        // Used to indicate a layer type not included in this enumeration
        Other,
    }
}