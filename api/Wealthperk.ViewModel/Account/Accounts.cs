﻿using System;

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

    public class PortfolioValue
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
        public string feeSavings;
        public bool autodeposit;
        public int earningsSign;
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

        public static ContributionSettings Undefined() {
            return new ContributionSettings() {
                contribution = "N/A",
                description = "You have no contributions defined"
            };
        }
    }

    public class RiskSettings
    {
        public string profileName;
        public string description;
        public string fee;

         public static RiskSettings Undefined() {
            return new RiskSettings() {
                profileName = "N/A",
                description = "You have no risks defined"
            };
        }
    }
}
