using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ShopEditor.Entity;

public class ShopDbContext(DbContextOptions<ShopDbContext> options) : DbContext(options)
{
    public required DbSet<DbShopItem> ShopItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopDbContext).Assembly);
    }
}
