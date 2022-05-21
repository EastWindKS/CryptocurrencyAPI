namespace CryptocurrencyAPI.Services.CoinApi.Exceptions;

public class NoDataException : CoinApiException
{
    public NoDataException()
    {
    }

    public NoDataException(string message) : base(message)
    {
    }

    public NoDataException(string message, Exception innerException) : base(message, innerException)
    {
    }
}