using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKit.Areas.PestKitAdmin.Models.Utilites.Extensions;
using PestKit.Areas.PestKitAdmin.ViewModels;
using PestKit.DAL;
using PestKit.Models;
using PestKit.Utilites.Enums;
using System.Threading.Tasks.Dataflow;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    [Area("PestKitAdmin")]
    [AuthorizeRoles(UserRoles.Admin, UserRoles.Moderator)]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Blog> blogs = _context.Blogs.Include(b => b.BlogTags).ThenInclude(bt => bt.Tag).ToList();
            return View(blogs);
        }
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Moderator)]
        public async Task<IActionResult> Create()
        {
            CreateBlogVM blogVM = new CreateBlogVM
            {
                Authors = await GetAuthorsAsync(),
                Tags = await GetTagsAsync(),
            };
            return View(blogVM);
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateBlogVM blogVM)
        {
            if (!ModelState.IsValid)
            {
                blogVM.Authors = await GetAuthorsAsync();
                blogVM.Tags = await GetTagsAsync();
                return View();
            }

            bool isExist = await _context.Blogs.AnyAsync(x => x.Title.Trim().ToLower() == blogVM.Title.Trim().ToLower());
            if (!isExist)
            {
                blogVM.Authors = await GetAuthorsAsync();
                ModelState.AddModelError("Author", "Bele bir author yoxdur");
                blogVM.Tags = await GetTagsAsync();
                ModelState.AddModelError("Tag", "Bele bir tag yoxdur");
            }

            foreach (int tagId in blogVM.TagIds)
            {
                bool TagResult = await _context.Tags.AnyAsync(t => t.Id == tagId);
                if (!TagResult)
                {
                    blogVM.Authors = await GetAuthorsAsync();
                    blogVM.Tags = await GetTagsAsync();
                    ModelState.AddModelError("Tag", "Bele bir tag movcud deir");
                    return View();
                }
            }
            

            Blog blog = new Blog
            {
                Title = blogVM.Title,
                Description = blogVM.Description,
                CreateTime = blogVM.CreateTime,
                ReplyCount = blogVM.ReplyCount,
                BlogTags = new List<BlogTag>()
            };

            foreach (int tagId in blogVM.TagIds)
            {
                BlogTag blogTag = new BlogTag();
                blogTag.TagId = blogVM.TagIds[tagId];
                blog.BlogTags.Add(blogTag);
            }

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }







        public async Task<List<Author>> GetAuthorsAsync()
        {
            List<Author> authors = await _context.Authors.ToListAsync();
            return authors;
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            List<Tag> tags = await _context.Tags.ToListAsync();
            return tags;
        }





    }
}
