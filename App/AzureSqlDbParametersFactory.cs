using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace App
{
    public static class AzureSqlDbParametersFactory
    {
        public static AzureSqlDbParameters CreateDefaultAzureSqlDbParameters()
        {
            var ipAddress = FindCurrentIpAddress();

            return new AzureSqlDbParameters()
            {
                AdminLogin = "AdminLoginDemo1234",
                AdminPassword = "AdminPasswordDemo1234",
                DatabaseName = GetRandomName("sqldb"),
                ServerName = GetRandomName("sqlserver"),
                ResourceGroupName = GetRandomName("rgsqlserver"),
                FirewallRuleStartIpAddress = ipAddress,
                FirewallRuleEndIpAddress = ipAddress
            };
        }

        private static string GetRandomName(string prefix, int maxLength = 20)
        {
            return SdkContext.RandomResourceName(prefix, maxLength);
        }

        private static string FindCurrentIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                ?.ToString();
        }
    }
}
