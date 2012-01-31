namespace ServiceModelContrib.Testing.Web
{
    using System;
    using System.Net;
    using System.ServiceModel.Web;
    using System.Text;
    using ServiceModelContrib.Web;

    public class FakeHttpResponseContext : IHttpResponseContext
    {
        public Encoding BindingWriteEncoding { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public string ETag { get; set; }
        public WebMessageFormat? Format { get; set; }
        public WebHeaderCollection Headers { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public Uri StatusAsCreatedLocationUri { get; private set; }
        public bool StatusAsNotFoundSet { get; set; }
        public string StatusAsNotFoundDescription { get; set; }


        public void SetETag(object etag)
        {
            ETag = etag.ToString();
        }

        public void SetStatusAsCreated(Uri locationUri)
        {
            StatusAsCreatedLocationUri = locationUri;
        }

        public void SetStatusAsNotFound()
        {
            StatusAsNotFoundSet = true;
        }

        public void SetStatusAsNotFound(string description)
        {
            StatusAsNotFoundDescription = description;
        }
    }
}