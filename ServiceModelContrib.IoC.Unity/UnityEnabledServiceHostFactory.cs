namespace ServiceModelContrib.IoC.Unity
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    ///<summary>
    ///</summary>
    public class UnityEnabledServiceHostFactory : ServiceHostFactory
    {
        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address. 
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"/> for the type of service specified with a specific base address.
        /// </returns>
        /// <param name="serviceType">Specifies the type of service to host. </param><param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.</param>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new UnityEnabledServiceHost(serviceType, baseAddresses);
        }
    }
}