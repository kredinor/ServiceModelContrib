namespace ServiceModelContrib.IoC.Unity.Tests.Mocks
{
    using System;
    using Microsoft.Practices.Unity;

    public class StubServiceHost : UnityEnabledServiceHost
    {
        private IUnityContainer _parentContainer;

        public StubServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        public StubServiceHost(object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
        }

        protected override IUnityContainer ParentContainer
        {
            get { return _parentContainer; }
        }

        public void SetParentContainer(IUnityContainer parentContainter)
        {
            _parentContainer = parentContainter;
        }
    }
}