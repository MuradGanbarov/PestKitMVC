using Microsoft.AspNetCore.Identity;
using PestKit.ViewModels;

namespace PestKit.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }

        public List<BasketItem>? BasketItems { get; set; }
        public List<Product>? Products { get; set; }
    }
}
