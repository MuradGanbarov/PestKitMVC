﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PestKit.Areas.PestKitAdmin.Models.Utilites.Extensions;
using PestKit.Areas.PestKitAdmin.ViewModels;
using PestKit.DAL;
using PestKit.Models;
using PestKit.Utilites.Enums;
using System.ComponentModel;

namespace PestKit.Areas.PestKitAdmin.Controllers
{
    [Area("PestKitAdmin")]
    [AuthorizeRolesAttribute(UserRoles.Admin,UserRoles.Moderator)]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page)
        {
            List<Author> authors = await _context.Authors.Skip(page*3).Take(3).ToListAsync();

            double count = await _context.Authors.CountAsync();

            PaginationVM<Author> paginationVM = new PaginationVM<Author>()
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count / 3),
                Items = authors
            };



            return View(paginationVM);
        }


        [AuthorizeRoles(UserRoles.Admin,UserRoles.Moderator)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorVM authorVM)
        {
            Author author = new Author
            {
                Name = authorVM.Name,
                Surname = authorVM.Surname,
            };

            TempData["Message"] = "";
            if (!ModelState.IsValid) return View();

            bool IsExist = await _context.Authors.AnyAsync(a => a.Name.Trim().ToLower() == authorVM.Name.Trim().ToLower());
            if (IsExist)
            {
                ModelState.AddModelError("Author", "Bele bir author hal hazirda var");
                TempData["Message"] += $"<p class=\"text-danger\">\n{author.Name},{author.Surname}: Bele bir author hal hazirda var</p>";
                return View();
            }



            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [AuthorizeRoles(UserRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return NotFound();
            Author existed = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if( existed is null) return NotFound();
            _context.Authors.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AuthorizeRoles(UserRoles.Admin, UserRoles.Moderator)]
        public async Task<IActionResult> Update(int id)
        {
            if(id <= 0) return NotFound();
            Author author = await _context.Authors.FirstOrDefaultAsync(a=>a.Id == id);
            UpdateAuthorVM updateVM = new UpdateAuthorVM()
            {
                Name = author.Name,
            };

            if (author is null) return BadRequest();

            return View(updateVM);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateAuthorVM updateVM)
        {
            if (ModelState.IsValid)
            {
                return View();
            }

            Author exist = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
    
            if(exist is null) return NotFound();
            if(exist.Name == updateVM.Name)
            {
                return RedirectToAction(nameof(Index));
            }
            

            exist.Name = updateVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 


        }


        public async Task<List<Author>> GetAuthorAsync()
        {
            List<Author> authors = await _context.Authors.ToListAsync();
            return authors;
        }

    }
}
