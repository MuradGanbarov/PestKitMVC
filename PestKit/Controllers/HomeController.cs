using Microsoft.AspNetCore.Mvc;

namespace PestKit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ErrorPage(string error)
        {
            if (error is null) return RedirectToAction(nameof(Index));
            return View(model: error);
        }


    }
}
