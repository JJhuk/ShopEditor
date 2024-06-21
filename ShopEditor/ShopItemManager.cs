using Microsoft.EntityFrameworkCore;
using ShopEditor.Entity;

namespace ShopEditor;

public sealed record ShopItem(uint ShopItemId, uint ShopId, int ItemId, int Price, int Position);

public sealed class ShopItemManager(ShopDbContext shopDbContext)
{
    private const int ShopPositionOffset = 4;
    
    private async Task<int> GetHighestPosition(uint shopId, CancellationToken cancellationToken = default)
    {
        // ref ShopItem comment
        const int defaultPosition = 100;
        return await shopDbContext.ShopItems
            .Where(shopItem => shopItem.ShopId == shopId)
            .Select(shopItem => shopItem.Position)
            .DefaultIfEmpty(defaultPosition)
            .MaxAsync(cancellationToken);
    }
    
    public async Task InsertShopItem(ShopItem shopItem)
    {
        var highestPosition = await GetHighestPosition(shopItem.ShopId);

        await shopDbContext.AddAsync(new DbShopItem
        {
            ItemId = shopItem.ItemId,
            Position = highestPosition + ShopPositionOffset,
            Price = shopItem.Price,
            ShopId = shopItem.ShopId,
        });

        await shopDbContext.SaveChangesAsync();
    }
}
