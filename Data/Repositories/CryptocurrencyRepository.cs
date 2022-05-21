using CryptocurrencyAPI.Data.Contexts;
using CryptocurrencyAPI.Data.Interfaces;
using CryptocurrencyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptocurrencyAPI.Data.Repositories;

public class CryptocurrencyRepository : ICryptocurrencyRepository
{
    private readonly MainDbContext _context;

    public CryptocurrencyRepository(MainDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }

    public async Task<IEnumerable<Cryptocurrency>> GetAsync(string[] ids)
    {
        return await _context.Cryptocurrencies.Where(c => ids.Contains(c.Id)).ToListAsync();
    }
    
    public async Task<IEnumerable<Cryptocurrency>> GetAllAsync()
    {
        return await _context.Cryptocurrencies.ToListAsync();
    }

    public async Task<Cryptocurrency> GetByIdAsync(string id)
    {
        return await _context.Cryptocurrencies.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task InsertAsync(Cryptocurrency cryptocurrency)
    {
        await _context.Cryptocurrencies.AddAsync(cryptocurrency);
        await _context.SaveChangesAsync();
    }
}