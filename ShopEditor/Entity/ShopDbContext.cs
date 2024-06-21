using Microsoft.EntityFrameworkCore;

namespace ShopEditor.Entity;

public class ShopDbContext(DbContextOptions<ShopDbContext> options) : DbContext(options)
{
    public DbSet<DbShopItem> ShopItems { get; set; }
}
