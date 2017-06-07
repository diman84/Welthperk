using System.Collections.Generic;
using System.Threading.Tasks;
using Wealthperk.Model.Profile;

namespace  Wealthperk.Model
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetUserProfileAsync(object userId);
        Task SetProfileToUserAsync(object userId, UserProfile accounts);
        Task<UserProfile> GetUserProfileByUserNameAsync(string email);
    }
}