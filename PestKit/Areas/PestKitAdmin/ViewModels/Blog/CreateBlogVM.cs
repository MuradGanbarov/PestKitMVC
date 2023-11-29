using PestKit.Models;

namespace PestKit.Areas.PestKitAdmin.ViewModels;

public class CreateBlogVM
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreateTime { get; set; }
    public int ReplyCount { get; set; }
    //public string ImageURL { get; set; }
    public List<int> TagIds { get; set; }
    public int AuthorId { get; set; }
    public List<Author> Authors { get; set; }
    public List<Tag>? Tags { get; set; }
}
