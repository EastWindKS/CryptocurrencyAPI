using CryptocurrencyAPI.Models;

namespace CryptocurrencyAPI.Data.Interfaces;

public interface ICryptocurrencyInformationRepository
{
    Task<bool> SaveChangesAsync();

    Task<IEnumerable<CryptocurrencyInformation>> GetAsync(string[] ids);

    void Insert(CryptocurrencyInformation cryptocurrencyInformation);
}