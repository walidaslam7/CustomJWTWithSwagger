using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Blogifier.Shared.ViewModels.ResponseModels
{
    public class HttpStatusMessage
    {
        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }
    }

    public class Response<T>
    {
        public T response { get; set; }
        public HttpStatusMessage responseStatus { get; set; }

        public void SetResponse(T responseModel, HttpStatusCode statusCode, object message)
        {
            response = responseModel;
            responseStatus = new() { statusCode = statusCode, message = message.ToString() };
        }
    }
}
