using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKit.DAL;
using PestKit.Models;
using PestKit.Utilites.Exceptions;

namespace PestKit.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Project> projects = await _context.Projects.Include(p=>p.ProjectImages).ToListAsync();
            return View(projects);
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0) throw new WrongRequestException("This project is not exist");
            Project project = await _context.Projects.Include(p => p.ProjectImages).FirstOrDefaultAsync(pi=>pi.Id==id);
            if (project is null) throw new NotFoundException("This project was not found");
            return View(project);
        }


    }
}
