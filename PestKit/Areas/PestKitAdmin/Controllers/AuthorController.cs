using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKit.Areas.PestKitAdmin.ViewModels;
using PestKit.DAL;
using PestKit.Models;
using System.ComponentModel;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    [Area("PestKitAdmin")]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Author> authors = await _context.Authors.ToListAsync();

            return View(authors);
        }

        

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorVM authorVM)
        {
            Author author = new Author
            {
                Name = authorVM.Name,
                Surname = authorVM.Surname,
            };

            TempData["Message"] = "";
            if (!ModelState.IsValid) return View();

            bool IsExist = await _context.Authors.AnyAsync(a=>a.Name.Trim().ToLower() == authorVM.Name.Trim().ToLower());
            if(IsExist)
            {
                ModelState.AddModelError("Author", "Bele bir author hal hazirda var");
                TempData["Message"] += $"<p class=\"text-danger\">\n{author.Name},{author.Surname}: Bele bir author hal hazirda var</p>";
                return View();
            }
            


            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<List<Author>> GetAuthorAsync()
        {
            List<Author> authors = await _context.Authors.ToListAsync();
            return authors;
        }

    }
}
