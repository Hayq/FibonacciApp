using System.Net;

namespace FibonacciDTO.AppException
{
    public class AppException : Exception
    {
        public int StatusCode { get; set; }
        public object Detailes { get; set; }

        public AppException(string message = null, object detailes = null) : base(message)
        {
            Detailes = detailes;
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
