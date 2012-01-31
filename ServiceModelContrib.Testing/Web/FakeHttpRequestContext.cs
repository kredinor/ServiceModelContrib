namespace ServiceModelContrib.Testing.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Net.Mime;
    using ServiceModelContrib.Web;

    public class FakeHttpRequestContext : IHttpRequestContext
    {
        private Collection<ContentType> _headerElements;
        private Action<object> _conditionalRetrieveAction;
        private Action<object> _conditionalUpdateAction;


        public WebHeaderCollection Headers { get; set; }
        public IEnumerable<string> IfMatch { get; set; }
        public IEnumerable<string> IfNoneMatch { get; set; }
        public DateTime? IfUnmodifiedSince { get; set; }
        public string Method { get; set; }
        public DateTime? IfModifiedSince { get; set; }
        public string Accept { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public UriTemplateMatch UriTemplateMatch { get; set; }
        public string UserAgent { get; set; }


        public void SetAcceptHeaderElements(Collection<ContentType> headerElements)
        {
            _headerElements = headerElements;
        }

        public Collection<ContentType> GetAcceptHeaderElements()
        {
            return _headerElements;
        }

        public void SetConditionalRetrieveAction(Action<object> action)
        {
            _conditionalRetrieveAction = action;
        }

        public void CheckConditionalRetrieve(object etag)
        {
            _conditionalRetrieveAction(etag);
        }

        public void SetConditionalUpdateAction(Action<object> action)
        {
            _conditionalUpdateAction = action;
        }

        public void CheckConditionalUpdate(object etag)
        {
            _conditionalUpdateAction(etag);
        }
    }
}
