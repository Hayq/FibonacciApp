using System.Net;

namespace FibonacciDTO.Response
{
    public class ResponseModelBase
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
        public object Error { get; set; }
        public object Data { get; set; }
    }
}
