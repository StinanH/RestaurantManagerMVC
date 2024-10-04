using System.ComponentModel;

namespace RestaurantManagerMVC.Models
{
    public class Restaurant
    {
        public int? Id { get; set; }

        [DisplayName("Restaurant")]
        public string? Name { get; set; }

        [DisplayName("Phonenumber")]
        public string? PhoneNumber { get; set; }
        [DisplayName("Email")]
        public string? Email { get; set; }
        [DisplayName("Address")]
        public string? Address { get; set; }
        [DisplayName("About") ]
        public string? Description { get; set; }
    }
}
