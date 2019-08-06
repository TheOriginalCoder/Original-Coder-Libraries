//=============================================================================
// Original Coder - Layer Library - Configuration & Implementation Example
//-----------------------------------------------------------------------------
// Visit the Original Coder Blog at http://OriginalCoder.dev
// Hosted at https://github.com/TheOriginalCoder/Original-Coder-Libraries
//-----------------------------------------------------------------------------
// Copyright (C) 2019 by James B. Higgins
// Released under the GNU LESSER GENERAL PUBLIC LICENSE Version 3
//=============================================================================

using OriginalCoder.Layers.Config;
using OriginalCoder.Layers.Interfaces;

namespace LayerApiMockup
{
    /// <summary>
    /// Creating and configuring the <see cref="IOcLayerConfiguration{IMyRequest}"/> can be done where ever or how ever it is preferred.
    /// The below is simply for demonstration purposes.
    /// </summary>
    public static class LayerConfig
    {
        private static IMyLayerConfiguration BuildAndConfigure()
        {
            var config = new MyLayerConfiguration(new MyAuthorizationService(), new MyMapperService());

            // This configuration step is completely optional, skipping it will allow any type of layer to be used.
            config.ConfigureAllowedLayerTypes(OcLayerType.Api, OcLayerType.Adapter, OcLayerType.Business, OcLayerType.Repository);

            // This configuration step is completely optional, skipping it will allow layer type in the system to access any other layer type.
            config.ConfigureLayerRestrictions(OcLayerType.Api, OcLayerType.Adapter);  // Restricts API layers so that they can only communicate with Adapter layers
            config.ConfigureLayerRestrictions(OcLayerType.Adapter, OcLayerType.Adapter, OcLayerType.Business);  // Restricts Adapter layers to other Adapters and Business layers
            config.ConfigureLayerRestrictions(OcLayerType.Business, OcLayerType.Business, OcLayerType.Repository);  // Restricts Business layers to other Business layers and Repositories
            config.ConfigureLayerRestrictions(OcLayerType.Repository);  // Restricts Repository layers so that they can not access any other layer (including other Repositories)

            // The below registers the various layers available to the system and provides a factory method for constructing them when needed
            // Note that it would be possible to use a standard IoC container implementation to handle construction.

            config.RegisterLayer(SystemConstants.Resource_Customer, OcLayerType.Api, request => new MyCustomerApiController(config));
            config.RegisterLayer(SystemConstants.Resource_Customer, OcLayerType.Adapter, request => new MyCustomerApiAdapter(config, request));
            config.RegisterLayer(SystemConstants.Resource_Customer, OcLayerType.Business, request => new MyCustomerBusinessLogic(config, request));
            config.RegisterLayer(SystemConstants.Resource_Customer, OcLayerType.Repository, request => new MyCustomerRepository(config, request));

            config.RegisterLayer(SystemConstants.Resource_Product, OcLayerType.Api, request => new MyProductApiController(config));
            config.RegisterLayer(SystemConstants.Resource_Product, OcLayerType.Adapter, request => new MyProductApiAdapter(config, request));
            config.RegisterLayer(SystemConstants.Resource_Product, OcLayerType.Business, request => new MyProductBusinessLogic(config, request));
            config.RegisterLayer(SystemConstants.Resource_Product, OcLayerType.Repository, request => new MyProductRepository(config, request));

            config.RegisterLayer(SystemConstants.Resource_Order, OcLayerType.Api, request => new MyOrderApiController(config));
            config.RegisterLayer(SystemConstants.Resource_Order, OcLayerType.Adapter, request => new MyOrderApiAdapter(config, request));
            config.RegisterLayer(SystemConstants.Resource_Order, OcLayerType.Business, request => new MyOrderBusinessLogic(config, request));
            config.RegisterLayer(SystemConstants.Resource_Order, OcLayerType.Repository, request => new MyOrderRepository(config, request));

            return config;
        }

        public static IMyLayerConfiguration Config
        {
            get
            {
                lock (_lock)
                {
                    if (_config == null)
                        _config = BuildAndConfigure();
                    return _config;
                }
            }
        }
        private static IMyLayerConfiguration _config;
        private static readonly object _lock = new object();
    }
}
