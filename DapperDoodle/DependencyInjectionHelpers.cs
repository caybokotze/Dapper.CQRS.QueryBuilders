using System;
using Microsoft.Extensions.DependencyInjection;

namespace DapperDoodle
{
    public static class DependencyInjectionHelpers
    {
        public static IServiceProvider GetServiceProviderInstance()
        {
            var factory = new DefaultServiceProviderFactory();
            var serviceCollection = new ServiceCollection();
            factory.CreateBuilder(serviceCollection);
            return factory.CreateServiceProvider(serviceCollection);
        }
    }
}