namespace ShopEditor;

public class ShopEditor
{
    private readonly ShopItemManager shopItemManager;

    public ShopEditor(ShopItemManager shopItemManager)
    {
        this.shopItemManager = shopItemManager;
    }

    public async Task Run(CancellationToken token)
    {
        Console.Write("Enter ShopId : ");
        var shopIdStr = await Console.In.ReadLineAsync(token);
        var shopId = int.Parse(shopIdStr!);
        Console.Write("Enter itemId. ex) 101, 102 or 101 :");
        var itemIdList = await ParseFromStdIn(token);
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
