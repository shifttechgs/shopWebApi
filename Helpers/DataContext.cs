using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopWebApi.Entities;
using ShopWebApi.Models;

namespace ShopWebApi.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
           modelBuilder.ApplyConfiguration(new RoleConfiguration());
           
        }
        

        public DbSet<Test> Tests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
       
        
       
    }
}
