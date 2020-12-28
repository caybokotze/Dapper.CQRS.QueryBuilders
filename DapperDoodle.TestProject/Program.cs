using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TestRunner;

namespace TestProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public static class ServiceProviderFactory
    {
        public static IServiceProvider ServiceProvider { get; }

        static ServiceProviderFactory()
        {
            var host = Host.CreateDefaultBuilder().ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                }).Build();
            ServiceProvider = host.Services;
            host.Run();
        }
    }
}