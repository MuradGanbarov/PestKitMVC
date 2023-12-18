using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKit.Areas.PestKitAdmin.Models.Utilites.Extensions;
using PestKit.Areas.PestKitAdmin.ViewModels;
using PestKit.DAL;
using PestKit.Models;
using PestKit.Utilites.Enums;
using Pronia.Utilites.Enums;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    [Area("PestKitAdmin")]
    [AuthorizeRoles(UserRoles.Admin, UserRoles.Moderator)]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page)
        {
            List<Position> positions = await _context.Positions.Skip(page*3).Take(3).Include(p => p.Employees).ToListAsync();
            double count = await _context.Positions.CountAsync();

            PaginationVM<Position> paginationVM = new()
            {
                CurrentPage = 1,
                TotalPage = Math.Ceiling(count / 3),
                Items = positions,
            };
            return View(paginationVM);
        }

        [AuthorizeRoles(UserRoles.Admin,UserRoles.Moderator )]

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateUpdatePositionVM positionVM)
        {
            if (!ModelState.IsValid)
            {
                return View(positionVM);
            }
            bool result = await _context.Positions.AnyAsync(p=>p.Name == positionVM.Name);
        
            if(!result)
            {
                ModelState.AddModelError("Name","This position is existed, please try new name");
                return View(positionVM);
            }

            Position position = new()
            {
                Name = positionVM.Name,
            };

            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();
            return View(position);
        
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Position existed = await _context.Positions.FirstOrDefaultAsync(p=>p.Id == id);

            if (existed is null) return NotFound();

            CreateUpdatePositionVM createUpdatePositionVM = new()
            {
                Name = existed.Name,
            };
            return View(createUpdatePositionVM);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int id,CreateUpdatePositionVM createUpdatePositionVM)
        {
            if (!ModelState.IsValid) return View(createUpdatePositionVM);
            Position existed = await _context.Positions.FirstOrDefaultAsync(p=>p.Id==id);
            if(existed is null) return NotFound();
            bool result = await _context.Positions.AnyAsync(p => p.Name.ToLower().Trim() == createUpdatePositionVM.Name.ToLower().Trim());
            if (!result)
            {
                ModelState.AddModelError("Name", "This position is existed, you need to create new");
                return View(createUpdatePositionVM);
            }

            existed.Name = createUpdatePositionVM.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id <= 0) return NotFound();
            Position position = await _context.Positions.FirstOrDefaultAsync(p=>p.Id==id);
            if(position is null) return NotFound();

            _context.Remove(position);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            if(id <= 0) return NotFound();
            Position position = await _context.Positions.Include(p=>p.Employees).FirstOrDefaultAsync(p=>p.Id== id);
            if(position is null) return NotFound();
            return View(position);
        }


    }






}
