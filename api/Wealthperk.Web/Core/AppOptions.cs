using System.Collections.Generic;

namespace Wealthperk.Web.Core
{
    public class AppOptions
    {
        public IEnumerable<RiskOptions> RiskStrategies { get; set; }

        public CalculationOptions Calculation { get;set; }
    }

    public class CalculationOptions
    {
        public PredictionOptions FutureYou { get;set; }
    }

    public class PredictionOptions
    {
        public int? YearsAtRetirement { get; set; }
        public int? YearsForPrediction { get; set; }
        public double? AnnualGrowth { get; set; }
    }

    public class RiskOptions
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? FeePercentage { get; set; }
    }
}