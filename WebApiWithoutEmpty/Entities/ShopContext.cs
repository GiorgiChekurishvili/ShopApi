using Microsoft.EntityFrameworkCore;

namespace WebApiWithoutEmpty.Entities
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Category>().HasKey(p => p.Id);
            modelBuilder.Entity<Category>().HasOne(x=>x.User).WithMany(x=> x.Categories).HasForeignKey(x=>x.UserId);
            modelBuilder.Entity<Product>().HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
            modelBuilder.Entity<Category>().Property(p => p.Name).IsRequired().HasColumnType("nvarchar(100)");
            modelBuilder.Entity<Product>().Property(x => x.Price).HasPrecision(10, 2);
            modelBuilder.Entity<Product>().HasOne(x => x.User).WithMany(x => x.Products).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<User>().HasKey(p => p.Id);
            modelBuilder.Entity<Role>().HasKey(p => p.Id);

            modelBuilder.Entity<UserRole>().HasOne(x => x.user).WithMany(x => x.userRoles).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<UserRole>().HasOne(x => x.role).WithMany(x=> x.userRoles).HasForeignKey(x=> x.RoleId);
            modelBuilder.Entity<UserRole>().HasKey(k => new { k.RoleId, k.UserId });

        }
    }
}
