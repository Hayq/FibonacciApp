using FibonacciService.FibonacciGenerator;

namespace FibonacciApp.FibonacciUtils
{
    public static class FibAppConfiguration
    {
        public static void SetupFibonacciServices(this IServiceCollection appServices)
        {
            appServices.AddTransient<IFibNumGenerator, FibNumGenerator>();
            appServices.AddControllers(options =>
            {
                //options.Filters.Add<HttpResponseExceptionFilter>();//TODO:
            });

            appServices.AddEndpointsApiExplorer();
            appServices.AddSwaggerGen();
        }

        public static void SetupFibonacciWebApp(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }
}
