using DemoApi.Model;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
                
        }
        DbSet<Product> products {  get; set; }
    }
}
