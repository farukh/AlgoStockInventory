using Microsoft.EntityFrameworkCore;

namespace AlgoStockInventory.Models
{
    public class AlgoStockDbContext : DbContext
    {
        public AlgoStockDbContext(DbContextOptions<AlgoStockDbContext> options) : base(options) { }
        public DbSet<Products> Products { get; set; }
        public DbSet<Stocks> Stocks{ get; set; }
        public DbSet<StockPurchases> StockPurchases { get; set; }
        public DbSet<StockSale> StockSale { get; set; }


    }
}
