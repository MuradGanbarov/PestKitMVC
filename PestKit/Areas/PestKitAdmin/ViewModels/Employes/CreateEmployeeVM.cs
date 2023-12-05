using PestKit.Models;

namespace PestKit.Areas.PestKitAdmin.ViewModels.Employes
{
    public class CreateEmployeeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public string Facebook { get; set; }
        public string X { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string ImageURL { get; set; }
    }
}
