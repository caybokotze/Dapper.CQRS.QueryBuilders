using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.CQRS
{
    /// <summary>
    /// The Service Activator acts as a means to fetch an instance of the statically registered IServiceProvider when registered in the IApplicationBuilder instance.
    /// </summary>
    public class ServiceActivator
    {
        internal static IServiceProvider _serviceProvider = null;

        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static IServiceScope GetScope(IServiceProvider serviceProvider = null)
        {
            var provider = serviceProvider ?? _serviceProvider;
            return provider?
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
        }
    }
}