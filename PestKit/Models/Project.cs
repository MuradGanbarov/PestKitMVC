using System.ComponentModel.DataAnnotations;

namespace PestKit.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public List<ProjectImages> ProjectImages { get; set; }

    }
}
