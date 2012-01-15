namespace ServiceModelContrib.Tests
{
    using System;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;

    public static class ServiceTestContext
    {
        static ServiceTestContext()
        {
            Binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
        }

        public static Binding Binding { get; set; }

        public static ServiceTestBuilder Test<TServiceImpl>()
        {
            return new ServiceTestBuilder(typeof (TServiceImpl));
        }

        private static void Execute(ServiceTestBuilder builder)
        {
            var uri = new Uri(builder.ServiceAddress + builder.ContractType.Name); // Disambiguate
            var host = Activator.CreateInstance(builder.ServiceHostType, builder.ServiceType, uri) as ServiceHost;

            if (builder.IncludeExceptionDetails)
            {
                var sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
                sdb.IncludeExceptionDetailInFaults = true;
            }
            host.AddServiceEndpoint(builder.ContractType, builder.Binding, string.Empty);
            host.Open();

            object client = typeof (ChannelFactory<>).MakeGenericType(builder.ContractType).InvokeMember(
                "CreateChannel",
                BindingFlags.Public | /* public static method */
                BindingFlags.Static |
                BindingFlags.InvokeMethod,
                null, /* default binder */
                null, /* no instance - static method */
                new object[]
                    {
                        builder.Binding, /* WCF Binding */
                        new EndpointAddress(uri) /* Service Endpoint */
                    });

            OperationContextScope ctx = null;
            if (builder.EstablishOperationContextScope)
            {
                ctx = new OperationContextScope((IContextChannel) client);
            }

            builder.ClientAction.DynamicInvoke(client);

            if (ctx != null)
            {
                ctx.Dispose();
            }

            host.Abort(); // Fail-fast (instead of .Close() )
        }

        #region Nested type: ServiceTestBuilder

        public class ServiceTestBuilder
        {
            private Binding _binding;
            private Uri _serviceAddress;

            public ServiceTestBuilder(Type serviceType)
            {
                ServiceType = serviceType;
                EstablishOperationContextScope = true;
            }

            public Type ServiceHostType { get; private set; }
            public Type ServiceType { get; private set; }
            public Type ContractType { get; private set; }
            public Delegate ClientAction { get; private set; }
            public bool EstablishOperationContextScope { get; private set; }
            public bool IncludeExceptionDetails { get; private set; }

            public Binding Binding
            {
                get
                {
                    if (_binding == null)
                    {
                        _binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
                    }
                    return _binding;
                }
                set { _binding = value; }
            }

            public Uri ServiceAddress
            {
                get
                {
                    if (_serviceAddress == null)
                    {
                        _serviceAddress = new Uri("net.pipe://localhost/test/");
                    }
                    return _serviceAddress;
                }
                set { _serviceAddress = value; }
            }

            public ServiceTestBuilder ExposeAt(string address)
            {
                ServiceAddress = new Uri(address);
                return this;
            }

            public ServiceTestBuilder WithBinding(Binding binding)
            {
                Binding = binding;
                return this;
            }

            public ServiceTestBuilder HostIn<TServiceHost>()
                where TServiceHost : ServiceHost
            {
                ServiceHostType = typeof (TServiceHost);
                return this;
            }

            public ServiceTestBuilder DisableOperationContext()
            {
                EstablishOperationContextScope = false;
                return this;
            }

            public ServiceTestBuilder FlowExceptionDetails()
            {
                IncludeExceptionDetails = true;
                return this;
            }

            public void FromContract<TContract>(Action<TContract> clientAction)
            {
                ContractType = typeof (TContract);
                ClientAction = clientAction;
                Execute(this);
            }
        }

        #endregion
    }
}