using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wealthperk.Web.ViewModels;
using Wealthperk.ViewModel;

namespace WelthPeck.Controllers
{
    public class AccountController : Controller
    {

        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Values()
        {
            await Task.Yield();
            return Json(new AccountValue() {
                    retirementSavings = "$105,912.12",
                    returns = "+ $5,912.12",
                    totalEarnings = "+14.1%",
                    feeSavings = "$509",
                    freeTrades = "629",
                    dividents = "$643",
                });

        }

        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            await Task.Yield();
            return Json(new AccountSettings() {
                    contribution = new ContributionSettings {
                        contribution = "$250.10",
                        frequency = "Biweakly",
                        description = "<p>3% of your pay</p><p>1% employer match</p>"
                    },
                    riskProfile = new RiskSettings {
                        profileName = "Aggressive Growth",
                        description = "<p>100% Growth Assets (Stocks)</p><p>0% Defensive Assets (Bonds)</p>",
                        fee = "0.20%"
                    }
                });

        }
    }
}