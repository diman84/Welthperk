using System.Collections.Generic;
using System.Threading.Tasks;
using Wealthperk.Model.Settings;

namespace  Wealthperk.Model
{
    public interface IUserSettingsRepository
    {
        Task<PortfolioStrategy> GetUserSettingsAsync(object userId);
        Task SetSettingsToUserAsync(object userId, PortfolioStrategy accounts);
        Task<PortfolioStrategy> GetUserSettingsByUserNameAsync(string email);
    }
}