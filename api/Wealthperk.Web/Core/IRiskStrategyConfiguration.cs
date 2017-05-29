using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Wealthperk.Model;

namespace Wealthperk.Web.Core
{
    public class RiskStrategyConfiguration : IRiskStrategyConfiguration
    {
        private Dictionary<string, RiskStrategy> _riskStrategies;

        public RiskStrategyConfiguration(IOptions<AppOptions> options)
        {
            _riskStrategies = options.Value.RiskStrategies.ToDictionary(c=>c.Key, x=>
                new RiskStrategy {
                    Name=x.Name,
                    Description = x.Description,
                    Fee = x.Fee
                }
            );
        }

        public Dictionary<string, RiskStrategy> RiskStrategies => _riskStrategies;
    }

    public class RiskOptions
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Fee { get; set; }
    }
}