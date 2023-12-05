using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PestKit.DAL;
using PestKit.Models;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options => { options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = default;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.Lockout.MaxFailedAccessAttempts = 3;

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
   name: "areas",
   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute("default",
    "{controller=home}/{action=index}/{id?}");

app.Run();
