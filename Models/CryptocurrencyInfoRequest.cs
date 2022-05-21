namespace CryptocurrencyAPI.Models;

public class CryptocurrencyInfoRequest
{
    public string CryptocurrencyId { get; set; }

    public string Period { get; set; } = "1MIN";

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }

    public string CurrencyToExchange { get; set; }
}