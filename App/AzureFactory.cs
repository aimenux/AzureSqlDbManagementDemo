using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace App
{
    public static class AzureFactory
    {
        public static IAzure CreateAzure(Settings settings)
        {
            return CreateAzure(settings.ClientId, settings.ClientSecret, settings.TenantId);
        }

        public static IAzure CreateAzure(string clientId, string clientSecret, string tenantId)
        {
            var credentials = SdkContext
                .AzureCredentialsFactory
                .FromServicePrincipal(
                    clientId,
                    clientSecret,
                    tenantId,
                    AzureEnvironment.AzureGlobalCloud);

            var azure = Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials)
                .WithDefaultSubscription();

            return azure;
        }
    }
}
