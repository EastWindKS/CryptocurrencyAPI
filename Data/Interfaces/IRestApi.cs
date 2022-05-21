using CryptocurrencyAPI.Models;

namespace CryptocurrencyAPI.Data.Interfaces;

public interface IRestApi
{
    Task<IEnumerable<Cryptocurrency>> GetCryptocurrencies();

    Task<Dictionary<string, IEnumerable<CryptocurrencyInformation>>> GetPriceInformation(IEnumerable<CryptocurrencyInfoRequest> cryptocurrencyInfoRequests);
}