using System.ComponentModel.DataAnnotations;

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
}
