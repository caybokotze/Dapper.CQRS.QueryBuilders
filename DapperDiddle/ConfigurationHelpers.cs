using System.Data;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace DapperDiddle
{
    public static class ConfigurationHelpers
    {
        public static void ConfigureDapperDiddleForMySql(
            this IServiceCollection services, 
            string connectionString)
        {
            services.AddScoped<IBaseSqlExecutor>
                (executor => new BaseSqlExecutor
                (new MySqlConnection(connectionString)));
        }
    }
}