namespace PestKit.Models
{
    public class ProjectImages
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public  string? Alternative { get; set; }
        public bool? IsPrimary { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

    }
}
