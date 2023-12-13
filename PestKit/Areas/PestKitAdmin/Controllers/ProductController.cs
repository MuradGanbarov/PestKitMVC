using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKit.DAL;
using static PestKit.Areas.PestKitAdmin.Models.Enums.FileTypes;
using static PestKit.Areas.PestKitAdmin.Models.Enums.FileSizes;
using PestKit.Areas.PestKitAdmin.Models.Enums;
using PestKit.Models;
using PestKit.Areas.PestKitAdmin.Models.Utilites.Extensions;
using PestKit.Areas.PestKitAdmin.ViewModels;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    [Area("PestKitAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        
        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if (!ModelState.IsValid) return View(productVM);

            if (!productVM.Img.IsValidType(productVM.Img,FileTypes.FileType.Image))
            {
                ModelState.AddModelError("Photo", "This type of file can not upload");
                return View(productVM);
            }

            if (!productVM.Img.IsValidSize(5, FileSize.Megabite))
            {
                ModelState.AddModelError("Photo", "This size of image can not upload");
                return View(productVM);
            }

            Product product = new()
            {
                Name = productVM.Name,
                Description = productVM.Description,
                Price = productVM.Price,
                Img = await productVM.Img.CreateAsync(_env.WebRootPath,"img"),
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return NotFound();
            
            Product product = await _context.Products.SingleOrDefaultAsync(p=>p.Id == id);

            if(product is null) return NotFound();

            product.Img.Delete(_env.WebRootPath, "img");

             _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        //public IActionResult Update(int id)
        //{
        


        //}



    }
}
