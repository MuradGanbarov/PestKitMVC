using Microsoft.AspNetCore.Mvc;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public Task<IActionResult> Create()
        {
            if (!ModelState.IsValid)
            {
                
            }
        }


    }
}
