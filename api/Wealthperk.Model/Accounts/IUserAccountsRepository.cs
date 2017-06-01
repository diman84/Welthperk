using System.Collections.Generic;
using System.Threading.Tasks;
using Wealthperk.Model.Accounts;

namespace  Wealthperk.Model
{
    public interface IUserAccountsRepository
    {
        Task<IEnumerable<AccountInfo>> GetUserAccountsAsync(object userId);
        Task AddAccountsToUserAsync(object userId, IEnumerable<AccountInfo> accounts);
        Task<IEnumerable<AccountInfo>> GetUserAccountsByUserNameAsync(string email);
    }

    public interface IUserAccountTimeseriesRepository
    {
        Task UploadSeriesToAccountAsync(string accountId, IEnumerable<AccountTimeseriesValue> values);
        Task<double?> GetLatestMarketValueForAccountAsync(string accountId);
        Task<double?> GetStartMarketValueForAccountAsync(string accountId);
        Task<double?> GetTotalCashFlowForAccountAsync(string accountId);
    }
}