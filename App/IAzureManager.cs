using Microsoft.Azure.Management.Sql.Fluent;

namespace App
{
    public interface IAzureManager
    {
        ISqlServer CreateAzureSqlServer(AzureSqlDbParameters parameters);
        void DeleteAzureSqlServer(ISqlServer sqlServer);
    }
}
