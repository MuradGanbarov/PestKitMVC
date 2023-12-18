using System.ComponentModel.DataAnnotations;

namespace PestKit.Areas.PestKitAdmin.ViewModels
{
    public class CreateUpdatePositionVM
    {
        [Required(ErrorMessage = "Title should have name")]
        [MaxLength(25,ErrorMessage = "Title name length should be maximum 25 letters")]
        public string Name { get; set; }
    }
}
