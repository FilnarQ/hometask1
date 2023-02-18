using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hometask1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    services.Configure<Config>(configuration.GetSection(nameof(Config)));
                    services.AddHostedService<Worker>();
                    services.AddSingleton<FileWatcher>();
                    services.AddScoped<FileProcessor>();
                    services.AddScoped<MetaLogger>();
                });
    }
}
