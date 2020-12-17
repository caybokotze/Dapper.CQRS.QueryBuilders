using System;
using System.Data;
using DapperDiddle.Enums;
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
            services.AddScoped<IBaseSqlExecutor>(provider =>
                new BaseSqlExecutor(
                    new MySqlConnection(connectionString)));
        }
    }
}