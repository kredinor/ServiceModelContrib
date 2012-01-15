namespace ServiceModelContrib.Tests.Mocks
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IMockService2
    {
        [OperationContract]
        string Operation1(string input);
    }
}