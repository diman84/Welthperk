using System;

namespace Wealthperk.Model
{
    public class UserInfo
    {
        public bool EmailConfirmed;
        public string UserName { get; set; }
        public string Email { get; set; }
        public object Id { get; set; }
        public string PasswordHash { get; set; }
    }

    public class AccountInfo {
        public string AccountId { get; set; }
        public string DisplayName { get; set; }
        public string SourceId { get; set; }
    }
}
