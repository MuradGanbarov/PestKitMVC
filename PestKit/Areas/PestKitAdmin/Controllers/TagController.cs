using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PestKit.Areas.PestKitAdmin.ViewModels;
using PestKit.DAL;
using PestKit.Models;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    [Area("PestKitAdmin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }


        public async Task <IActionResult> Index()
        {
            List<Tag> tags = await _context.Tags.ToListAsync();
            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagVMcs TagVM)
        {
            Tag Tag = new Tag() { Name = TagVM.Name };

            TempData["Message"] = "";

            if (!ModelState.IsValid) return View();

            bool IsExist = await _context.Tags.AnyAsync(t=>t.Name.Trim() == TagVM.Name.Trim());
            
            if(IsExist)
            {
                TempData["Message"] = $"<p class=\"text-danger\">{Tag.Name} Bele bir ad hal hazirda var</p>";
                return View();
            }

            await _context.Tags.AddAsync(Tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Tag exist = await _context.Tags.FirstOrDefaultAsync(t=>t.Id == id);
            

            if (exist is null) return NotFound();

            _context.Remove(exist);

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            if(id<=0) return BadRequest();

            Tag tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            UpdateTagVM tagVM = new UpdateTagVM() {Name = tag.Name};

            return View(tagVM);

        }

        [HttpPost]


        public async Task<IActionResult> Update(int id, UpdateTagVM tagVM)
        {
            if(id <= 0) return BadRequest();

            Tag exist = await _context.Tags.FirstOrDefaultAsync(t=>t.Id == id);

            if(exist is null) return NotFound();
            if(exist.Name  == tagVM.Name)
            {
                return RedirectToAction(nameof(Index));
            }

            exist.Name = tagVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

           
        }



    }
}
