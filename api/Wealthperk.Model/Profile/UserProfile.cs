using System;

namespace Wealthperk.Model.Profile
{
    public class UserProfile {
        public DateTime? Birthday {get;set;}

        public static UserProfile Default()
        {
            return new UserProfile ();
        }

        public int? Age()
        {
            if (Birthday.HasValue)
                return DateTime.Now.Year - Birthday.Value.Year;

            return null;
        }
    }
}