namespace CryptocurrencyAPI.Services.CoinApi.Exceptions;

public class TooManyRequestsException : CoinApiException
{
    public TooManyRequestsException()
    {
    }

    public TooManyRequestsException(string message) : base(message)
    {
    }

    public TooManyRequestsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}