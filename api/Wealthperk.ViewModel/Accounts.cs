using System;

namespace Wealthperk.ViewModel
{
    public class AccountValue
    {
      public string retirementSavings;
      public string returns;
      public string totalEarnings;
      public string feeSavings;
      public string freeTrades;
      public string dividents;
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
