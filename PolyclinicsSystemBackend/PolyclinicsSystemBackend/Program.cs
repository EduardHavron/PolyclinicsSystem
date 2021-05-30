using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace PolyclinicsSystemBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureServices(services => services.AddAutofac())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("AppSettings/appsettings.json", false, true);
                    config.AddJsonFile(
                        "AppSettings/appsettings." + hostingContext.HostingEnvironment.EnvironmentName + ".json", true,
                        true);
                });
        }
    }
}