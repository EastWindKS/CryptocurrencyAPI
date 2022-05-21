using CryptocurrencyAPI.Data.Contexts;
using CryptocurrencyAPI.Data.Interfaces;
using CryptocurrencyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptocurrencyAPI.Data.Repositories;

public class CryptocurrencyInformationRepository : ICryptocurrencyInformationRepository
{
    private readonly MainDbContext _context;

    public CryptocurrencyInformationRepository(MainDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }

    public async Task<IEnumerable<CryptocurrencyInformation>> GetAsync(string[] ids)
    {
        return await _context.CryptocurrencyInformation.Where(c => ids.Contains(c.CryptocurrencyId)).ToListAsync();
    }

    public void Insert(CryptocurrencyInformation cryptocurrencyInformation)
    {
        _context.CryptocurrencyInformation.Add(cryptocurrencyInformation);
        _context.SaveChanges();
    }
}