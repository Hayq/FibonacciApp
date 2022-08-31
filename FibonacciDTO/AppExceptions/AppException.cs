using System.Net;

namespace FibonacciDTO.AppExceptions
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Detailes { get; set; }

        public AppException(string message = null, object detailes = null) : base(message)
        {
            Detailes = detailes;
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
