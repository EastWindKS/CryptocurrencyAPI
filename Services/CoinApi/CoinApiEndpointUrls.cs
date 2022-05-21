namespace CryptocurrencyAPI.Services.CoinApi;

public static class CoinApiEndpointUrls
{
    public static string Assets() => "/v1/assets";

    public static string ExchangeRateHistory(string assetId, string exchangeCurrencyId, string period, string timeStart, string timeEnd)
    {
        return $"/v1/exchangerate/{assetId}/{exchangeCurrencyId}/history/?period_id={period}&time_start={timeStart}&time_end={timeEnd}";
    }
}