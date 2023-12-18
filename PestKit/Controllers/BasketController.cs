using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PestKit.DAL;
using PestKit.Interfaces;
using PestKit.Models;
using PestKit.Services;
using PestKit.ViewModels;
using Pronia.Utilites.Enums;
using System.Collections.Generic;
using System.Security.Claims;

namespace PestKit.Controllers
{
    public class BasketController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly LayoutService _service;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        public BasketController(AppDbContext context, UserManager<AppUser> userManager, LayoutService service, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _service = service;
            _emailService = emailService;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> basketVM = new();
            if (User.Identity.IsAuthenticated)
            {
                AppUser? user = await _userManager.Users.Include(u => u.BasketItems.Where(bi=>bi.OrderId==null)).ThenInclude(bi=>bi.Product).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (user is null) return NotFound();
                foreach (var item in user.BasketItems)
                {
                    basketVM.Add(new BasketItemVM
                    {
                        Id = item.ProductId,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        Count = item.Count,
                        SubTotal = (item.Count * item.Product.Price),
                        Img = item.Product.Img,
                    });
                }
                return View(basketVM);
            }

            else
            {
                if (Request.Cookies["Basket"] is not null)
                {
                    List<BasketCookiesItemVM> basket = JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(Request.Cookies["Basket"]);
                    foreach (BasketCookiesItemVM basketitem in basket)
                    {
                        Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketitem.Id);
                        if (product is not null)
                        {
                            BasketItemVM basketItemVM = new()
                            {
                                Id = product.Id,
                                Name = product.Name,
                                Price = product.Price,
                                SubTotal = product.Price * basketitem.Quantity,
                                Count = basketitem.Quantity,

                            };
                            basketVM.Add(basketItemVM);
                        }



                    }


                }
            }

            return View(basketVM);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id <= 0) return BadRequest();
                AppUser? user = await _userManager.Users.Include(u => u.BasketItems).ThenInclude(bi => bi.Product).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (user is null) return NotFound();
                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.ProductId == id);
                if (item is null) return NotFound();
                _context.BasketItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            else
            {
                if (Request.Cookies["Basket"] != null)
                {

                    List<BasketCookiesItemVM> basket = JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(Request.Cookies["Basket"]);
                    BasketCookiesItemVM item = basket.FirstOrDefault(b => b.Id == id);
                    basket.Remove(item);
                    string json = JsonConvert.SerializeObject(basket);
                    Response.Cookies.Append("Basket", json);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddBasket(int id)
        {
            if (id <= 0) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return NotFound();
            List<BasketCookiesItemVM> basket = new();

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.Users.Include(u => u.BasketItems.Where(bi => bi.OrderId == null)).FirstOrDefaultAsync(u => u.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (user is null) return NotFound();
                BasketItem item = user.BasketItems.FirstOrDefault(b => b.ProductId == id && b.AppUserId==User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (item is null)
                {
                    item = new BasketItem
                    {
                        AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        ProductId = product.Id,
                        Count = 1,
                        
                    };

                    await _context.BasketItems.AddAsync(item);
                }
                else
                {
                    item.Count++;
                    _context.BasketItems.Update(item);

                }
                await _context.SaveChangesAsync();
               
            }

            else
            {
                if (Request.Cookies["Basket"] is not null)
                {
                    basket = JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(Request.Cookies["Basket"]);

                    BasketCookiesItemVM item = basket.FirstOrDefault(b => b.Id == id);

                    if (item is null)
                    {
                        BasketCookiesItemVM basketCookiesItemVM = new BasketCookiesItemVM
                        {
                            Id = id,
                            Quantity = 1
                        };

                        basket.Add(basketCookiesItemVM);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        item.Quantity++;
                    }

                }

                else
                {
                    basket = new()
                    {
                        new() {Id=id, Quantity=1}
                    };

                }

                string json = JsonConvert.SerializeObject(basket);
                Response.Cookies.Append("Basket", json);


            }
            List<BasketItemVM> basketItems = await _service.GetBasketItemsAsync(basket);
            return PartialView("BasketItemPartialView", basketItems);
        }

        public async Task<IActionResult> Checkout()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser? user = await _userManager.Users.Include(u => u.BasketItems.Where(bi => bi.OrderId == null)).ThenInclude(bi => bi.Product).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
                OrderVM orderVM = new()
                {
                    BasketItems = user.BasketItems,
                };
                return View(orderVM);

            }

            else
            {
                TempData["Message"] = $"<div class=\"alert alert-danger\" role=\"alert\">\r\n You need to login for checkout!\r\n</div>";
                return RedirectToAction(nameof(Index), "Home");
            }

        }
        [HttpPost]
        public async Task<IActionResult> Checkout(OrderVM orderVM)
        {

            AppUser? user = await _userManager.Users.Include(u => u.BasketItems.Where(bi => bi.Order == null)).ThenInclude(bi => bi.Product).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!ModelState.IsValid)
            {
                orderVM.BasketItems = user.BasketItems;
                return View(orderVM);
            }
            
            decimal total = 0;
            foreach (BasketItem item in user.BasketItems)
            {
                item.Price = item.Product.Price;
                total += item.Count * item.Price;
            }

            Order order = new()
            {
                Status = null,
                Address = orderVM.Address,
                PurchaseAt = DateTime.Now,
                AppUserId = user.Id,
                BasketItems = user.BasketItems,
                TotalPrice = total,
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            string body = @"<table border=""1"">
                           <thead>
                              <tr>    
                                    <th>Name</th>    
                                    <th>Price</th>    
                                    <th>Count</th>    
                                </tr>    
                            </thead>
                            <tbody>";
            foreach (var item in order.BasketItems)
            {
                body += @$" <tr>
                                    <td>{item.Product.Name}</td>
                                    <td>{item.Price}</td>
                                    <td>{item.Count}</td>
                            </tr>";

            }

            body += @"</tbody>
                  </table>";

            await _emailService.SendMailAsync(user.Email, "Your Order", body, true);

            return RedirectToAction(nameof(Index), "Home");



        }

        


    }
}

