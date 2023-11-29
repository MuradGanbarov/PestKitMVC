using Microsoft.AspNetCore.Mvc;

namespace PestKit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
