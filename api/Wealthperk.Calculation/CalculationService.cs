using System;
using Wealthperk.Model;

namespace Wealthperk.Calculation
{
    public interface ICalculationService
    {
        double PredictionForYears(double startBalance, double annualContribution, double? annualGrowth, int years);
    }

    public class CalculationService : ICalculationService
    {
        public double PredictionForYears(double startBalance, double annualContribution, double? annualGrowth, int years)
        {
            double result = startBalance;
            for (int i = 0; i < years; i++)
            {
                result = result * (1 + (annualGrowth ?? 0)) + annualContribution;
            }
            return result;
        }
    }
}
