using FibonacciApp.FibonacciUtils;
using FibonacciService.FibonacciGenerator;

namespace FibonacciApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.SetupFibonacciServices(builder);

            var app = builder.Build();
            app.SetupFibonacciWebApp();

            app.Run();
        }
    }
}