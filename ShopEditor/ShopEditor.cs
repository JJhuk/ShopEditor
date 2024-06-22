namespace ShopEditor;

public class ShopEditor(ShopItemManager shopItemManager)
{
    public async Task RunCore(CancellationToken token)
    {
        Console.WriteLine("1. Sort Items");
        Console.WriteLine("2. Insert Items");
        Console.Write("Select : ");
        var select = await Console.In.ReadLineAsync(token);

        switch (select)
        {
            case "1":
                await SortItems(token);
                break;
            case "2":
                await InsertItems(token);
                break;
            default:
                Console.WriteLine("Invalid Input");
                break;
        }
    }

    private async Task SortItems(CancellationToken token)
    {
        Console.Write("Enter ShopId : ");
        var shopIdStr = await Console.In.ReadLineAsync(token);
        var shopId = uint.Parse(shopIdStr!);
        await shopItemManager.SortPosition(shopId, token);
        Console.WriteLine("Sort Done");
    }

    private async Task InsertItems(CancellationToken token)
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

        await shopItemManager.InsertShopItemList(shopItemList, token);
        Console.WriteLine("Insert Done");
    }

    private static async Task<List<int>> ParseFromStdIn(CancellationToken token)
    {
        var readLineAsync = await Console.In.ReadLineAsync(token);

        if (readLineAsync is null)
        {
            throw new InvalidOperationException("Input is null");
        }

        var input = readLineAsync.Split(',', ' ');

        return Array.ConvertAll(input, int.Parse).ToList();
    }
}
