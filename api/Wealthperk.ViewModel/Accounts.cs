using System;

namespace Wealthperk.ViewModel
{
    public class TotalValue
    {
        public string retirementSavings;
        public string returns;
        public string totalEarnings;
        public string feeSavings;
        public string freeTrades;
        public string dividents;
    }

    public class AccountValue
    {
        public TotalValue total;
        public AccountBalance[] accounts;
    }

    public class AccountBalance
    {
        public string id;
        public string name;
        public string balance;
        public string earnings;
        public bool autodeposit;
    }

    public class AccountSettings
    {
      public ContributionSettings contribution;
      public RiskSettings riskProfile;
    }

    public class ContributionSettings
    {
        public string contribution;
        public string frequency;
        public string description;
    }

    public class RiskSettings
    {
        public string profileName;
        public string description;
        public string fee;
    }
}
