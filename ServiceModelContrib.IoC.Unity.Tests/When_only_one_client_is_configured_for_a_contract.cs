namespace ServiceModelContrib.IoC.Unity.Tests
{
    using System.Diagnostics;
    using Microsoft.Practices.Unity;
    using ServiceModelContrib.Tests.Mocks;
    using Xunit;

    public class When_only_one_client_is_configured_for_a_contract
    {
        private UnityContainer _container;
        private IMockService2 _mockServiceClient;

        public When_only_one_client_is_configured_for_a_contract()
        {
            Arrange();
        }

        private void Arrange()
        {
            // ensure that the IMockService type / ServiceContrib.Tests assembly is loaded into the current appdomain.
            var hack = typeof (IMockService);
            var hack2 = hack.Name;
            Debug.WriteLine(hack2);
            _container = new UnityContainer();
            _container.RegisterEndpointsFromConfiguration();
        }

        [Fact]
        public void Should_be_able_to_resolve_a_client_without_endpoint_name()
        {
            _mockServiceClient = _container.Resolve<IMockService2>();
            Assert.NotNull(_mockServiceClient);
        }

        [Fact]
        public void Should_be_able_to_resolve_a_client_with_an_endpoint_name()
        {
            _mockServiceClient = _container.Resolve<IMockService2>("mockClient3");
            Assert.NotNull(_mockServiceClient);
        }

        [Fact]
        public void Cleanup()
        {
            _container.Dispose();
        }
    }
}