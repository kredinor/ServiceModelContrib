# ServiceModelContrib

A (at the moment) minimal set of helpers for robust WCF development. Use the helper methods in ChannelHelper.cs to handle client proxies properly and wire up SilverlightFaultBehavior to return faults back to the browser with a HTTP status code 200 so that the fault reaches the Silverlight client.

Package can be installed from the official Nuget feed:
<div>
  <p>
  	<code style="background-color: #202020; border: 4px solid silver; border-bottom-left-radius: 5px 5px; border-bottom-right-radius: 5px 5px; border-top-left-radius: 5px 5px; border-top-right-radius: 5px 5px; color: #E2E2E2; display: block; font: normal normal normal 1.5em/normal 'andale mono', 'lucida console', monospace; line-height: 1.5em; overflow: auto; padding: 15px;">PM> Install-Package ServiceModelContrib</code>
  </p>
</div>

# ServiceModelContrib.IoC.Unity

Integrates Unity into the client- and service-side pipelines.
## Client-side
import the ServiceModelContrib.IoC.Unity namespace (using) and use the extensions method in the ChannelFactoryContainerExtensions class. 
The following code:

    using ServiceModelContrib.IoC.Unity;
    ...
    IUnityContainer container = ...;
    container.RegisterAllEndpointsFromConfiguration();
    
will read up the system.serviceModel/clients section from the configuration file (app or web.config) and register lazy loaded channel factories for the given channel types in the container. On resolve, a concrete channel factory will be returned. Proper cleanup is done by the container. The class contains other methods for doing the registration programmatically as well.

## Service-side
Either use the UnityEnabledServiceHost/UnityEnabledServiceHostFactory, or add a reference (either programmatically or in configuration) to UnityContainerServiceBehavior/UnityContainerBehaviorElement to use Unity for service instantiation.

The behavior uses child container functionality in Unity to enable an application-wide container and an inherited container on a per-request basis. To "plug into" the container configuration, derive from the UnityRegistry class in your code and register your services etc. there. This will ensure that all the dependencies are loaded in time for registration (something that can be a real hassle with ASP.NET Web application and lazy loading of assembly dependencies).

Package can be installed from the official Nuget feed:
<div>
  <p>
  	<code style="background-color: #202020; border: 4px solid silver; border-bottom-left-radius: 5px 5px; border-bottom-right-radius: 5px 5px; border-top-left-radius: 5px 5px; border-top-right-radius: 5px 5px; color: #E2E2E2; display: block; font: normal normal normal 1.5em/normal 'andale mono', 'lucida console', monospace; line-height: 1.5em; overflow: auto; padding: 15px;">PM> Install-Package ServiceModelContrib.IoC.Unity</code>
  </p>
</div>