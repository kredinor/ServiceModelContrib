namespace ServiceModelContrib.IoC.Unity
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Configuration;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.StaticFactory;

    /// <summary>
    /// Utility methods to quickly register a new WCF <c>ChannelFactory</c> for a service channel.
    /// </summary>
    public static class ChannelFactoryContainerExtensions
    {
        private const string RegisterFactoryMethodName = "RegisterFactory";
        private const string CreateChannelMethodName = "CreateChannel";
        private const string SystemServiceModelClientSectionName = "system.serviceModel/client";
        private const string IMetadataExchangeTypeName = "IMetadataExchange";
        private const string WildcardTypeName = "*";

        private static readonly object[] NoParameters = new object[] {};
        private static readonly Type ChannelFactoryType = typeof (ChannelFactory<>);
        private static readonly Type[] NoParameterTypes = new Type[] {};

        private static readonly Type[] StringAndFactoryDelegateAsParameters = new[]
                                                                                  {
                                                                                      typeof (string),
                                                                                      typeof (Func<IUnityContainer, object>
                                                                                          )
                                                                                  };

        private static readonly Type[] FactoryDelegateAsParameter = new[] {typeof (Func<IUnityContainer, object>)};
        private static readonly Type[] StringAsOnlyParameter = new[] {typeof (string)};
        // ReSharper disable InconsistentNaming
        private static readonly Type IStaticFactoryConfigurationType = typeof (IStaticFactoryConfiguration);
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Registers a new ChannelFactory for TChannel binding it to a registered binding and endpoint.
        /// </summary>
        /// <typeparam name="TChannel">WCF channel type.</typeparam>
        /// <param name="container">Unity container to extend.</param>
        /// <param name="bindingName">Name of the registered binding (in the Unity container).</param>
        /// <param name="endpointName">Name of the registered endpoint address (in the Unity Container).</param>
        /// <param name="clientName"></param>
        /// <returns>The Unity container - to enable chaining of calls.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IUnityContainer RegisterChannelFactory<TChannel>(this IUnityContainer container,
                                                                       string bindingName,
                                                                       string endpointName, string clientName)
        {
            var injectionMembers = new InjectionMember[]
                                       {
                                           new InjectionConstructor(
                                               new ResolvedParameter<Binding>(bindingName),
                                               new ResolvedParameter<EndpointAddress>(endpointName)
                                               )
                                       };
            InnerRegisterChannelFactory<TChannel>(container, clientName, injectionMembers);
            return container;
        }

        /// <summary>
        /// Registers a new ChannelFactory for TChannel directly binding it to a Binding and EndpointAddress.
        /// </summary>
        /// <typeparam name="TChannel">WCF channel type.</typeparam>
        /// <param name="container">Unity container to extend.</param>
        /// <param name="name"></param>
        /// <param name="binding">Binding instance to use in channel factory.</param>
        /// <param name="endpointAddress">EndpointAddress instance to use in channel factory.</param>
        /// <returns>The Unity container - to enable chaining of calls.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IUnityContainer RegisterChannelFactory<TChannel>(this IUnityContainer container,
                                                                       string name,
                                                                       Binding binding,
                                                                       EndpointAddress endpointAddress)
        {
            var injectionMembers = new InjectionMember[]
                                       {
                                           new InjectionConstructor(
                                               new InjectionParameter<Binding>(binding),
                                               new InjectionParameter<EndpointAddress>(endpointAddress)
                                               )
                                       };
            InnerRegisterChannelFactory<TChannel>(container, name, injectionMembers);
            return container;
        }

        ///<summary>
        ///</summary>
        ///<param name="container"></param>
        ///<param name="channelFactoryType"></param>
        ///<param name="channelType"></param>
        ///<param name="channelFactory"></param>
        ///<returns></returns>
        public static IUnityContainer RegisterChannelFactoryInstance(this IUnityContainer container,
                                                                     Type channelFactoryType,
                                                                     Type channelType, object channelFactory)
        {
            return RegisterChannelFactoryInstance(container, channelFactoryType, channelType, null, channelFactory);
        }

        /// <summary>
        /// Registers an existing channel factory instance.
        /// </summary>
        /// <param name="container">Unity container to extend.</param>
        /// <param name="channelFactoryType">Channel factory type.</param>
        /// <param name="channelType">The channel type.</param>
        /// <param name="clientName">The client name.</param>
        /// <param name="channelFactory">Channel factory instance.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterChannelFactoryInstance(this IUnityContainer container,
                                                                     Type channelFactoryType,
                                                                     Type channelType, string clientName,
                                                                     object channelFactory)
        {
            EnsureThatStaticFactoryExtensionIsLoaded(container);
            if (clientName == null)
            {
                container.RegisterInstance(channelFactoryType, channelFactory, new ContainerControlledLifetimeManager());
                DynamicallyRegisterChannelFactory(container, channelFactoryType, channelType);
            }
            else
            {
                container.RegisterInstance(channelFactoryType, clientName, channelFactory,
                                           new ContainerControlledLifetimeManager());
                DynamicallyRegisterChannelFactory(container, clientName, channelFactoryType, channelType);
            }
            return container;
        }

        private static void DynamicallyRegisterChannelFactory(IUnityContainer container, string clientName,
                                                              Type channelFactoryType, Type channelType)
        {
            MethodInfo methodInfo = IStaticFactoryConfigurationType.GetMethod(RegisterFactoryMethodName,
                                                                              StringAndFactoryDelegateAsParameters);
            MethodInfo registerFactoryMethodInfo = methodInfo.MakeGenericMethod(new[] {channelType});

            Func<IUnityContainer, object> factoryDelegate = innerContainer =>
                                                                {
                                                                    object channelFactory =
                                                                        innerContainer.Resolve(channelFactoryType,
                                                                                               clientName);
                                                                    object channel = channelFactoryType.GetMethod(
                                                                        CreateChannelMethodName,
                                                                        NoParameterTypes)
                                                                        .Invoke(channelFactory, NoParameters);
                                                                    innerContainer.RegisterInstance(channel,
                                                                                                    new ContainerControlledCommunicationObjectLifetimeManager
                                                                                                        ());
                                                                    return channel;
                                                                };

            registerFactoryMethodInfo.Invoke(container.Configure<IStaticFactoryConfiguration>(),
                                             new object[] {clientName, factoryDelegate});
        }

        private static void DynamicallyRegisterChannelFactory(IUnityContainer container, Type channelFactoryType,
                                                              Type channelType)
        {
            MethodInfo methodInfo = IStaticFactoryConfigurationType.GetMethod(RegisterFactoryMethodName,
                                                                              FactoryDelegateAsParameter);
            MethodInfo registerFactoryMethodInfo = methodInfo.MakeGenericMethod(new[] {channelType});

            Func<IUnityContainer, object> factoryDelegate = innerContainer =>
                                                                {
                                                                    object channel = channelFactoryType.GetMethod(
                                                                        CreateChannelMethodName,
                                                                        NoParameterTypes)
                                                                        .Invoke(
                                                                            innerContainer.Resolve(channelFactoryType)
                                                                            , NoParameters);
                                                                    innerContainer.RegisterInstance(channel,
                                                                                                    new ContainerControlledCommunicationObjectLifetimeManager
                                                                                                        ());
                                                                    return channel;
                                                                };

            registerFactoryMethodInfo.Invoke(container.Configure<IStaticFactoryConfiguration>(),
                                             new object[] {factoryDelegate});
        }

        /// <summary>
        /// Registers a <c>Binding</c> instance in the container.
        /// </summary>
        /// <param name="container">Unity container to extend.</param>
        /// <param name="name">Name to register the binding instance with in the container.</param>
        /// <param name="binding">Binding instance.</param>
        /// <returns>The Unity container - to enable chaining of calls.</returns>
        public static IUnityContainer RegisterBinding(this IUnityContainer container, string name, Binding binding)
        {
            return container.RegisterInstance(name, binding, new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Registers an <c>EndpointAddress</c> instance in the container.
        /// </summary>
        /// <param name="container">Unity container to extend.</param>
        /// <param name="name">Name to register the endpoint address instance with in the container.</param>
        /// <param name="endpointAddress">EndpointAddress instance.</param>
        /// <returns>The Unity container - to enable chaining of calls.</returns>
        public static IUnityContainer RegisterEndpointAddress(this IUnityContainer container, string name,
                                                              EndpointAddress endpointAddress)
        {
            return container.RegisterInstance(name, endpointAddress, new ContainerControlledLifetimeManager());
        }

        /// <summary>           
        /// Registers channel factories for all endpoints in the system.serviceModel/clients section in the configuration file.
        /// </summary>
        /// <param name="container">Unity container to extend.</param>
        /// <returns>The Unity container - to enable chaining of calls.</returns>
        public static IUnityContainer RegisterEndpointsFromConfiguration(this IUnityContainer container)
        {
            var clientSection = ConfigurationManager.GetSection(SystemServiceModelClientSectionName) as ClientSection;
            if (clientSection != null)
            {
                foreach (ChannelEndpointElement endpointElement in clientSection.Endpoints)
                {
                    // Ignore metadata endpoints.
                    if (endpointElement.Contract == IMetadataExchangeTypeName)
                        continue;

                    // Ignore wildcard endpoints (routing).
                    if (endpointElement.Contract == WildcardTypeName)
                        continue;

                    Type channelType = GetChannelType(endpointElement);

                    if (channelType == null)
                        throw new ConfigurationErrorsException("Could not find the contract type for " +
                                                               endpointElement.Contract);


                    Type concreteChannelFactoryType = ChannelFactoryType.MakeGenericType(channelType);
                    ConstructorInfo constructorInfo = concreteChannelFactoryType.GetConstructor(StringAsOnlyParameter);
                    if (constructorInfo == null)
                    {
                        throw new InvalidOperationException(
                            "The type did not contain a constructor with one string-based parameter.");
                    }
                    object channelFactory = constructorInfo.Invoke(new object[] {endpointElement.Name});

                    int noOfClientsWithSameContract = GetNoOfClientsWithSameContract(clientSection, endpointElement);

                    container.RegisterChannelFactoryInstance(concreteChannelFactoryType, channelType,
                                                             endpointElement.Name, channelFactory);
                    if (noOfClientsWithSameContract == 1)
                    {
                        container.RegisterChannelFactoryInstance(concreteChannelFactoryType, channelType, channelFactory);
                    }
                }
            }
            return container;
        }

        private static int GetNoOfClientsWithSameContract(ClientSection clientSection,
                                                          ChannelEndpointElement endpointElement)
        {
            return clientSection.Endpoints.Cast<ChannelEndpointElement>()
                .Count(x => InvariantCompareOfContractName(x, endpointElement));
        }

        private static bool InvariantCompareOfContractName(ChannelEndpointElement x,
                                                           ChannelEndpointElement endpointElement)
        {
            return string.Compare(x.Contract, endpointElement.Contract, StringComparison.InvariantCulture) == 0;
        }

        private static Type GetChannelType(ChannelEndpointElement endpointElement)
        {
            ChannelEndpointElement element = endpointElement;

            return (from asm in AppDomain.CurrentDomain.GetAssemblies()
                    let contractType = asm.GetType(element.Contract)
                    where contractType != null
                    select contractType)
                .FirstOrDefault();
        }

        private static void InnerRegisterChannelFactory<TChannel>(this IUnityContainer container,
                                                                  string name,
                                                                  InjectionMember[] injectionMembers)
        {
            EnsureThatStaticFactoryExtensionIsLoaded(container);

            Func<IUnityContainer, object> factoryDelegate = innerContainer =>
                                                                {
                                                                    TChannel channel =
                                                                        innerContainer.Resolve<ChannelFactory<TChannel>>
                                                                            ()
                                                                            .CreateChannel();
                                                                    innerContainer.RegisterInstance(channel,
                                                                                                    new ContainerControlledCommunicationObjectLifetimeManager
                                                                                                        ());
                                                                    return channel;
                                                                };

            container.RegisterType<ChannelFactory<TChannel>>(new ContainerControlledLifetimeManager(),
                                                             injectionMembers)
                .Configure<IStaticFactoryConfiguration>()
                .RegisterFactory<TChannel>(factoryDelegate)
                .RegisterFactory<TChannel>(name, factoryDelegate);
        }

        private static void EnsureThatStaticFactoryExtensionIsLoaded(IUnityContainer container)
        {
            if (container.Configure<IStaticFactoryConfiguration>() == null)
            {
#pragma warning disable 612,618
                container.AddNewExtension<StaticFactoryExtension>();
#pragma warning restore 612,618
            }
        }
    }
}