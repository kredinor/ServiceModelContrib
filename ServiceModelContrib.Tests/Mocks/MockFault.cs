namespace ServiceModelContrib.Tests.Mocks
{
    using System.Runtime.Serialization;

    [DataContract]
    public class MockFault
    {
        [DataMember(Order = 0, IsRequired = true)]
        public string Reason { get; set; }
    }
}