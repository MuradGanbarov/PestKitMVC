namespace PestKit.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public int ReplyCount { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public string ImageURL { get; set; }
        public List<BlogTag> BlogTags { get; set; } 
    }
}
