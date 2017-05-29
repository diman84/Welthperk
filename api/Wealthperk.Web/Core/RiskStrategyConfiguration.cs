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
        private Dictionary<string, RiskStrategyDescription> _riskStrategies;

        public RiskStrategyConfiguration(IOptions<AppOptions> options)
        {
            _riskStrategies = options.Value.RiskStrategies.ToDictionary(c=>c.Key, x=>
                new RiskStrategyDescription {
                    Name=x.Name,
                    Description = x.Description,
                    Fee = x.FeePercentage
                }
            );
        }

        public Dictionary<string, RiskStrategyDescription> RiskStrategies => _riskStrategies;
    }
}