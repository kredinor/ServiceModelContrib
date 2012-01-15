namespace ServiceModelContrib.Tests.Mocks
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IMockService
    {
        [OperationContract]
        [FaultContract(typeof (LoggerNotPresentException))]
        void DoOperation(string message);

        [OperationContract]
        [FaultContract(typeof (LoggerNotPresentException))]
        string GetLastLogEntry();

        [OperationContract]
        string FlowUserName();

        [OperationContract(IsOneWay = true)]
        void OneWay();

        [FaultContract(typeof (MockFault))]
        [OperationContract]
        void ShouldThrow();
    }

    public interface IMockServiceClient : IMockService, IClientChannel
    {
    }
}