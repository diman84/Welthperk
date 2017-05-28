using System;

namespace Wealthperk.Model.Accounts
{
    public class AccountTimeseriesValue
    {
        public DateTime Date { get; set; }
        public string AccountId { get; set; }

        public double? MarketValue { get; set; }

        public double? CashFlow { get; set; }
    }
}