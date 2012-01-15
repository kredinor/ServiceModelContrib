namespace KrediNor.ServiceModel.Tests.UnityIntegration
{
    using Microsoft.Practices.Unity;
    using ServiceModelContrib.IoC.Unity;
    using ServiceModelContrib.Tests.Mocks;
    using Xunit;

    public class When_more_than_one_client_is_configured_with_the_same_contract
    {
        private UnityContainer _container;
        private IMockService _mockServiceClient;

        public When_more_than_one_client_is_configured_with_the_same_contract()
        {
            Arrange();
            Act();
        }

        private void Arrange()
        {
            _container = new UnityContainer();
            _container.RegisterEndpointsFromConfiguration();
        }

        private void Act()
        {
            _mockServiceClient = _container.Resolve<IMockService>("mockClient");
        }

        [Fact]
        public void Should_resolve_to_a_client_instance()
        {
            Assert.NotNull(_mockServiceClient);
        }
    }

    public class LoggerA : ILogger
    {
        private string _message;

        #region ILogger Members

        public void Log(string message)
        {
            _message = message;
        }

        public string GetLastEntry()
        {
            return _message;
        }

        #endregion
    }

    internal class LoggerB : ILogger
    {
        private string _message;

        #region ILogger Members

        public void Log(string message)
        {
            _message = message;
        }

        public string GetLastEntry()
        {
            return _message;
        }

        #endregion
    }
}