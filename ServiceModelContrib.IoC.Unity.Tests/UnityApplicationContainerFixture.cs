namespace ServiceModelContrib.IoC.Unity.Tests
{
    using Microsoft.Practices.Unity;
    using Xunit;

    public class UnityApplicationContainerFixture
    {
        [Fact]
        public void ShouldScanForUnityRegistriesWhenAccessingSingleton()
        {
            IUnityContainer unityContainer = UnityApplicationContainer.Instance;
            Assert.Equal(42, unityContainer.Resolve<int>());
        }
    }

    public class MyRegistry : UnityRegistry
    {
        public override void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterInstance(42);
        }
    }
}