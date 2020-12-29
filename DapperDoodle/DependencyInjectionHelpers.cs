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
            return factory.CreateServiceProvider(serviceCollection);
        }
    }
}