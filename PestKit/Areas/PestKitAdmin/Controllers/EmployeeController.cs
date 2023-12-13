using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKit.Areas.PestKitAdmin.Models.Enums;
using PestKit.Areas.PestKitAdmin.Models.Utilites.Extensions;
using PestKit.Areas.PestKitAdmin.ViewModels.Employes;
using PestKit.DAL;
using PestKit.Models;
using PestKit.Utilites.Enums;
using static PestKit.Areas.PestKitAdmin.Models.Enums.FileTypes;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    [Area("PetsKitAdmin")]
    [AuthorizeRoles(UserRoles.Admin, UserRoles.Moderator)]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees.Include(e=>e.Department).Include(e=>e.Position).ToListAsync();
            return View(employees);
        }


        public async Task<IActionResult> Create()
        {
            CreateEmployeeVM employeeVM = new()
            {
                Departments = await _context.Departments.ToListAsync(),
                Positions = await _context.Positions.ToListAsync()

            };
            return View(employeeVM);
        }

        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            if(employeeVM.ImageURL is null)
            {
                await _context.Employees.ToListAsync();
                await _context.Departments.ToListAsync();
                return View(employeeVM);
            }

            if (!employeeVM.ImageURL.IsValidType(employeeVM.ImageURL,FileType.Image))
            {
                ModelState.AddModelError("Photo", "File type is incorrect");
                return View();
            }

            if (!employeeVM.ImageURL.IsValidSize(5, FileSizes.FileSize.Megabite))
            {
                ModelState.AddModelError("Photo", "File size is incorrect");
                return View();
            }

            string filename = await employeeVM.ImageURL.CreateAsync(_env.WebRootPath,"img");

            Employee employee = new()
            {
                Name = employeeVM.Name,
                MiddleName = employeeVM.MiddleName,
                LastName = employeeVM.LastName,
                ImageURL = filename,
                Instagram = employeeVM.Instagram,
                LinkedIn = employeeVM.LinkedIn,
                X = employeeVM.X,
                Facebook = employeeVM.Facebook,
                DepartmentId = employeeVM.DepartmentId,
                PositionId  = employeeVM.PositionId,
            };

            await _context.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


    }
}
