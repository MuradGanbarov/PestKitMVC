using Microsoft.AspNetCore.Mvc;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
