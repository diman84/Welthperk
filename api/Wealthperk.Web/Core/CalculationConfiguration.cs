using System;
using Microsoft.Extensions.Options;
using Wealthperk.Model;

namespace Wealthperk.Web.Core
{
    public class CalculationConfiguration : ICalculationConfiguration
    {
        private int _retirement;
        private int _years;
        private double _growth;

        public int YearsAtRetirement => _retirement;

        public double DefaultGrowth => _growth;

        public int YearsForPrediction => _years;

        public CalculationConfiguration(IOptions<AppOptions> options)
        {
            _retirement = (options.Value?.Calculation?.FutureYou?.YearsAtRetirement) ?? 65;
            _growth = (options.Value?.Calculation?.FutureYou?.AnnualGrowth) ?? 0.05;
            _years = (options.Value?.Calculation?.FutureYou?.YearsForPrediction) ?? 25;
        }
    }
}