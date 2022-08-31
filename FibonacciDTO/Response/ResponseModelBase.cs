using System.Net;

namespace FibonacciDTO.Response
{
    public class ResponseModelBase
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public object ErrorMessage { get; set; }
        public object Data { get; set; }
    }
}
