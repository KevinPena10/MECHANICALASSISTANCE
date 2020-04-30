using MechanicalAssistance.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MechanicalAssistance.Web.Data
{
    public class DataContext : IdentityDbContext<UserEntity>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<MechanicalServiceEntity> Services { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductBrandEntity> ProductBrands { get; set; }
        public DbSet<RequestServiceEntity> RequestServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductBrandEntity>()
                .HasIndex(P => P.BrandName)
                .IsUnique();
        }

    }
}
