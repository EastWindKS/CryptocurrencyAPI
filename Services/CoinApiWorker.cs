using CryptocurrencyAPI.Data.Interfaces;
using CryptocurrencyAPI.Models;
using CryptocurrencyAPI.Services.CoinApi;
using CryptocurrencyAPI.Services.CoinApi.Models;

namespace CryptocurrencyAPI.Services;

public class CoinApiWorker : IRestApi
{
    private readonly string _apiKey;

    private readonly IWebSocket _webSocketWorker;

    public CoinApiWorker(IConfiguration configuration)
    {
        _apiKey = configuration.GetSection("API_KEY").Value;
    }

    public CoinApiWorker(IConfiguration configuration, IWebSocket webSocketWorker)
    {
        _webSocketWorker = webSocketWorker;
        _apiKey = configuration.GetSection("API_KEY").Value;
    }

    public async Task<IEnumerable<Cryptocurrency>> GetCryptocurrencies()
    {
        var coinApiRestClient = new CoinApiRestClient(_apiKey);
        var assets = await coinApiRestClient.Metadata_list_assetsAsync();

        return Mapper.FromAssets(assets.Where(a => a.type_is_crypto));
    }

    public async Task<Dictionary<string,IEnumerable<CryptocurrencyInformation>>> GetPriceInformation(
        IEnumerable<CryptocurrencyInfoRequest> cryptocurrencyInfoRequests)
    {
        var coinApiRestClient = new CoinApiRestClient(_apiKey);

        var dictionary = new Dictionary<(string cryptocurrency, string exchangeCurrency), IEnumerable<ExchangeRateHistory>>();

        var requests = cryptocurrencyInfoRequests
            .Select(c =>
            {
                var exchangeRateHistory = coinApiRestClient
                    .GetExchangeRateHistory(c.CryptocurrencyId, c.CurrencyToExchange, c.Period, c.TimeStart, c.TimeEnd);

                return exchangeRateHistory
                    .ContinueWith(_ => dictionary.Add((c.CryptocurrencyId, c.CurrencyToExchange), exchangeRateHistory.Result));
            });

        await Task.WhenAll(requests);
        
        _webSocketWorker.SubscribeOnChangePrice(cryptocurrencyInfoRequests);

        return Mapper.FromExchangeRateHistory(dictionary);
    }
}