using System.ComponentModel.DataAnnotations;

namespace PestKit.Areas.PestKitAdmin.ViewModels
{
    public class CreateProjectVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile MainPhoto { get; set; }
        public string? Alternative { get; set; }
        [Required]
        public List<IFormFile>? SecondaryPhoto { get; set; }
    }
}
