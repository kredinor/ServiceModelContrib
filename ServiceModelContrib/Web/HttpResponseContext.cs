namespace ServiceModelContrib.Web
{
    using System;
    using System.Net;
    using System.ServiceModel.Web;
    using System.Text;

    internal class HttpResponseContext : IHttpResponseContext
    {
        private readonly OutgoingWebResponseContext _outgoingResponse;

        public HttpResponseContext(OutgoingWebResponseContext outgoingResponse)
        {
            _outgoingResponse = outgoingResponse;
        }

        public Encoding BindingWriteEncoding
        {
            get { return _outgoingResponse.BindingWriteEncoding; }
        }

        public long ContentLength
        {
            get { return _outgoingResponse.ContentLength; }
            set { _outgoingResponse.ContentLength = value; }
        }

        public string ContentType
        {
            get { return _outgoingResponse.ContentType; }
            set { _outgoingResponse.ContentType = value; }
        }

        public string ETag
        {
            get { return _outgoingResponse.ETag; }
            set { _outgoingResponse.ETag = value; }
        }

        public WebMessageFormat? Format
        {
            get { return _outgoingResponse.Format; }
            set { _outgoingResponse.Format = value; }
        }

        public WebHeaderCollection Headers
        {
            get { return _outgoingResponse.Headers; }
        }

        public HttpStatusCode StatusCode
        {
            get { return _outgoingResponse.StatusCode; }
            set { _outgoingResponse.StatusCode = value; }
        }

        public string StatusDescription
        {
            get { return _outgoingResponse.StatusDescription; }
            set { _outgoingResponse.StatusDescription = value; }
        }

        public void SetStatusAsCreated(Uri locationUri)
        {
            _outgoingResponse.SetStatusAsCreated(locationUri);
        }

        public void SetStatusAsNotFound()
        {
            _outgoingResponse.SetStatusAsNotFound();
        }

        public void SetStatusAsNotFound(string description)
        {
            _outgoingResponse.SetStatusAsNotFound(description);
        }

        public void SetETag(object etag)
        {
            if (etag is Guid)
            { 
                _outgoingResponse.SetETag((Guid)etag);
                return;
            }
            if (etag is string)
            {
                _outgoingResponse.SetETag((string)etag);
                return;
            }
            if (etag is int)
            {
                _outgoingResponse.SetETag((int)etag);
                return;
            }
            if (etag is long)
            {
                _outgoingResponse.SetETag((long)etag);
            }
        }
    }
}