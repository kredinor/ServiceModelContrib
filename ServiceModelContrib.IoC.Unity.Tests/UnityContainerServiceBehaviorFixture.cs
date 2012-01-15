namespace ServiceModelContrib.IoC.Unity.Tests
{
    using System.ServiceModel;
    using Mocks;
    using ServiceModelContrib.Tests.Mocks;
    using Xunit;

    public class UnityContainerServiceBehaviorFixture
    {
        public readonly EndpointAddress ServiceEndpointAddress = new EndpointAddress("net.tcp://localhost:7890");

        [Fact]
        public void BuildProgramaticallyServiceInstanceTest()
        {
            UnityApplicationContainer.SetInstanceForTest(UnityContainerMother.GetContainerWithMockLogger());
            var serviceHost = new UnityEnabledServiceHost(typeof (MockService));
            var binding = new NetTcpBinding();
            serviceHost.Open();

            IMockService proxy = ChannelFactory<IMockService>.CreateChannel(binding, ServiceEndpointAddress);
            const string input = "DoOperation()";

            proxy.DoOperation(input);
            proxy.DoOperation(input);
            Assert.Equal(proxy.GetLastLogEntry(), input);

            ((ICommunicationObject) proxy).Close();
            serviceHost.Close();
            UnityApplicationContainer.SetInstanceForTest(null);
        }
    }
}