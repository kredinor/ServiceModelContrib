namespace ServiceModelContrib.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Net.Mime;
    using System.ServiceModel.Web;

    internal class HttpRequestContext : IHttpRequestContext
    {
        private readonly IncomingWebRequestContext _incomingRequest;

        public HttpRequestContext(IncomingWebRequestContext incomingRequest)
        {
            _incomingRequest = incomingRequest;
        }

        public WebHeaderCollection Headers
        {
            get { return _incomingRequest.Headers; }
        }

        public IEnumerable<string> IfMatch
        {
            get { return _incomingRequest.IfMatch; }
        }

        public IEnumerable<string> IfNoneMatch
        {
            get { return _incomingRequest.IfNoneMatch; }
        }

        public DateTime? IfUnmodifiedSince
        {
            get { return _incomingRequest.IfUnmodifiedSince; }
        }

        public string Method
        {
            get { return _incomingRequest.Method; }
        }

        public DateTime? IfModifiedSince
        {
            get { return _incomingRequest.IfModifiedSince; }
        }

        public string Accept
        {
            get { return _incomingRequest.Accept; }
        }

        public long ContentLength
        {
            get { return _incomingRequest.ContentLength; }
        }

        public string ContentType
        {
            get { return _incomingRequest.ContentType; }
        }

        public UriTemplateMatch UriTemplateMatch
        {
            get { return _incomingRequest.UriTemplateMatch; }
        }

        public string UserAgent
        {
            get { return _incomingRequest.UserAgent; }
        }

        public Collection<ContentType> GetAcceptHeaderElements()
        {
            return _incomingRequest.GetAcceptHeaderElements();
        }

        public void CheckConditionalRetrieve(object etag)
        {
            var stringETag = etag as string;
            if (stringETag != null)
            {
                _incomingRequest.CheckConditionalRetrieve(stringETag);
                return;
            }
            if (etag is Guid)
            {
                _incomingRequest.CheckConditionalRetrieve((Guid)etag);
                return;
            }
            if (etag is int)
            {
                _incomingRequest.CheckConditionalRetrieve((int)etag);
                return;
            }
            if (etag is long)
            {
                _incomingRequest.CheckConditionalRetrieve((long)etag);
            }
        }


        public void CheckConditionalUpdate(object etag)
        {
            var stringETag = etag as string;
            if (stringETag != null)
            {
                _incomingRequest.CheckConditionalUpdate(stringETag);
                return;
            }
            if (etag is Guid)
            {
                _incomingRequest.CheckConditionalUpdate((Guid)etag);
                return;
            }
            if (etag is int)
            {
                _incomingRequest.CheckConditionalUpdate((int)etag);
                return;
            }
            if (etag is long)
            {
                _incomingRequest.CheckConditionalUpdate((long)etag);
            }
        }
    }
}