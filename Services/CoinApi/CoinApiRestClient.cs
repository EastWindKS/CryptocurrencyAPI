using Newtonsoft.Json;
using System.Net;
using CryptocurrencyAPI.Services.CoinApi.Exceptions;
using CryptocurrencyAPI.Services.CoinApi.Models;

namespace CryptocurrencyAPI.Services.CoinApi;

public class CoinApiRestClient
{
    private readonly string _apikey;
    private readonly string _webUrl = "https://rest.coinapi.io";

    private static string DateFormat => "yyyy-MM-ddTHH:mm:ss.fff";

    public CoinApiRestClient(string apikey, bool sandbox = false)
    {
        _apikey = apikey;
        if (sandbox)
            _webUrl = "https://rest-sandbox.coinapi.io";
        _webUrl = _webUrl.TrimEnd('/');
    }

    private async Task<T> GetData<T>(string url)
    {
        T data;
        try
        {
            using var handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using var client = new HttpClient(handler, false);
            client.DefaultRequestHeaders.Add("X-CoinAPI-Key", _apikey);
            var response = await client.GetAsync(_webUrl + url).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                await RaiseError(response).ConfigureAwait(false);
            data = await Deserialize<T>(response).ConfigureAwait(false);
        }
        catch (CoinApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new CoinApiException("Unexpected error", ex);
        }

        return data;
    }

    private static async Task RaiseError(HttpResponseMessage response)
    {
        var errorMessage = await Deserialize<ErrorMessage>(response).ConfigureAwait(false);
        var message = errorMessage.Message;
        errorMessage = null;
        var statusCode = (int)response.StatusCode;

        throw statusCode switch
        {
            400 => new BadRequestException(message),
            401 => new UnauthorizedException(message),
            403 => new ForbiddenException(message),
            429 => new TooManyRequestsException(message),
            550 => new NoDataException(message),
            _ => new CoinApiException(message)
        };
    }

    private static async Task<T> Deserialize<T>(HttpResponseMessage responseMessage)
    {
        var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        var data = JsonConvert.DeserializeObject<T>(responseString);
        var obj = data;
        responseString = null;
        data = default;
        return obj;
    }

    public Task<List<Asset>> Metadata_list_assetsAsync()
    {
        return GetData<List<Asset>>(CoinApiEndpointUrls.Assets());
    }

    public Task<IEnumerable<ExchangeRateHistory>> GetExchangeRateHistory(string assetId, string exchangeCurrencyId, string period, DateTime timeStart,
        DateTime timeEnd)
    {
        return GetData<IEnumerable<ExchangeRateHistory>>(CoinApiEndpointUrls.ExchangeRateHistory(assetId, exchangeCurrencyId, period,
            timeStart.ToString(DateFormat),
            timeEnd.ToString(DateFormat)));
    }
}