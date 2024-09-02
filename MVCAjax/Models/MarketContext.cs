using Microsoft.EntityFrameworkCore;

namespace MVCAjax.Models
{
    public class MarketContext : DbContext
    {
        public DbSet<Products> Productos { get; set; }


        public MarketContext(DbContextOptions<MarketContext> options) : base(options)
        {
        }

    }
}
