using System.Collections.Generic;

namespace Wealthperk.Model
{
    public interface IRiskStrategyConfiguration
    {
        Dictionary<string, RiskStrategyDescription> RiskStrategies { get; }
    }

    public class RiskStrategyDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Fee { get; set; }
    }
}