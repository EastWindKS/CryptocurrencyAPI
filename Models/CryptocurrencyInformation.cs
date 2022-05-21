using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptocurrencyAPI.Models;

public class CryptocurrencyInformation
{
    [Key]
    public int Id { get; set; }

    public string CryptocurrencyId { get; set; }

    public decimal Price { get; set; }

    public DateTime UpdateTime { get; set; }

    public string ExchangeCurrency { get; set; }

    [ForeignKey("CryptocurrencyId")]
    public Cryptocurrency Cryptocurrency { get; set; }
}