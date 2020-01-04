using Microsoft.EntityFrameworkCore;
using EasyPizza.Entities;

namespace EasyPizza.DAL
{
    public class EasyPizzaContext : DbContext
    {
        public EasyPizzaContext(DbContextOptions<EasyPizzaContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }
    }
}
