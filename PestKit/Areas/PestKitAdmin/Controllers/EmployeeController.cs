using Microsoft.AspNetCore.Mvc;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
S