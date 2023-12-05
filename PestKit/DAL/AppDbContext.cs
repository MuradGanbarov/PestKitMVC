using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PestKit.Models;

namespace PestKit.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Employee> Employees { get; set;}
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<Tag> Tags { get; set; } 
        public DbSet<Project>? Projects { get; set; }
        public DbSet<ProjectImages>? ProjectImages { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
    }
    
}
