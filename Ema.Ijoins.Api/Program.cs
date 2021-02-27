using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Ema.Ijoins.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging((hostingContext, logging) =>
                {
                  logging.ClearProviders();
                  logging.AddConsole();
                  logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                  logging.AddNLog();
                })
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
              //webBuilder.UseUrls("http://localhost:5000", "https://localhost:5001");
            });
  }
}
