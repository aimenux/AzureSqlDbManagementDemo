using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace App
{
    public static class Program
    {
        public static void Main()
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            services.Configure<Settings>(configuration.GetSection(nameof(Settings)));
            services.AddSingleton<IAzureManager>(provider =>
            {
                var settings = provider.GetService<IOptions<Settings>>().Value;
                var azure = AzureFactory.CreateAzure(settings);
                return new AzureManager(azure);
            });
            
            var serviceProvider = services.BuildServiceProvider();

            var manager = serviceProvider.GetService<IAzureManager>();

            var parameters = AzureSqlDbParametersFactory.CreateDefaultAzureSqlDbParameters();

            var sqlServer = manager.CreateAzureSqlServer(parameters);

            const ConsoleKey deleteKey = ConsoleKey.D;
            Console.WriteLine($"Press '{deleteKey}' to delete previous created resources ..");
            while (!IsKeyPressed(deleteKey))
            {
            }

            manager.DeleteAzureSqlServer(sqlServer);

            Console.WriteLine("Press any key to exit program !");
            Console.ReadKey();
        }

        private static bool IsKeyPressed(ConsoleKey key)
        {
            return Console.KeyAvailable && Console.ReadKey(true).Key == key;
        }
    }
}
