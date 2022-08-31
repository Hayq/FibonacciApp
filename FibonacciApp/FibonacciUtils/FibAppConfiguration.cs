using FibonacciApp.GlobalExceptionHandling;
using FibonacciService.FibonacciGenerator;
using Microsoft.AspNetCore.Mvc;

namespace FibonacciApp.FibonacciUtils
{
    public static class FibAppConfiguration
    {
        public static void SetupFibonacciServices(this IServiceCollection appServices, WebApplicationBuilder builder)
        {
            appServices.AddTransient<IFibNumGenerator, FibNumGenerator>();
            appServices.AddControllers(options =>
            {
                options.CacheProfiles.SetupCacheProfiles(builder);
            });

            appServices.AddResponseCaching();
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

            app.UseMiddleware(typeof(GlobalErrorHandlingMiddleware));
            app.UseCors();
            app.UseAuthorization();

            app.MapControllers();
        }

        private static void SetupCacheProfiles(
            this IDictionary<string, CacheProfile> cacheProfiles,
            WebApplicationBuilder builder)
        {
            var cacheProfileConfigs = builder.Configuration
                .GetSection("CacheProfiles")
                .GetChildren();

            foreach (var cacheProfile in cacheProfileConfigs)
            {
                cacheProfiles.Add(
                    cacheProfile.Key,
                    cacheProfile.Get<CacheProfile>());
            }
        }
    }
}
