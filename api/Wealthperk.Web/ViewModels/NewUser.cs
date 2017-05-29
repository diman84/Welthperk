using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Wealthperk.Model;

namespace Wealthperk.Web.ViewModels
{
    public class NewUser
    {
        [Required]
        [EmailAddress]
        public string username { get; set; }

        [Required]
        [MinLength(8)]
        public string password { get; set; }
    }

    public class UserAccount
    {
        public string username { get; set; }
        public string accountname { get;set; }
    }

    public class UserSettingsRequest
    {
        public string username { get; set; }
        public string riskstrategy { get;set; }
        public int? contributionAmount { get; set; }
        public string contributionDescription { get; set; }
        public Frequency? contributionFrequency { get; set; }
    }

    public class AccountsRequest
    {
        public string username { get; set; }
        public string accountname { get;set; }
        public IFormFile file {get;set;}
    }
}
