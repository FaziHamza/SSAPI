using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SSAPI.Core;
using SSAPI;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var endpointReader = host.Services.GetRequiredService<IEndpointReader>();
        await endpointReader.ReadAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);
            })
         .ConfigureServices((hostContext, services) =>
         {
             services.Configure<AppSettings>(hostContext.Configuration);
             services.AddSingleton(x => x.GetRequiredService<IOptions<AppSettings>>().Value);
             services.AddTransient<IEndpointReader, EndpointReader>();
         });

}
