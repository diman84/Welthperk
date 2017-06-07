using System;
using System.Globalization;

namespace Wealthperk.Model
{
    public class PortfolioStrategy
    {
        public Contribution ContributionStrategy { get; set; }
        public string RiskStrategy { get; set; }
    }

    public class Contribution
    {
        public double? Salary { get; set; }
        public Frequency? ContributionFrequency { get; set; }
        public double? SalaryPercent { get; set; }
        public double? CompanyMatch { get; set; }

        public double? AnnualContribution()
        {
            return GetContribution(Salary);
        }

        public double? AmountPerFrequency()
        {
            var dfi = DateTimeFormatInfo.CurrentInfo;
            var cal = dfi.Calendar;
            var lastDay = new DateTime(DateTime.Now.Year,12,31);
            double? salaryPerFrequency = null;
            switch (ContributionFrequency){
                case Frequency f when (f == Frequency.Weekly):
                    salaryPerFrequency = Salary / cal.GetWeekOfYear(lastDay, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                    break;
                case Frequency f when (f == Frequency.BiWeekly):
                     salaryPerFrequency =  Salary / cal.GetWeekOfYear(lastDay, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) / 2;
                     break;
                case Frequency f when (f == Frequency.Monthly):
                    salaryPerFrequency =  Salary / 12;
                     break;
                case null:
                default:
                    break;
            }
            return GetContribution(salaryPerFrequency);
        }

        private double? GetContribution(double? amount)
        {
            return amount * (SalaryPercent + (CompanyMatch ?? 0));
        }
    }

    public enum Frequency
    {
        Weekly,
        BiWeekly,
        Monthly
    }
}