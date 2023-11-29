using Microsoft.AspNetCore.Mvc;
using PestKit.Areas.PestKitAdmin.Models.Enums;
using PestKit.Areas.PestKitAdmin.Models.Utilites.Extensions;
using PestKit.Areas.PestKitAdmin.ViewModels;
using PestKit.DAL;
using PestKit.Models;
using static PestKit.Areas.PestKitAdmin.Models.Enums.FileTypes;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    [Area("PestKitAdmin")]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProjectController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

       public IActionResult Index()
       {
            /*List<Project> projects = await _context.Projects.ToListAsync()*/;
            return View();
       }

        //public IActionResult Create()
        //{   
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Create(CreateProjectVM projectVM)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    if (!projectVM.MainPhoto.IsValidType(FileType.Image))
        //    {
        //        ModelState.AddModelError("Photo", "Image tipi uygun deyil");
        //        return View(projectVM);
        //    }

        //    if (!projectVM.MainPhoto.IsValidSize(5, FileSizes.FileSize.Megabite))
        //    {
        //        ModelState.AddModelError("Photo", "Image size'i 5 mb'den chox olmali deyil");
        //        return View(projectVM);
        //    }

        //    if (!projectVM.SecondaryPhoto.IsValidType(FileType.Image))
        //    {
        //        ModelState.AddModelError("Photo", "Image tipi uygun deyil");
        //        return View(projectVM);
        //    } 

        //    if(!projectVM.SecondaryPhoto.IsValidSize(5, FileSizes.FileSize.Megabite))
        //    {
        //        ModelState.AddModelError("Photo", "Image size'i 5 mb'den chox olmali deyil");
        //        return View(projectVM);
        //    }

        //    string fileName = await projectVM.MainPhoto.CreateAsync(_env.WebRootPath, "admin","img");

            

        //    Project project = new Project
        //    {
        //        Name = projectVM.Name,
        //        MainPhoto = projectVM.MainPhoto,
        //        SecondaryPhoto = projectVM.SecondaryPhoto,
        //    };

            
           
        //    await _context.AddAsync(project);

        //}
        


    }
}
