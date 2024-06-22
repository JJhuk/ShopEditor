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
        // 104 - offset 4 = 100
        const int defaultPosition = 100;

        var positions = await shopDbContext.ShopItems
            .Where(shopItem => shopItem.ShopId == shopId)
            .Select(shopItem => shopItem.Position)
            .ToListAsync(cancellationToken);

        return positions
            .DefaultIfEmpty(defaultPosition)
            .Max();
    }

    public async Task SortPosition(uint shopId, CancellationToken token)
    {
        var shopItems = await shopDbContext.ShopItems
            .Where(shopItem => shopItem.ShopId == shopId)
            .OrderBy(shopItem => shopItem.Position)
            .ToListAsync(token);

        for (var i = 0; i < shopItems.Count; i++)
        {
            shopItems[i].Position = i + 1;
        }

        await shopDbContext.SaveChangesAsync(token);
    }
    
    public async Task InsertShopItemList(List<ShopItem> shopItemToInsert, CancellationToken token)
    {
        var highestPosition = await GetHighestPosition(shopItemToInsert.First().ShopId, token);

        var dbShopItemList = shopItemToInsert
            .Select((shopItem, index) => new DbShopItem
            {
                ItemId = shopItem.ItemId,
                Position = highestPosition + ShopPositionOffset * (index + 1),
                Price = shopItem.Price,
                ShopId = shopItem.ShopId,
            })
            .ToList();

        await shopDbContext.AddRangeAsync(dbShopItemList, token);
        await shopDbContext.SaveChangesAsync(token);
    }
}
