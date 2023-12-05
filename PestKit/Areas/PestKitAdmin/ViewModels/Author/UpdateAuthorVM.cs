using System.ComponentModel.DataAnnotations;

namespace PestKit.Areas.PestKitAdmin.ViewModels
{
    public class UpdateAuthorVM
    {
        [Required(ErrorMessage = "Ad mutleq daxil edilmelidir")]
        [MaxLength(25, ErrorMessage = "Uzunluqu 25 xarakterden chox olmamalidir")]
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
