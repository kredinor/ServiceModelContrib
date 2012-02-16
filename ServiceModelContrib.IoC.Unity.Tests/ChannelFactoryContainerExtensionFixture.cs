namespace ServiceModelContrib.IoC.Unity.Tests
{
    using System;
    using System.ServiceModel;
    using Microsoft.Practices.Unity;
    using ServiceModelContrib.Tests.Mocks;
    using Xunit;

    public class ChannelFactoryContainerExtensionFixture : IDisposable
    {
        private readonly ChannelFactory<IMockServiceClient> _channelFactory;
        private readonly ServiceHost _host;

        public ChannelFactoryContainerExtensionFixture()
        {
            _channelFactory = CreateChannelFactory();
            _host = new ServiceHost(typeof (MockService), new Uri("net.pipe://testpipe/"));
            _host.AddServiceEndpoint(typeof (IMockService), new NetNamedPipeBinding(NetNamedPipeSecurityMode.None),
                                     string.Empty);
            _host.Open();
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_host != null)
            {
                ChannelHelper.ProperClose(_host);
            }
        }

        #endregion

        public void Teardown()
        {
            ChannelHelper.ProperClose(_host);
        }

        [Fact]
        public void Should_register_all_clients_from_configuration_in_container()
        {
            using (var container = new UnityContainer())
            {
                container.RegisterEndpointsFromConfiguration();
                var client = container.Resolve<IMockService>("mockClient");
                var client2 = container.Resolve<IMockService>("mockClient2");
                Assert.NotNull(client);
                Assert.NotNull(client2);
            }
        }

        [Fact]
        public void Should_invoke_action_on_ChannelFactory_upon_configuration()
        {
            using (var container = new UnityContainer())
            {
                bool checker = false;
                container.RegisterEndpointsFromConfiguration(cf =>
                                                                 {
                                                                     checker = true;
                                                                 });
                Assert.Equal(true, checker);
            }
        }

        [Fact]
        public void Should_be_able_to_resolve_a_named_client()
        {
            using (var container = new UnityContainer())
            {
                container.RegisterEndpointsFromConfiguration();
                var client = container.Resolve<IMockService>("mockClient");
                Assert.NotNull(client);
            }
        }

        [Fact]
        public void Should_handle_resolving_channels_from_registered_channel_factory_instances()
        {
            using (var container = new UnityContainer())
            {
                container.RegisterChannelFactoryInstance(typeof (ChannelFactory<IMockServiceClient>),
                                                         typeof (IMockServiceClient),
                                                         "client", _channelFactory);

                var channel = container.Resolve<IMockServiceClient>("client");
                Assert.NotNull(channel);
            }
        }

        [Fact]
        public void Should_properly_close_the_client_channel()
        {
            IMockServiceClient channel = null;
            using (var container = new UnityContainer())
            {
                container.RegisterChannelFactoryInstance(typeof (ChannelFactory<IMockServiceClient>),
                                                         typeof (IMockServiceClient),
                                                         "client", _channelFactory);

                channel = container.Resolve<IMockServiceClient>("client");
                channel.Open();
            }
            Assert.NotEqual(CommunicationState.Opened, channel.State);
        }

        private static ChannelFactory<IMockServiceClient> CreateChannelFactory()
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            var endpointAddress = new EndpointAddress("net.pipe://testpipe/");
            return new ChannelFactory<IMockServiceClient>(binding, endpointAddress);
        }
    }
}