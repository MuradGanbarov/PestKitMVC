using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKit.DAL;
using PestKit.Models;

namespace PestKit.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
            
        }
        public async Task<IActionResult> Index()
        {
            List<Department> departments = await _context.Departments.ToListAsync();
            return View(departments);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Department? department = await _context.Departments.Include(d => d.Employees).ThenInclude(e => e.Position).FirstOrDefaultAsync(e => e.Id == id);
            return View(department);
        }

    }
}
