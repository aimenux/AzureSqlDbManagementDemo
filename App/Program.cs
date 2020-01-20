using System;

namespace App
{
    public static class Program
    {
        public static void Main()
        {
            const string clientId = "!PUT YOUR CLIENT ID HERE!";
            const string clientSecret = "!PUT YOUR CLIENT SECRET HERE!";
            const string tenantId = "!PUT YOUR TENANT ID HERE!";

            var azure = AzureFactory.CreateAzure(clientId, clientSecret, tenantId);
            var manager = new AzureManager(azure);
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
