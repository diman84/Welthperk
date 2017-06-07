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

namespace WelthPeck.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserAccountsRepository _accountRepo;
        private IUserAccountTimeseriesRepository _timeseriesRepo;
        private UserManager<UserInfo> _userManager;
        private IUserSettingsRepository _settingsRepo;
        private IRiskStrategyConfiguration _risksConfig;

        public AccountController(IUserAccountsRepository accountRepo, UserManager<UserInfo> userManager,
        IUserAccountTimeseriesRepository timeseriesRepo, IUserSettingsRepository settingsRepo, IRiskStrategyConfiguration risksConfig)
        {
            _accountRepo = accountRepo;
            _userManager = userManager;
            _timeseriesRepo = timeseriesRepo;
            _settingsRepo = settingsRepo;
            _risksConfig = risksConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Settings(UserSettingsRequest req)
        {
             var user = await _userManager.FindByEmailAsync(req.username);
            if (user == null)
            {
                return Redirect("/api/Home/Index?error=User not found");
            }

            await _settingsRepo.SetSettingsToUserAsync(user.Id, MapSettingsRequestToProtfolioSettings(req));

            return Redirect("/api/Home/Index?success");
        }

        [HttpPost]
        public async Task<IActionResult> Accounts(AccountsRequest req)
        {
            var user = await _userManager.FindByEmailAsync(req.username);
            if (user == null)
            {
                return Redirect("/api/Home/Index?error=User not found");
            }

            var accounts = await _accountRepo.GetUserAccountsAsync(user.Id);
            var account = accounts.FirstOrDefault(x=>x.DisplayName.Equals(req.accountname, StringComparison.CurrentCultureIgnoreCase));
            if (account == null){
                account = new AccountInfo {
                    AccountId = IdGenerator.GenerateIdForAccount(user.Id),
                    DisplayName = req.accountname
                };
                await _accountRepo.AddAccountsToUserAsync(user.Id, new []{account});
            }

            if (req.file != null && req.file.Length > 0){
                try {
                    await _timeseriesRepo.UploadSeriesToAccountAsync(account.AccountId, await ParseFileContent(req.file));
                }
                catch(Exception ex) {
                    return Redirect("/api/Home/Index?error=Account is created, but series won't upload|" + ex.Message);
                }
            }

            return Redirect("/api/Home/Index?success");
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
            await Task.Yield();

            return Json(new Forecast{
                byAmount = "$1,019,101",
                byAge = "65",
                currentAge = "52",
                currentAmount = "$51,823.33",
                forecast = new [] {
                    new ChartPointReal { x = 1, label = "Today", y = 180, z = 180 },
                    new ChartPointReal { x = 2, label = "", y = 240, z = 240 },
                    new ChartPointReal { x = 3, label = "48 years old", y = 360, z = 360 },
                    new ChartPoint { x = 4, label = "65 years old", z = 1000 }
                    }
                });
        }

        private PortfolioStrategy MapSettingsRequestToProtfolioSettings(UserSettingsRequest req)
        {
            var settings = new PortfolioStrategy();
            settings.RiskStrategy = req.riskstrategy;
            if (req.salary.HasValue && req.contributionFrequency.HasValue) {
                settings.ContributionStrategy = new Contribution {
                    Salary = req.salary,
                    SalaryPercent = req.contributionPercentage,
                    CompanyMatch = req.companyMatch,
                    ContributionFrequency = req.contributionFrequency,

                };
            }
            return settings;
        }

        private async Task<IEnumerable<AccountTimeseriesValue>> ParseFileContent(IFormFile file)
        {
            var rv = new List<AccountTimeseriesValue>();
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                using(var reader = new StreamReader(memoryStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(new [] {";"}, StringSplitOptions.RemoveEmptyEntries);
                        if(values.Length > 1
                            &&  DateTime.TryParse(values[0], out DateTime date)
                            &&  double.TryParse(values[1], out double mv))
                        {
                            var timeseriesData = new AccountTimeseriesValue {
                                Date = date,
                                MarketValue = mv
                            };
                            if (values.Length > 2 && double.TryParse(values[2], out double cf)) {
                                timeseriesData.CashFlow = cf;
                            }
                            rv.Add(timeseriesData);
                        }
                    }
                }

            }
            return rv;
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