using System.Linq;
using System.Security.Claims;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Http;

namespace Wealthperk.Web.Auth
{
    public class HttpUserIdentity
    {
        public static string CurrentUserName(ClaimsPrincipal user)
        {
            if (user == null || user.Claims == null)
                return null;

            var claim = user.Claims.FirstOrDefault(c=>c.Type == OpenIdConnectConstants.Claims.Name);

            return claim?.Value;
        }

    }
}