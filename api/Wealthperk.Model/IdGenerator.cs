using System;
using HashidsNet;

namespace Wealthperl.Model
{
    public class IdGenerator
    {
        public static string GenerateIdForAccount(object userId)
        {
            var hash = new Hashids(userId.ToString());
            return userId + "_" + hash.Encode(DateTime.Now.Millisecond);
        }
    }
}