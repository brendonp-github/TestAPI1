using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestAPI1.Data.Models;

namespace TestAPI1.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext() : base() { }
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
        public DbSet<Post> Post => Set<Post>();
        public DbSet<Recipe> Recipe => Set<Recipe>();
        public DbSet<Ingredient> Ingredient => Set<Ingredient>();
    }
}
