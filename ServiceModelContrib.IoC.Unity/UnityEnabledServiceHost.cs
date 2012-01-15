namespace ServiceModelContrib.IoC.Unity
{
    using System;
    using System.ServiceModel;
    using System.Web;
    using Microsoft.Practices.Unity;

    ///<summary>
    ///</summary>
    public class UnityEnabledServiceHost : ServiceHost
    {
        private const string ApplicationContainerName = "ApplicationContainer";

        ///<summary>
        ///</summary>
        ///<param name="serviceType"></param>
        ///<param name="baseAddresses"></param>
        public UnityEnabledServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="singletonSingletonInstance"></param>
        ///<param name="baseAddresses"></param>
        public UnityEnabledServiceHost(object singletonSingletonInstance, params Uri[] baseAddresses)
            : base(singletonSingletonInstance, baseAddresses)
        {
        }

        /// <summary>
        /// Gets the parent Unity Container.
        /// By default, it will look in the HttpRuntime.Cache object
        /// (key: ApplicationContainer) for it.
        /// </summary>
        /// <returns>The parent Unity container if found - else null.</returns>
        protected virtual IUnityContainer ParentContainer
        {
            get
            {
                if (HttpRuntime.Cache[ApplicationContainerName] != null)
                {
                    return HttpRuntime.Cache[ApplicationContainerName] as IUnityContainer;
                }
                return null;
            }
        }

        /// <summary>
        /// Loads the service description information from the configuration file and applies it to the runtime being constructed.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The description of the service hosted is null.</exception>
        protected override void ApplyConfiguration()
        {
            base.ApplyConfiguration();
            Description.Behaviors.Add(new UnityContainerServiceBehavior());
        }
    }
}