using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.BusinessLogic
{
    
    public static class Response
    {
        public static Response<T> Fail<T>(string message,HttpStatusCode statusCode, T data = default) => 
            new Response<T>(data, message, true,statusCode);

        public static Response<T> Ok<T>(string message, T data, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            new Response<T>(data, message, false, statusCode);
    }

    public class Response<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        
        public Response(T data,string message,bool error, HttpStatusCode statusCode)
        {
            Data = data;
            Message = message;
            Error = error;
            StatusCode = statusCode;
        }
    }
}



