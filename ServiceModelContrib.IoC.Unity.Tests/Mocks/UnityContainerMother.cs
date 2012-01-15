namespace ServiceModelContrib.IoC.Unity.Tests.Mocks
{
    using Microsoft.Practices.Unity;
    using ServiceModelContrib.Tests.Mocks;

    public class UnityContainerMother
    {
        public static UnityContainer GetContainerWithMockLogger()
        {
            var container = new UnityContainer();
            container.RegisterType<ILogger, MockLogger>();
            return container;
        }
    }
}