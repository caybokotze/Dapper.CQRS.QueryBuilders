using System;
using Microsoft.Extensions.DependencyInjection;

namespace DapperDoodle.Tests
{
    public class TestHelpers
    {
        public IServiceProvider GetServiceProviderInstance()
        {
            var factory = new DefaultServiceProviderFactory();
            var serviceCollection = new ServiceCollection();
            return factory.CreateServiceProvider(serviceCollection);
        }
    }
}