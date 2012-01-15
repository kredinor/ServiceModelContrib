namespace ServiceModelContrib.IoC.Unity
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// Custom WCF Service Behavior that enables the use of Unity - an IoC/DI container for service instance buildup.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"),
     AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class UnityContainerServiceBehavior : Attribute, IServiceBehavior
    {
        #region IServiceBehavior Members

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects 
        /// such as error handlers, message or parameter interceptors, security extensions, and other 
        /// custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription,
                                          ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = channelDispatcherBase as ChannelDispatcher;

                if (channelDispatcher == null)
                {
                    continue;
                }

                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.InstanceProvider =
                        new UnityContainerInstanceProvider(serviceDescription.ServiceType,
                                                           UnityApplicationContainer.Instance);
                }
            }
        }

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">The service description of the service.</param>
        /// <param name="serviceHostBase">The host of the service.</param>
        /// <param name="endpoints">The service endpoints.</param>
        /// <param name="bindingParameters">Custom objects to which binding elements have access.</param>
        public void AddBindingParameters(ServiceDescription serviceDescription,
                                         ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
            // Not used in this behavior.
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm 
        /// that the service can run successfully.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The service host that is currently being constructed.</param>
        public void Validate(ServiceDescription serviceDescription,
                             ServiceHostBase serviceHostBase)
        {
            // Not used in this behavior.
        }

        #endregion
    }
}