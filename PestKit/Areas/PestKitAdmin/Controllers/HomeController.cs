using Microsoft.AspNetCore.Mvc;
using PestKit.Areas.PestKitAdmin.Models.Utilites.Extensions;
using PestKit.Utilites.Enums;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    [Area("PestKitAdmin")]
    [AuthorizeRoles(UserRoles.Admin, UserRoles.Moderator)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
