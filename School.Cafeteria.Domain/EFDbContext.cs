using Microsoft.EntityFrameworkCore;

using School.Cafeteria.Domain.Entities;
using School.Cafeteria.Domain.Seed;

namespace School.Cafeteria.Domain
{
    public class EFDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Product> Products { get; set; }

        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SeedProducts();

            base.OnModelCreating(modelBuilder);
        }
    }
}
