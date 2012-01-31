namespace ServiceModelContrib.Web
{
    using System;
    using System.Net;
    using System.ServiceModel.Web;
    using System.Text;

    public interface IHttpResponseContext
    {
        Encoding BindingWriteEncoding { get;  }
        long ContentLength { get; set; }
        string ContentType { get; set; }
        string ETag { get; set; }
        WebMessageFormat? Format { get; set; }
        WebHeaderCollection Headers { get; }
        HttpStatusCode StatusCode { get; set; }
        string StatusDescription { get; set; }

        void SetETag(object etag);
        void SetStatusAsCreated(Uri locationUri);
        void SetStatusAsNotFound();
        void SetStatusAsNotFound(string description);
    }
}