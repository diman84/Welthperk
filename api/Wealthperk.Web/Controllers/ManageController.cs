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
    public class ManageController : Controller
    {
        private IUserAccountsRepository _accountRepo;
        private IUserAccountTimeseriesRepository _timeseriesRepo;
        private UserManager<UserInfo> _userManager;
        private IUserSettingsRepository _settingsRepo;
        private IUserProfileRepository _profileRepo;
        private IRiskStrategyConfiguration _risksConfig;
        private ICalculationService _calculation;
        private ICalculationConfiguration _calcConfig;

        public ManageController(
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

        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileRequest req)
        {
             var user = await _userManager.FindByEmailAsync(req.username);
            if (user == null)
            {
                return Redirect("/api/Home/Index?error=User not found");
            }

            await _profileRepo.SetProfileToUserAsync(user.Id, MapProfileRequestToUserProfile(req));

            return Redirect("/api/Home/Index?success");
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

        private UserProfile MapProfileRequestToUserProfile(UserProfileRequest req)
        {
            var porfile = new UserProfile();
            porfile.Birthday = req.birthday;
            porfile.FirstName = req.firstName;
            porfile.LastName = req.lastName;
            return porfile;
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
    }
}