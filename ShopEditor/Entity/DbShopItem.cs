using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShopEditor.Entity;

public class DbShopItem
{
    public uint ShopItemId { get; set; }
    public uint ShopId { get; set; }
    public int ItemId { get; set; }
    public int Price { get; set; }
    public int Pitch { get; set; } = 0;
    
    // sort is an arbitrary field designed to give leeway when modifying shops.
    // The lowest number is 104, and it increments by 4 for each item
    // to allow decent space for swapping/inserting/removing items.
    public int Position { get; set; }
}

public class ShopItemsEntityTypeConfiguration : IEntityTypeConfiguration<DbShopItem>
{
    public void Configure(EntityTypeBuilder<DbShopItem> builder)
    {
        builder
            .HasKey(x => x.ShopItemId);
    }
}
