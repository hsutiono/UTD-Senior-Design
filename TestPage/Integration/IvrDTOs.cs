using System;
using System.Net;

namespace TestPage.Integration
{
    public class SessionTokenModel
    {
        public string AuthenticationToken { get; set; }
        public string Message { get; set; }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
    }

    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}