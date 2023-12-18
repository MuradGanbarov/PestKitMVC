using PestKit.Models;
using System.ComponentModel.DataAnnotations;

namespace PestKit.Areas.PestKitAdmin.ViewModels.Employes
{
    public class CreateEmployeeVM
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string MiddleName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string LastName { get; set; }
        public int PositionId { get; set; }
        public List<Position> MyProperty { get; set; }
        public int DepartmentId { get; set; }
        public List<Department>? Departments { get; set; }
        public string Facebook { get; set; }
        public string X { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public IFormFile? ImageURL { get; set; }
    }
}
