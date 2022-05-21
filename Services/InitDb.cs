using CryptocurrencyAPI.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CryptocurrencyAPI.Services;

public static class InitDb
{
    public static void InitData(IApplicationBuilder builder, IConfiguration configuration)
    {
        using var serviceScope = builder.ApplicationServices.CreateScope();
        var appDbContext = serviceScope.ServiceProvider.GetService<MainDbContext>();
        Seed(appDbContext, configuration);
    }

    private static void Seed(MainDbContext appDbContext, IConfiguration configuration)
    {
        var isExist = appDbContext.Database.CanConnect();

        if (!isExist)
        {
            appDbContext.Database.Migrate();
        }

        if (!appDbContext.Cryptocurrencies.Any())
        {
            var coinApiWorker = new CoinApiWorker(configuration);
            var cryptocurrencies = coinApiWorker.GetCryptocurrencies().Result;
            appDbContext.Cryptocurrencies.AddRange(cryptocurrencies);
            appDbContext.SaveChanges();
        }
    }
}