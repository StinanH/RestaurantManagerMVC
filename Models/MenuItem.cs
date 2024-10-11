using System.ComponentModel.DataAnnotations;

namespace RestaurantManagerMVC.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name of item is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required (ex. Pasta, Pizza, Drink..)")]
        public string Category { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public int Price { get; set; }
        public int AmountSold { get; set; }
    }
}
