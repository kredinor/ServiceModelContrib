namespace ServiceModelContrib.Testing.Web
{
    using ServiceModelContrib.Web;

    public class FakeHttpOperationContext : IHttpOperationContext
    {
        public FakeHttpOperationContext()
        {
            Request = new FakeHttpRequestContext();
            Response = new FakeHttpResponseContext();
        }

        public IHttpRequestContext Request { get; set; }

        public IHttpResponseContext Response { get; set; } 
    }
}