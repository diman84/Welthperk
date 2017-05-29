using System.Collections.Generic;

namespace Wealthperk.Model
{
    public interface IRiskStrategyConfiguration
    {
        Dictionary<string, RiskStrategy> RiskStrategies { get; }
    }
}