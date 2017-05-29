namespace Wealthperk.Web.Core
{
    public class AppOptions
    {
        public RiskOptions[] RiskStrategies { get; set; }
    }

    public class RiskOptions
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? FeePercentage { get; set; }
    }
}