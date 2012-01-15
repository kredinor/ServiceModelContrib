namespace ServiceModelContrib.Tests
{
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    [TestClass]
    public class SilverlightFaultTest
    {
        [TestMethod]
        public void ShouldReturnHttp200WhenRaisingAFault()
        {
            ServiceTestContext.Test<MockService>()
                .WithBinding(new BasicHttpBinding(BasicHttpSecurityMode.None))
                .ExposeAt("http://localhost:12345/")
                .HostIn<StubServiceHost>()
                .FlowExceptionDetails()
                .FromContract<IMockService>(client =>
                                                {
                                                    try
                                                    {
                                                        client.ShouldThrow();
                                                    }
                                                    catch
                                                    {
                                                        var property =
                                                            OperationContext.Current.IncomingMessageProperties[
                                                                HttpResponseMessageProperty.Name] as
                                                            HttpResponseMessageProperty;
                                                        Assert.AreEqual(HttpStatusCode.OK, property.StatusCode);
                                                    }
                                                });
        }
    }
}