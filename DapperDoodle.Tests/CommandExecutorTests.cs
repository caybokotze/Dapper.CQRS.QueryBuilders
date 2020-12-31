using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace DapperDoodle.Tests
{
    [TestFixture]
    public class CommandExecutorTests
    {
        [TestFixture]
        public class Registrations
        {
            [Test]
            public async Task AssertThatIApplicationBuilderRegistersCommandExecutor()
            {
                var hostBuilder = new HostBuilder().ConfigureWebHost(webhost =>
                {
                    webhost.UseTestServer();
                    webhost.Configure(app =>
                    {
                        app.Run(handle => handle
                            .Response
                            .StartAsync());
                    });

                    webhost.ConfigureServices(config =>
                    {
                        config
                            .ConfigureDapperDoodle(null, DBMS.SQLite);
                    });
                });

                var host = await hostBuilder.StartAsync();
                var serviceProvider = host.Services;
                
                var commandExecutor = serviceProvider
                    .GetService<ICommandExecutor>();
                var result = commandExecutor.Execute(new CommandInheritor());
            
                Assert.AreEqual(result, 9);
            }
            
            public class CommandInheritor : Command<int>
            {
                public override void Execute()
                {
                    Result = SelectQuery<int>("Select 9;");
                }
            }
        }
    }
}