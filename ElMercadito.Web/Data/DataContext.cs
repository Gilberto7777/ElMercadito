using ElMercadito.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElMercadito.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<BusinessType> BusinessTypes { get; set; }

    }
}