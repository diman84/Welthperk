namespace Wealthperk.ViewModel
{
    public class AppUser
    {
        public string email;
        public AccountInfo[] accounts;

        public class AccountInfo
        {
            public string id;
            public string name;
        }
    }
}