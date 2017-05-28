using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

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

    public class AccountsRequest
    {
        public string username { get; set; }
        public string accountname { get;set; }
        public IFormFile file {get;set;}
    }
}
