namespace ServiceModelContrib.Web
{
    using System.ServiceModel.Web;

    public class HttpOperationContext : IHttpOperationContext
    {
        private readonly IHttpRequestContext _requestContext;
        private readonly IHttpResponseContext _responseContext;

        public HttpOperationContext(IHttpRequestContext requestContext, IHttpResponseContext responseContext)
        {
            _requestContext = requestContext;
            _responseContext = responseContext;
        }

        public HttpOperationContext(WebOperationContext webOperationContext)
        {
            _requestContext = new HttpRequestContext(webOperationContext.IncomingRequest);
            _responseContext = new HttpResponseContext(webOperationContext.OutgoingResponse);
        }

        public IHttpRequestContext Request
        {
            get { return _requestContext; }
        }

        public IHttpResponseContext Response
        {
            get { return _responseContext; }
        }
    }
}