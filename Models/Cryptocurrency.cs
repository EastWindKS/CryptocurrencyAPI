using System.ComponentModel.DataAnnotations;

namespace CryptocurrencyAPI.Models;

public class Cryptocurrency
{
    [Key]
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public List<CryptocurrencyInformation> CryptocurrencyInformation { get; set; }
}