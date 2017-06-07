using System.Collections.Generic;

namespace Wealthperk.Model
{
    public interface ICalculationConfiguration
    {
        int YearsAtRetirement { get; }
        int YearsForPrediction { get; }
        double DefaultGrowth { get; }
    }
}