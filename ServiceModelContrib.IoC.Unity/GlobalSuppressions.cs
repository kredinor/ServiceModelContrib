// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace",
        Target = "KrediNor.ServiceModel.UnityIntegration")]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
        Target =
            "KrediNor.ServiceModel.UnityIntegration.ChannelFactoryContainerExtensions.#DynamicallyRegisterChannelFactory(Microsoft.Practices.Unity.IUnityContainer,System.String,System.Type,System.Type)"
        )]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
        Target =
            "KrediNor.ServiceModel.UnityIntegration.ChannelFactoryContainerExtensions.#DynamicallyRegisterChannelFactory(Microsoft.Practices.Unity.IUnityContainer,System.Type,System.Type)"
        )]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
        Target =
            "KrediNor.ServiceModel.UnityIntegration.ChannelFactoryContainerExtensions.#InnerRegisterChannelFactory`1(Microsoft.Practices.Unity.IUnityContainer,System.String,Microsoft.Practices.Unity.InjectionMember[])"
        )]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
        Target =
            "KrediNor.ServiceModel.UnityIntegration.ChannelFactoryContainerExtensions.#RegisterBinding(Microsoft.Practices.Unity.IUnityContainer,System.String,System.ServiceModel.Channels.Binding)"
        )]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
        Target =
            "KrediNor.ServiceModel.UnityIntegration.ChannelFactoryContainerExtensions.#RegisterChannelFactoryInstance(Microsoft.Practices.Unity.IUnityContainer,System.Type,System.Type,System.String,System.Object)"
        )]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
        Target =
            "KrediNor.ServiceModel.UnityIntegration.ChannelFactoryContainerExtensions.#RegisterEndpointAddress(Microsoft.Practices.Unity.IUnityContainer,System.String,System.ServiceModel.EndpointAddress)"
        )]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
        Target =
            "KrediNor.ServiceModel.UnityIntegration.HierarchicalLifetimeStrategy.#PreBuildUp(Microsoft.Practices.ObjectBuilder2.IBuilderContext)"
        )]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member",
        Target = "KrediNor.ServiceModel.UnityIntegration.UnityApplicationContainer.#CreateAndConfigureContainer()")]