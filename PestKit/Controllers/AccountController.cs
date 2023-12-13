using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PestKit.DAL;
using PestKit.Models;
using PestKit.Utilites.Enums;
using PestKit.ViewModels;

namespace PestKit.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _userrole;
        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> userrole)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userrole=userrole;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser 
            {
                Name = userVM.Name,
                UserName = userVM.Username,
                Surname = userVM.Surname,
                Email = userVM.Email,
                Gender = userVM.Gender.ToString(),
            };

            IdentityResult result = await _userManager.CreateAsync(user,userVM.Password);

            if(!result.Succeeded)
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);

                }
                return View();

            }

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index","Home");

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginVM loginVM,string? returnUrl)
        {
            if(!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);

            if(user is null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);

                if(user is null)
                {
                    ModelState.AddModelError(String.Empty, "This account is not exist");
                    return View();
                }
            }



            var result = await _signInManager.PasswordSignInAsync(user,loginVM.Password,false, true);

            if(result.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "Please try again in 3 minutes");
                return View();
            }


            if(!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty,"Username,password or email is incorrect");
                return View();
            }

            if (returnUrl is null)
            {
               return RedirectToAction("Index", "Home");
            }
            return Redirect(returnUrl);

        }

        public async Task<IActionResult> CreateRole()
        {
            foreach(UserRoles role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _userrole.RoleExistsAsync(role.ToString()))
                {
                    await _userrole.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    });
                }
            }
            return RedirectToAction("Index", "Home");
        }




    }

    
}
