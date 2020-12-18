using System;
using System.Data;
using System.IO;
using DapperDiddle.Enums;
using DapperDiddle.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace DapperDiddle
{
    public static class ConfigurationHelpers
    {
        public static void ConfigureDapperDiddleForMySql(
            this IServiceCollection services, 
            string connectionString, Dbms database)
        {
            switch (database)
            {
                case Dbms.MySql:
                {
                    ConfigureForMySql(services, connectionString);
                    break;
                }
                case Dbms.Default:
                {
                    throw new ArgumentException("The database you have selected is not yet supported.");
                }
                default:
                {
                    throw new ArgumentException("The database you have selected is not yet supported.");
                }
            }
        }

        private static void ConfigureForMySql(
            this IServiceCollection services, 
            string connectionString)
        {
            services.AddScoped<IBaseSqlExecutorOptions>(provider =>
                new BaseSqlExecutorDependencies()
                {
                    ConnectionString = "LKaldkfj",
                    Database = Dbms.MySql
                });
        }
    }

    public static class ServiceProviderFactory
    {
        public static IServiceProvider ServiceProvider { get; }

        static ServiceProviderFactory()
        {
            // HostingEnvironment env = new HostingEnvironment();
            // env.ContentRootPath = Directory.GetCurrentDirectory();
            // env.EnvironmentName = "Development";
            //
            // Startup startup = new Startup(env);
            // ServiceCollection sc = new ServiceCollection();
            // startup.ConfigureServices(sc);
            // ServiceProvider = sc.BuildServiceProvider();
        }
    }
}