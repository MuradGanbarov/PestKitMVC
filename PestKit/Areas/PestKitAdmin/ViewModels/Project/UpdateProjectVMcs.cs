using PestKit.Models;
using System.ComponentModel.DataAnnotations;

namespace PestKit.Areas.PestKitAdmin.ViewModels
{
    public class UpdateProjectVM
    {
        public string Name { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public string? Alternative { get; set; }
        public List<IFormFile>? SecondaryPhoto { get; set; }
        public List<int>? ImageIds { get; set; }
        public List<ProjectImages>? ProjectImages { get; set; }

    }
}
