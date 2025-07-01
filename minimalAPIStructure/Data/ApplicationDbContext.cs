using Microsoft.EntityFrameworkCore;
using minimalAPIStructure.Models;
using MinimalAPIStructure.Models;

namespace MinimalAPIStructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : DbContext(options)
    {
        public required DbSet<Order> Orders { get; set; }
        public required DbSet<Customer> Customers { get; set; }
        public required DbSet<Product> Products { get; set; }
    }
}
