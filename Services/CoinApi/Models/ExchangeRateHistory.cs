using Newtonsoft.Json;

namespace CryptocurrencyAPI.Services.CoinApi.Models;

public class ExchangeRateHistory
{
    public DateTime time_period_start { get; set; }

    public DateTime time_period_end { get; set; }
    public DateTime time_open { get; set; }
    public DateTime time_close { get; set; }

    public decimal rate_open { get; set; }

    public decimal rate_high { get; set; }

    public decimal rate_low { get; set; }

    public decimal rate_close { get; set; }

    [JsonIgnore]
    public string asset_id { get; set; }

    [JsonIgnore]
    public string asset_id_quote { get; set; }
}