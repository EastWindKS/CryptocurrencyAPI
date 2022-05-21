using CryptocurrencyAPI.Models;

namespace CryptocurrencyAPI.Data.Interfaces;

public interface ICryptocurrencyRepository
{
    Task<bool> SaveChangesAsync();

    Task<IEnumerable<Cryptocurrency>> GetAsync(string[] ids);
    
    Task<IEnumerable<Cryptocurrency>> GetAllAsync();

    Task<Cryptocurrency> GetByIdAsync(string id);

    Task InsertAsync(Cryptocurrency cryptocurrency);
}