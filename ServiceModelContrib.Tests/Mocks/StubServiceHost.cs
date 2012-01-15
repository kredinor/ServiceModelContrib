namespace ServiceModelContrib.Tests.Mocks
{
    using System;
    using System.ServiceModel;

    public class StubServiceHost : ServiceHost
    {
        public StubServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        public StubServiceHost(object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
        }

        protected override void ApplyConfiguration()
        {
            base.ApplyConfiguration();

            Description.Behaviors.Add(new SilverlightFaultBehavior());
        }
    }
}