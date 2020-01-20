namespace App
{
    public class AzureSqlDbParameters
    {
        public string AdminLogin { get; set; }
        public string AdminPassword { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string ResourceGroupName { get; set; }
        public string FirewallRuleStartIpAddress { get; set; }
        public string FirewallRuleEndIpAddress { get; set; }
    }
}
