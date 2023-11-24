using System.ComponentModel.DataAnnotations;

namespace PestKit.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ad mutleq daxil edilmelidir")]
        [MaxLength(25, ErrorMessage = "Uzunluqu 25 xarakterden chox olmamalidir")]
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
