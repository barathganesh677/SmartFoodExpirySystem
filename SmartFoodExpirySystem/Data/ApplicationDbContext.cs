using Microsoft.EntityFrameworkCore;
using SmartFoodExpirySystem.Models;

namespace SmartFoodExpirySystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<ShoppingPlanner> ShoppingPlanners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingPlanner>().ToTable("ShoppingPlanner");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<FoodItem>().ToTable("FoodItems");
        }
    }
}