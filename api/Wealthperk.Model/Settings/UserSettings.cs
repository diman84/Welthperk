namespace Wealthperk.Model
{
    public class PortfolioStrategy
    {
        public Contribution ContributionStrategy { get; set; }
        public string RiskStrategy { get; set; }
    }

    public class Contribution
    {
        public int? Amount { get; set; }
        public Frequency? Frequency { get; set; }
        public string Description { get; set; }
    }

    public enum Frequency
    {
        Weekly,
        BiWeekly,
        Monthly
    }
}