using CryptocurrencyAPI.Models;

namespace CryptocurrencyAPI.Data.Interfaces;

public interface IWebSocket
{
    void SubscribeOnChangePrice(IEnumerable<CryptocurrencyInfoRequest> cryptocurrencyInfoRequests);
}