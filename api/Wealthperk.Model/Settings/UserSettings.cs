namespace Wealthperk.Model
{
    public class PortfolioStrategy
    {
        public Contribution Contribution { get; set; }
        public RiskStrategy Risks { get; set; }
    }

    public class RiskStrategy
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Fee { get; set; }
    }

    public class Contribution
    {
        public int Amount { get; set; }
        public Frequency Frequency { get; set; }
        public string Description { get; set; }
    }

    public enum Frequency
    {
        Weekly,
        BiWeekly,
        Monthly
    }
}