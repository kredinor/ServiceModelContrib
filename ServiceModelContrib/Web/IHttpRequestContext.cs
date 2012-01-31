namespace ServiceModelContrib.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Net.Mime;

    public interface IHttpRequestContext
    {
        WebHeaderCollection Headers { get; }
        IEnumerable<string> IfMatch { get; }
        IEnumerable<string> IfNoneMatch { get; }
        DateTime? IfUnmodifiedSince { get; }
        string Method { get; }
        DateTime? IfModifiedSince { get; }
        string Accept { get; }
        long ContentLength { get; }
        string ContentType { get; }
        UriTemplateMatch UriTemplateMatch { get; }
        string UserAgent { get; }
        Collection<ContentType> GetAcceptHeaderElements();

        void CheckConditionalRetrieve(object etag);
        void CheckConditionalUpdate(object etag);
    }
}