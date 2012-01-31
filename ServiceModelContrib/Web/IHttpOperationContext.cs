namespace ServiceModelContrib.Web
{
    public interface IHttpOperationContext
    {
        IHttpRequestContext Request { get; }
        IHttpResponseContext Response { get; }
    }
}