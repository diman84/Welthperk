using System;

namespace Wealthperk.Model
{
    public class UserInfo
    {
        public bool EmailConfirmed;

        public string UserName { get; set; }
        public string Email { get; set; }
        public object Id { get; set; }
    }
}
