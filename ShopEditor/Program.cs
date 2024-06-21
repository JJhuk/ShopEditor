using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopEditor;
using ShopEditor.Entity;

var cts = new CancellationTokenSource();

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("config.json", optional: false)
    .Build();

var serviceCollections = new ServiceCollection()
    .AddDbContext<ShopDbContext>(config =>
    {
        config.UseMySQL(configurationBuilder.GetConnectionString("ConnectionString")!);
    })
    .AddSingleton<ShopItemManager>()
    .AddSingleton<ShopEditor.ShopEditor>()
    .BuildServiceProvider();


var shopEditor = serviceCollections.GetRequiredService<ShopEditor.ShopEditor>();
await shopEditor.RunCore(cts.Token);
