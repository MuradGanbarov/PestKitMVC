using Microsoft.AspNetCore.Mvc;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    public class DepartmentController : Controller
    {
        [Area("PestKitAdmin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
