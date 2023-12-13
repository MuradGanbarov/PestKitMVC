using System.ComponentModel.DataAnnotations;

namespace PestKit.Areas.PestKitAdmin.ViewModels
{
    public class CreateProductVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public decimal Price { get; set; }
        public IFormFile? Img { get; set; }
    }
}
