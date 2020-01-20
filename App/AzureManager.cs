using System;
using System.Linq;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Sql.Fluent;

namespace App
{
    public class AzureManager : IAzureManager
    {
        private readonly IAzure _azure;

        public AzureManager(IAzure azure)
        {
            _azure = azure;
        }

        public ISqlServer CreateAzureSqlServer(AzureSqlDbParameters parameters)
        {
            var (ownerKey, ownerValue) = new Tuple<string, string>("Owner", "AzureSqlDbManagementDemo");  

            var sqlServer = _azure.SqlServers.Define(parameters.ServerName)
                .WithRegion(Region.EuropeWest)
                .WithNewResourceGroup(parameters.ResourceGroupName)
                .WithAdministratorLogin(parameters.AdminLogin)
                .WithAdministratorPassword(parameters.AdminPassword)
                .WithNewFirewallRule(parameters.FirewallRuleStartIpAddress, parameters.FirewallRuleEndIpAddress)
                .WithTag(ownerKey, ownerValue)
                .Create();

            Console.WriteLine($"Sql Server '{parameters.ServerName}' was created with id '{sqlServer.Id}'");

            var database = sqlServer.Databases
                .Define(parameters.DatabaseName)
                .WithBasicEdition(SqlDatabaseBasicStorage.Max2Gb)
                .WithTag(ownerKey, ownerValue)
                .Create();

            Console.WriteLine($"Sql database '{parameters.DatabaseName}' was created with id '{database.Id}'");

            return sqlServer;
        }

        public void DeleteAzureSqlServer(ISqlServer sqlServer)
        {
            var rgName = sqlServer.ResourceGroupName;

            var firewallRules = sqlServer.FirewallRules.List();
            foreach (var firewallRule in firewallRules)
            {
                Console.WriteLine($"Firewall rule '{firewallRule.Name}' was deleted");
                firewallRule.Delete();
            }

            var sqlDatabases = sqlServer.Databases.List().Where(x => !IsMaster(x));
            foreach (var sqlDatabase in sqlDatabases)
            {
                Console.WriteLine($"Sql database '{sqlDatabase.Name}' was deleted");
                sqlDatabase.Delete();
            }

            Console.WriteLine($"Sql server '{sqlServer.Name}' was deleted");
            _azure.SqlServers.DeleteById(sqlServer.Id);

            Console.WriteLine($"Resource group '{rgName}' was deleted");
            _azure.ResourceGroups.DeleteByName(rgName);
        }

        private static bool IsMaster(IHasName sqlDatabase)
        {
            return string.Equals(sqlDatabase.Name, "master", StringComparison.OrdinalIgnoreCase);
        }
    }
}