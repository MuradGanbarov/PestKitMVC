using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PestKit.DAL;
using PestKit.Models;
using PestKit.ViewModels;
using static System.Net.WebRequestMethods;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PestKit.Services
{
    public class LayoutService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppDbContext _context;
        private HttpRequest _request;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(IHttpContextAccessor contextAccessor, AppDbContext context,UserManager<AppUser> userManager)
        {
            _contextAccessor = contextAccessor;
            _context = context;
            _request = _contextAccessor.HttpContext.Request;
            _userManager = userManager;
        }

        public async Task<List<BasketItemVM>> GetBasketItemsAsync()
        {
            List<BasketItemVM> basketVM = new List<BasketItemVM>();
            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {

                var user = await _userManager.Users.
                    Include(u => u.BasketItems.Where(bi => bi.OrderId == null)).
                    ThenInclude(bi => bi.Product).
                    FirstOrDefaultAsync(u => u.Id == _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                foreach (var item in user.BasketItems)
                {
                    basketVM.Add(new BasketItemVM
                    {

                        Id = item.ProductId,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        Count = item.Count,
                        SubTotal = (item.Count * item.Product.Price),
                        Img = item.Product.Img

                    });

                }
            }
            else
            {
                List<BasketItemVM> bilist;
                if (_request.Cookies["Basket"] is not null)
                {
                    List<BasketCookiesItemVM> bcilist = JsonConvert.DeserializeObject<List<BasketCookiesItemVM>>(_request.Cookies["Basket"]);

                    bilist = new();

                    foreach (BasketCookiesItemVM basketCookieItem in bcilist)
                    {
                        Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketCookieItem.Id);

                        if (product is not null)
                        {
                            BasketItemVM basketItem = new()
                            {
                                Id = basketCookieItem.Id,
                                Count = basketCookieItem.Quantity,
                                Name = product.Name,
                                Price = product.Price,
                                SubTotal = product.Price * basketCookieItem.Quantity
                            };

                            bilist.Add(basketItem);

                        }
                        else
                        {
                            bilist = new();
                        }
                    }


                }
            }

            return basketVM;
        }

        public async Task<List<BasketItemVM>> GetBasketItemsAsync(List<BasketCookiesItemVM> bcilist)
        {
            List<BasketItemVM> bilist = new();

         
                if (_request.Cookies["Basket"] is not null)
                {
                    foreach (BasketCookiesItemVM basketCookieItem in bcilist)
                    {
                        Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketCookieItem.Id);

                        if (product is not null)
                        {
                            BasketItemVM basketItem = new()
                            {
                                Id = basketCookieItem.Id,
                                Count = basketCookieItem.Quantity,
                                Name = product.Name,
                                Price = product.Price,
                                SubTotal = product.Price * basketCookieItem.Quantity
                            };

                            bilist.Add(basketItem);

                        }
                        else
                        {
                            bilist = new();
                        }
                    }


                }
           

            return bilist;
        }



    }
}
