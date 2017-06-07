using System;
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
        public double? salary { get; set; }
        public double? contributionPercentage { get; set; }
        public double? companyMatch { get; set; }
        public Frequency? contributionFrequency { get; set; }
    }

    public class UserProfileRequest
    {
        public string username { get; set; }
        public DateTime? birthday { get;set; }
        public string firstName { get;set; }
        public string lastName { get;set; }

    }

    public class AccountsRequest
    {
        public string username { get; set; }
        public string accountname { get;set; }
        public IFormFile file {get;set;}
    }
}
