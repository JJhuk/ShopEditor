namespace ShopEditor;

public class ShopEditor(ShopItemManager shopItemManager)
{
    public async Task RunCore(CancellationToken token)
    {
        Console.Write("Enter ShopId : ");
        var shopIdStr = await Console.In.ReadLineAsync(token);
        var shopId = uint.Parse(shopIdStr!);
        Console.Write("Enter itemId. ex) 101, 102 or 101 :");
        var itemIdList = await ParseFromStdIn(token);
        
        Console.Write("Enter Price : ");
        var priceStr = await Console.In.ReadLineAsync(token);
        var price = int.Parse(priceStr!);
        
        var shopItemList = itemIdList
            .Select(itemId => new ShopItem(shopId, itemId, price))
            .ToList();
        
        await shopItemManager.InsertShopItemList(shopItemList);
        Console.WriteLine("Insert Done");
    }
    
    private static async Task<List<int>> ParseFromStdIn(CancellationToken token)
    {
        var readLineAsync = await Console.In.ReadLineAsync(token);

        if (readLineAsync is null)
        {
            throw new InvalidOperationException("Input is null");
        }
        
        var input = readLineAsync.Split([' ', ',']);

        return Array.ConvertAll(input, int.Parse).ToList();
    }
}
