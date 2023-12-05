using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKit.Areas.PestKitAdmin.Models.Enums;
using PestKit.Areas.PestKitAdmin.Models.Utilites.Extensions;
using PestKit.Areas.PestKitAdmin.ViewModels;
using PestKit.DAL;
using PestKit.Models;
using System.Net.Http.Headers;
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

        public async Task<IActionResult> Index()
        {
            List<Project> projects = await _context.Projects.Include(p => p.ProjectImages).ToListAsync();
            return View(projects);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectVM projectVM)
        {
            if (!ModelState.IsValid)
            {
                return View(projectVM);
            }

            if (!projectVM.MainPhoto.IsValidType(FileType.Image))
            {
                ModelState.AddModelError("Photo", "Image tipi uygun deyil");
                return View(projectVM);
            }

            if (!projectVM.MainPhoto.IsValidSize(5, FileSizes.FileSize.Megabite))
            {
                ModelState.AddModelError("Photo", "Image size'i 5 mb'den chox olmali deyil");
                return View(projectVM);
            }


            string fileName = await projectVM.MainPhoto.CreateAsync(_env.WebRootPath, "img");

            Project project = new Project
            {
                Name = projectVM.Name,
                ProjectImages = new() {
                new(){URL = fileName,IsPrimary=true},
                }
            };

            foreach (IFormFile photo in projectVM.SecondaryPhoto)
            {
                if (!photo.IsValidType(FileType.Image))
                {
                    TempData["Message"] += $"\n{photo.FileName} file tipi uygun deyil";

                }
                if (!photo.IsValidSize(5, FileSizes.FileSize.Megabite))
                {
                    TempData["Message"] += $"<p class=\"text-danger\">{photo.FileName} File olchusu uygun deyil: 5mb olmalidi</p>";
                }

                project.ProjectImages.Add(new ProjectImages
                {
                    Alternative = projectVM.Name,
                    IsPrimary = null,
                    URL = await photo.CreateAsync(_env.WebRootPath, "img")
                });

            }
            await _context.AddAsync(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Project project = await _context.Projects.Include(p => p.ProjectImages).FirstOrDefaultAsync(pi => pi.Id == id);
            if (project is null) return NotFound();

            foreach (ProjectImages photo in project.ProjectImages)
            {
                photo.URL.Delete(_env.WebRootPath, "img");
            }

            _context.Projects.Remove(project);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0) return BadRequest();
            Project project = await _context.Projects.Include(p => p.ProjectImages).FirstOrDefaultAsync(p => p.Id == id);
            if (project is null) return NotFound();
            return View(project);
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Project project = await _context.Projects.Include(p => p.ProjectImages).FirstOrDefaultAsync(pi => pi.Id == id);
            if (project is null) return NotFound();
            UpdateProjectVM updateVM = new UpdateProjectVM
            {
                Name = project.Name,
                ProjectImages=project.ProjectImages
            };
            return View(updateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProjectVM updateVM)
        {
            Project existed = await _context.Projects.Include(p => p.ProjectImages).FirstOrDefaultAsync(pi => pi.Id == id);
            updateVM.ProjectImages = existed.ProjectImages;

            if (!ModelState.IsValid)
            {
                updateVM.ProjectImages = await _context.ProjectImages.ToListAsync();
                ModelState.AddModelError("Image", "Bele bir Image yoxdur");
                return View(updateVM);
            }

            if (existed is null) return NotFound();

            if (updateVM.MainPhoto is not null)
            {
                if (!updateVM.MainPhoto.IsValidType(FileType.Image))
                {
                    ModelState.AddModelError("Image", "Bele bir type uygun deyil");
                    return View(updateVM);
                }

                if (!updateVM.MainPhoto.IsValidSize(5, FileSizes.FileSize.Megabite))
                {
                    ModelState.AddModelError("Image", "photo olchusu choxdu:5mb olmalidir");
                    return View(updateVM);
                }
            }



            if (updateVM.MainPhoto is not null)
            {
                string filename = await updateVM.MainPhoto.CreateAsync(_env.WebRootPath, "img");
                ProjectImages mainImage = existed.ProjectImages.FirstOrDefault(pi => pi.IsPrimary == true);
                mainImage.URL.Delete(_env.WebRootPath, "img");
                _context.ProjectImages.Remove(mainImage);
                existed.ProjectImages.Add(new ProjectImages
                {
                    IsPrimary = true,
                    URL = filename,
                });
            }

            if (updateVM.ImageIds is null)
            {
                updateVM.ImageIds = new List<int>();
            }

            Project project = new Project
            {
                Name = updateVM.Name,
                ProjectImages = new List<ProjectImages>(),
            };

            List<ProjectImages> removeable = existed.ProjectImages.Where(pi => !updateVM.ImageIds.Exists(imgId => imgId == pi.Id) && pi.IsPrimary == null).ToList();
            if (updateVM.SecondaryPhoto is not null) return NotFound();
            foreach (ProjectImages image in removeable)
            {
                image.URL.Delete(_env.WebRootPath, "img");
                existed.ProjectImages.Remove(image);
            }
            TempData["Message"] = "";
            if (updateVM.SecondaryPhoto is not null)
            {
                foreach (IFormFile photo in updateVM.SecondaryPhoto ?? new List<IFormFile>())
                {
                    if (!photo.IsValidType(FileType.Image))
                    {
                        TempData["Message"] += $"\n{photo.FileName} file tipi uygun deyil";
                        continue;
                    }
                    if (!photo.IsValidSize(5, FileSizes.FileSize.Megabite))
                    {
                        TempData["Message"] += $"<p class=\"text-danger\">{photo.FileName} File olchusu uygun deyil: 5mb</p>";
                        continue;
                    }

                    existed.ProjectImages.Add(new ProjectImages
                    {
                        Alternative = updateVM.Alternative,
                        IsPrimary = null,
                        URL = await photo.CreateAsync(_env.WebRootPath, "img"),

                    });

                }

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }




    }
    
}