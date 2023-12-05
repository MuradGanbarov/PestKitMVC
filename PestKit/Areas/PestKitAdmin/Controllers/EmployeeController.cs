using Microsoft.AspNetCore.Mvc;
using PestKit.DAL;
using PestKit.Models;

namespace PestKit.Areas.PestKitAdmin.Controllers
{

    [Area("PetsKitAdmin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        //public async IActionResult Index()
        //{
        //    return View();
        //}
    }
}
