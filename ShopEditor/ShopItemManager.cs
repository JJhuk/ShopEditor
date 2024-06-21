using Microsoft.EntityFrameworkCore;
using ShopEditor.Entity;

namespace ShopEditor;

public sealed record ShopItem(uint ShopId, int ItemId, int Price);

public sealed class ShopItemManager(ShopDbContext shopDbContext)
{
    private const int ShopPositionOffset = 4;
    
    private async Task<int> GetHighestPosition(uint shopId, CancellationToken cancellationToken = default)
    {
        // sort is an arbitrary field designed to give leeway when modifying shops.
        // The lowest number is 104, and it increments by 4 for each item
        // to allow decent space for swapping/inserting/removing items.
        const int defaultPosition = 104;
        
        return await shopDbContext.ShopItems
            .Where(shopItem => shopItem.ShopId == shopId)
            .Select(shopItem => shopItem.Position)
            .DefaultIfEmpty(defaultPosition)
            .MaxAsync(cancellationToken);
    }
    
    public async Task InsertShopItemList(List<ShopItem> shopItemToInsert)
    {
        var highestPosition = await GetHighestPosition(shopItemToInsert.First().ShopId);

        var dbShopItemList = shopItemToInsert
            .Select((shopItem, index) => new DbShopItem
            {
                ItemId = shopItem.ItemId,
                Position = highestPosition + ShopPositionOffset * index,
                Price = shopItem.Price,
                ShopId = shopItem.ShopId,
            })
            .ToList();

        await shopDbContext.AddRangeAsync(dbShopItemList);
        await shopDbContext.SaveChangesAsync();
    }
}
