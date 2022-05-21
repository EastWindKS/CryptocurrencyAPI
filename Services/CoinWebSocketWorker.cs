using CoinAPI.WebSocket.V1;
using CoinAPI.WebSocket.V1.DataModels;
using CryptocurrencyAPI.Data.Interfaces;
using CryptocurrencyAPI.Models;

namespace CryptocurrencyAPI.Services;

public class CoinWebSocketWorker : IWebSocket
{
    private readonly string _apiKey;

    private static readonly Stack<string> Queue = new();

    private readonly IServiceProvider _serviceProvider;

    public CoinWebSocketWorker(IConfiguration configuration, IServiceProvider serviceProviderProvider)
    {
        _apiKey = configuration.GetSection("API_KEY").Value;
        _serviceProvider = serviceProviderProvider;
    }

    public void SubscribeOnChangePrice(IEnumerable<CryptocurrencyInfoRequest> cryptocurrencyInfoRequests)
    {
        var coinApiWsClient = new CoinApiWsClient(true);
        var cryptocurrencyInfoRequestsForSubscribe = CheckSubscribe(cryptocurrencyInfoRequests);
        var subscribe = Subscribe(cryptocurrencyInfoRequestsForSubscribe);
        coinApiWsClient.SendHelloMessage(subscribe);
        coinApiWsClient.Error += ErrorOnSubscribe;
        coinApiWsClient.ExchangeRateEvent += OnExchangeRateEvent;
    }

    private void OnExchangeRateEvent(object sender, ExchangeRate exchangeRate)
    {
        using var scope = _serviceProvider.CreateScope();
        var cryptocurrencyInformationRepository = scope.ServiceProvider.GetRequiredService<ICryptocurrencyInformationRepository>();
        cryptocurrencyInformationRepository.Insert(new CryptocurrencyInformation
        {
            CryptocurrencyId = exchangeRate.asset_id_base,
            ExchangeCurrency = exchangeRate.asset_id_quote,
            Price = exchangeRate.rate,
            UpdateTime = exchangeRate.time
        });
    }

    private static void ErrorOnSubscribe(object sender, Exception e)
    {
        Queue.Clear();
        throw e;
    }

    private Hello Subscribe(IEnumerable<CryptocurrencyInfoRequest> cryptocurrencyInfoRequests)
    {
        var hello = new Hello
        {
            subscribe_data_type = new[] { FastEnumToString.SubscribeDataTypeEnumToString(SubscribeDataTypeEnum.exrate) },
            apikey = Guid.Parse(_apiKey),
            heartbeat = false,
        };

        var assetsForSubscribe = new List<string>();

        foreach (var cryptocurrencyInfoRequest in cryptocurrencyInfoRequests)
        {
            var assetInfo = ParseCryptocurrencyInfoRequest(cryptocurrencyInfoRequest);
            assetsForSubscribe.Add(assetInfo);
        }

        hello.subscribe_filter_asset_id = assetsForSubscribe.ToArray();

        return hello;
    }

    private static IEnumerable<CryptocurrencyInfoRequest> CheckSubscribe(IEnumerable<CryptocurrencyInfoRequest> cryptocurrencyInfoRequests)
    {
        var notSubscribed = new List<CryptocurrencyInfoRequest>();

        foreach (var cryptocurrencyInfoRequest in cryptocurrencyInfoRequests)
        {
            var pair = ParseCryptocurrencyInfoRequest(cryptocurrencyInfoRequest);
            var isInQueue = Queue.Contains(pair);

            if (!isInQueue)
            {
                notSubscribed.Add(cryptocurrencyInfoRequest);
                Queue.Push(pair);
            }
        }

        return notSubscribed;
    }

    private static string ParseCryptocurrencyInfoRequest(CryptocurrencyInfoRequest cryptocurrencyInfoRequest)
    {
        return $"{cryptocurrencyInfoRequest.CryptocurrencyId}/{cryptocurrencyInfoRequest.CurrencyToExchange}";
    }
}