using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerMVC.Models
{
    public class LoginViewModel
    {
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }
    }
}
