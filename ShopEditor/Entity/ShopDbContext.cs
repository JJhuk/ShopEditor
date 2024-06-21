using Microsoft.EntityFrameworkCore;

namespace ShopEditor.Entity;

public class ShopDbContext : DbContext
{
    public DbSet<DbShopItem> ShopItems { get; set; }
}
