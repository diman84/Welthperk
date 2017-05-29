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

namespace WelthPeck.Controllers
{
    public class AccountController : Controller
    {
        private IUserAccountsRepository _accountRepo;
        private IUserAccountTimeseriesRepository _timeseriesRepo;
        private UserManager<UserInfo> _userManager;

        public AccountController(IUserAccountsRepository accountRepo, UserManager<UserInfo> userManager,
        IUserAccountTimeseriesRepository timeseriesRepo)
        {
            _accountRepo = accountRepo;
            _userManager = userManager;
            _timeseriesRepo = timeseriesRepo;
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
                return BadRequest("You have no acocunt configured");
            }

            return Json(await GetAccountsWithValues(res));

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
                        var values = line.Split(new [] {';'}, StringSplitOptions.RemoveEmptyEntries);
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


        private async Task<AccountValue> GetAccountsWithValues(IEnumerable<AccountInfo> accounts)
        {
            double? retirementSavings = null;
            double? totalEarnings = null;
            var accountBalances = new List<AccountBalance>();
            foreach (var account in accounts)
            {
                var values = await Task.WhenAll(
                    _timeseriesRepo.GetLatestMarketValueForAccountAsync(account.AccountId),
                    _timeseriesRepo.GetStartMarketValueForAccountAsync(account.AccountId)
                    //TODO: add cash flow
                );
                double? mv = values[0];//await _timeseriesRepo.GetLatestMarketValueForAccountAsync(account.AccountId);
                double? earn = mv - values[1];
                retirementSavings += mv;
                totalEarnings += earn;
                accountBalances.Add(new AccountBalance {
                                id = account.AccountId,
                                name = account.DisplayName,
                                balance = mv.FormatCurrency(),
                                earnings = earn.FormatCurrency(),//"$1,203.51"
                                autodeposit = false
                            });
            }

            return new AccountValue() {
                        total = new TotalValue {
                            retirementSavings = retirementSavings.FormatCurrency(),
                            returns = "N/A",//"+14.1%",
                            totalEarnings = totalEarnings.FormatCurrency(),//"+ $5,912.12",
                            feeSavings = "N/A",//"$509",
                            freeTrades = "N/A",//"629",
                            dividents = "N/A"//"$643"
                        },
                        accounts = accountBalances.ToArray()
                };
        }
    }
}