using Biblio.Server.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();

            // Create a host and build it with all its dependencies.
            var host = CreateHostBuilder(args).Build();

            // Scoped instance needed for service access.
            using (var scope = host.Services.CreateScope())
            {
                // Get the service provider.
                var services = scope.ServiceProvider;

                try
                {
                    // Get the database context.
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    // Call to init the database.
                    DatabaseInitializer.InitDb(services, context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
