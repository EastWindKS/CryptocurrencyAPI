using CryptocurrencyAPI.Models;
using CryptocurrencyAPI.Services.CoinApi;
using CryptocurrencyAPI.Services.CoinApi.Models;

namespace CryptocurrencyAPI.Services;

public static class Mapper
{
    public static IEnumerable<Cryptocurrency> FromAssets(IEnumerable<Asset> assets)
    {
        return assets.Select(a => new Cryptocurrency { Id = a.asset_id, Name = a.name });
    }

    public static Dictionary<string, IEnumerable<CryptocurrencyInformation>> FromExchangeRateHistory(
        Dictionary<(string cryptocurrency, string exchangeCurrency), IEnumerable<ExchangeRateHistory>> exchangeRatesDictionary)
    {
        var dictionary = new Dictionary<string, IEnumerable<CryptocurrencyInformation>>();

        foreach (var (key, value) in exchangeRatesDictionary)
        {
            dictionary.Add($"{key.cryptocurrency}/{key.exchangeCurrency}", Convert(key.cryptocurrency, key.exchangeCurrency, value));
        }

        return dictionary;
    }

    private static IEnumerable<CryptocurrencyInformation> Convert(string cryptocurrency, string exchangeCurrency,
        IEnumerable<ExchangeRateHistory> exchangeRateHistories)
    {
        return exchangeRateHistories.Select(e => new CryptocurrencyInformation
        {
            CryptocurrencyId = cryptocurrency,
            ExchangeCurrency = exchangeCurrency,
            Price = e.rate_close,
            UpdateTime = e.time_close
        });
    }
}