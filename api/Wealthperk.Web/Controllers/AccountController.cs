using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wealthperk.Web.ViewModels;
using Wealthperk.ViewModel;
using Wealthperk.Model;
using Wealthperk.Web.Auth;
using Microsoft.AspNetCore.Http;
using Wealthperl.Model;
using Microsoft.AspNetCore.Identity;
using Wealthperk.Model.Accounts;
using System.IO;
using Wealthperk.Web.Formatting;
using Wealthperk.ViewModel.Account;
using Microsoft.AspNetCore.Authorization;
using Wealthperk.Calculation;
using Wealthperk.Model.Profile;

namespace WelthPeck.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserAccountsRepository _accountRepo;
        private IUserAccountTimeseriesRepository _timeseriesRepo;
        private UserManager<UserInfo> _userManager;
        private IUserSettingsRepository _settingsRepo;
        private IUserProfileRepository _profileRepo;
        private IRiskStrategyConfiguration _risksConfig;
        private ICalculationService _calculation;
        private ICalculationConfiguration _calcConfig;

        public AccountController(
            IUserAccountsRepository accountRepo,
            UserManager<UserInfo> userManager,
            IUserAccountTimeseriesRepository timeseriesRepo,
            IUserSettingsRepository settingsRepo,
            IRiskStrategyConfiguration risksConfig,
            ICalculationConfiguration calcConfig,
            IUserProfileRepository profileRepo,
            ICalculationService calculation)
        {
            _accountRepo = accountRepo;
            _userManager = userManager;
            _timeseriesRepo = timeseriesRepo;
            _settingsRepo = settingsRepo;
            _risksConfig = risksConfig;
            _profileRepo = profileRepo;
            _calculation = calculation;
            _calcConfig = calcConfig;
        }

        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Values()
        {
            var userName = HttpUserIdentity.CurrentUserName(User);
            var res = await _accountRepo.GetUserAccountsByUserNameAsync(userName);

            if (!res.Any()){
                return BadRequest(new { message = "You have no accounts configured"});
            }

            return Json(await GetAccountsWithValues(res));
        }

        [HttpGet("~/account/values/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> Values(string id)
        {
            var userName = HttpUserIdentity.CurrentUserName(User);
            var res = await _accountRepo.GetUserAccountsByUserNameAsync(userName);

            if (!res.Any()){
                return BadRequest(new { message = "You have no accounts configured"});
            }

            var account = res.FirstOrDefault(x => x.AccountId == id);

            if (account == null){
                return NotFound(new { message = "Account not found"});
            }

            return Json(await GetAccountWithValues(account));
        }

        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var userName = HttpUserIdentity.CurrentUserName(User);
            var settings = await _settingsRepo.GetUserSettingsByUserNameAsync(userName);
            var contribution = settings != null && settings.ContributionStrategy != null
                    ? new ContributionSettings {
                        contribution = settings.ContributionStrategy.AmountPerFrequency().FormatCurrency(),//"$250.10",
                        frequency = settings.ContributionStrategy.ContributionFrequency?.ToString() ?? "N/A",//"Biweekly"
                        description = string.Format("<p>{0} of your pay</p><p>{1} employer match</p>",
                            settings.ContributionStrategy.SalaryPercent?.ToString("P0") ?? "N/A",
                            settings.ContributionStrategy.CompanyMatch?.ToString("P0") ?? "N/A")
                    }
                    : ContributionSettings.Undefined();

            var risks =  settings != null && settings.RiskStrategy != null && _risksConfig.RiskStrategies.ContainsKey(settings.RiskStrategy)
                    ? new RiskSettings {
                        profileName = _risksConfig.RiskStrategies[settings.RiskStrategy].Name,//"Aggressive Growth",
                        description = _risksConfig.RiskStrategies[settings.RiskStrategy].Description,//"<p>100% Growth Assets (Stocks)</p><p>0% Defensive Assets (Bonds)</p>",
                        fee = string.Format("{0:0.00}%", _risksConfig.RiskStrategies[settings.RiskStrategy].Fee) //"0.20%"
                    }
                    : RiskSettings.Undefined();

            return Json(new AccountSettings() {
                    contribution = contribution,
                    riskProfile = risks
                });
        }

        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Forecast()
        {
            var userName = HttpUserIdentity.CurrentUserName(User);
            var age = (await _profileRepo.GetUserProfileByUserNameAsync(userName) ?? UserProfile.Default()).Age();
            var accounts = (await _accountRepo.GetUserAccountsByUserNameAsync(userName));
            var currentBalance = (await Task.WhenAll(accounts.Select(acc=> _timeseriesRepo.GetLatestMarketValueForAccountAsync(acc.AccountId)))).Sum() ?? 0;
            var startBalance = (await Task.WhenAll(accounts.Select(acc=> _timeseriesRepo.GetStartMarketValueForAccountAsync(acc.AccountId)))).Sum() ?? 0;
            //var halfWay = startBalance + (currentBalance - startBalance)/2;
            var annualContribution = (await _settingsRepo.GetUserSettingsByUserNameAsync(userName))?.ContributionStrategy?.AnnualContribution() ?? 0;
            var byAmount = _calculation.PredictionForYears(
                                startBalance: currentBalance,
                                annualContribution: annualContribution,
                                annualGrowth: _calcConfig.DefaultGrowth,
                                years: age.HasValue ? (_calcConfig.YearsAtRetirement - age.Value) : _calcConfig.YearsForPrediction);

            return Json(new Forecast{
                byAmount = byAmount.FormatCurrency(),// "$1,019,101",
                byAge = age.HasValue ? $"{_calcConfig.YearsAtRetirement} years old" : $" {_calcConfig.YearsForPrediction} years",//"65"
                currentAge = age.HasValue ? age.Value.ToString() : $" the moment", //"52"
                currentAmount = currentBalance.FormatCurrency(),// "$51,823.33",
                forecast = new [] {
                        new ChartPointReal { x = 1, label = "Joined Wealthperk", y = startBalance, z = startBalance },
                        //new ChartPointReal { x = 2, label = "", y = halfWay, z = halfWay };
                        new ChartPointReal { x = 2, label = age.HasValue ? $"{age}years old" : "current age", y = currentBalance, z = currentBalance }, //x = 3
                        new ChartPoint { x = 3, label = age.HasValue ? $"{_calcConfig.YearsAtRetirement} years old" : $" after {_calcConfig.YearsForPrediction} years", z = byAmount} //x = 4

                }   ,/*new [] {
                    new ChartPointReal { x = 1, label = "Today", y = 180, z = 180 },
                    new ChartPointReal { x = 2, label = "", y = 240, z = 240 },
                    new ChartPointReal { x = 3, label = "48 years old", y = 360, z = 360 },
                    new ChartPoint { x = 4, label = "65 years old", z = 1000 }
                    }*/
                forRetirement = age.HasValue
                });
        }

        private async Task<AccountBalance> GetAccountWithValues(AccountInfo account)
        {
            var values = await Task.WhenAll(
                    _timeseriesRepo.GetLatestMarketValueForAccountAsync(account.AccountId),
                    _timeseriesRepo.GetStartMarketValueForAccountAsync(account.AccountId),
                    _timeseriesRepo.GetTotalCashFlowForAccountAsync(account.AccountId)
                );
            double? mv = values[0];
            double? earn = mv - values[1] - (values[2] ?? 0);
            return new AccountBalance {
                            id = account.AccountId,
                            name = account.DisplayName,
                            balance = mv.FormatCurrency(),
                            earnings = earn.FormatCurrencyWithNoSign(),
                            feeSavings = "N/A",
                            autodeposit = false,
                            earningsSign = Math.Sign(earn ?? 0)
                        };
        }

        private async Task<PortfolioValue> GetAccountsWithValues(IEnumerable<AccountInfo> accounts)
        {
            double? retirementSavings = null;
            double? totalEarnings = null;
            var accountBalances = new List<AccountBalance>();
            foreach (var account in accounts)
            {
                var values = await Task.WhenAll(
                    _timeseriesRepo.GetLatestMarketValueForAccountAsync(account.AccountId),
                    _timeseriesRepo.GetStartMarketValueForAccountAsync(account.AccountId),
                    _timeseriesRepo.GetTotalCashFlowForAccountAsync(account.AccountId)
                );
                double? mv = values[0];//await _timeseriesRepo.GetLatestMarketValueForAccountAsync(account.AccountId);
                double? earn = mv - values[1] - (values[2] ?? 0);
                retirementSavings = (mv.HasValue ?
                    (retirementSavings.HasValue
                        ? retirementSavings + mv.Value
                        : mv)
                    : retirementSavings);
                totalEarnings = (earn.HasValue ?
                    (totalEarnings.HasValue
                        ? totalEarnings + earn.Value
                        : earn)
                    : totalEarnings);
                accountBalances.Add(new AccountBalance {
                                id = account.AccountId,
                                name = account.DisplayName,
                                balance = mv.FormatCurrency(),
                                earnings = earn.FormatCurrencyWithNoSign(),//"$1,203.51"
                                autodeposit = false,
                                earningsSign = Math.Sign(earn ?? 0)
                            });
            }

            return new PortfolioValue() {
                        total = new TotalValue {
                            retirementSavings = retirementSavings.FormatCurrency(),
                            returns = (totalEarnings/(retirementSavings - totalEarnings)).FormatPercentageWithSign(),//"+14.1%",
                            totalEarnings = totalEarnings.FormatCurrencyWithSign(),//"+ $5,912.12",
                            feeSavings = "N/A",//"$509",
                            freeTrades = "N/A",//"629",
                            dividents = "N/A"//"$643"
                        },
                        accounts = accountBalances.ToArray()
                };
        }
    }
}