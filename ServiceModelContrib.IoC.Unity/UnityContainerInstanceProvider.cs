namespace ServiceModelContrib.IoC.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Text;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Custom WCF Instance Provider that uses the Unity Container to initialize new service instances.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class UnityContainerInstanceProvider : IInstanceProvider
    {
        private readonly IUnityContainer _parentContainer;

        private readonly Dictionary<object, IUnityContainer> _requestContainers =
            new Dictionary<object, IUnityContainer>();

        private readonly Type _serviceType;

        /// <summary>
        /// Initializes a new <c>UnityContainerInstanceProvider</c>.
        /// </summary>
        /// <param name="serviceType">The WCF service type.</param>
        public UnityContainerInstanceProvider(Type serviceType)
            : this(serviceType, null)
        {
        }

        /// <summary>
        /// Initializes a new <c>UnityContainerInstanceProvider</c>.
        /// </summary>
        /// <param name="serviceType">The WCF service type.</param>
        /// <param name="parentContainer">Parent Unity container</param>
        public UnityContainerInstanceProvider(Type serviceType, IUnityContainer parentContainer)
        {
            _serviceType = serviceType;
            _parentContainer = parentContainer;
        }

        #region IInstanceProvider Members

        /// <summary>
        /// Returns a service object given the specified System.ServiceModel.InstanceContext object.
        /// </summary>
        /// <param name="instanceContext">The current System.ServiceModel.InstanceContext object.</param>
        /// <returns>A user-defined service object.</returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Returns a service object given the specified System.ServiceModel.InstanceContext object.
        /// </summary>
        /// <param name="instanceContext">The current System.ServiceModel.InstanceContext object.</param>
        /// <param name="message">The message that triggered the creation of a service object.</param>
        /// <returns>A user-defined service object.</returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            try
            {
                IUnityContainer requestContainer = _parentContainer.CreateChildContainer();
                object serviceInstance = requestContainer.Resolve(_serviceType);

                lock (_requestContainers)
                {
                    _requestContainers.Add(serviceInstance, requestContainer);
                }

                return serviceInstance;
            }
            catch (Exception ex)
            {
                string loadedAssemblies = string.Join(Environment.NewLine,
                                                      AppDomain.CurrentDomain.GetAssemblies().Select(a => a.FullName).
                                                          ToArray());
                var innerMessage = new StringBuilder();

                for (Exception innerException = ex.InnerException;
                     innerException != null;
                     innerException = innerException.InnerException)
                {
                    innerMessage.AppendFormat("\n============================\nInner exception:\n{0}",
                                              innerException.Message);
                }
                string exceptionMessage =
                    string.Format(
                        "Failed resolving {0}. Make sure all relevant assemblies that contain UnityRegistry subclasses are loaded. To force assembly loading use a comma separated list of assembly names in the appSettings section with key {1}.  Currently loaded assemblies are: {2}{3}",
                        _serviceType.Name, UnityApplicationContainer.UnityRegistryAssembliesAppSettingsKey,
                        loadedAssemblies, innerMessage);

                //Log.Error(exceptionMessage, ex);

                throw new ConfigurationErrorsException(exceptionMessage, ex);
            }
        }

        /// <summary>
        /// Called when an System.ServiceModel.InstanceContext object recycles a service object.
        /// </summary>
        /// <param name="instanceContext">The service's instance context.</param>
        /// <param name="instance">The service object to be recycled.</param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            lock (_requestContainers)
            {
                IUnityContainer unityContainer = _requestContainers[instance];
                _requestContainers.Remove(instance);
                unityContainer.Dispose();
            }

            var disposable = instance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        #endregion
    }
}