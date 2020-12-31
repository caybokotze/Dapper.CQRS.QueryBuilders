using DapperDoodle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestProject
{
    public static class ServiceProvider
    {
        public static void CreateHost()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<ServiceTester>();
            }).Build();
            host.Run();
        }
    }

    public class ServiceTester
    {
        public ServiceTester(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDapperDoodle(null, DBMS.SQLite);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServiceActivator.Configure(app.ApplicationServices);
        }
    }
}