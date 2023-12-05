using System.ComponentModel.DataAnnotations;

namespace PestKit.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<ProjectImages>? ProjectImages { get; set; }

        public static implicit operator Project?(Author? v)
        {
            throw new NotImplementedException();
        }
    }
}
