using CryptocurrencyAPI.Models;

namespace CryptocurrencyAPI.Services;

internal static class FastEnumToString
{
    internal static string SubscribeDataTypeEnumToString(SubscribeDataTypeEnum subscribeDataTypeEnum)
    {
        return subscribeDataTypeEnum switch
        {
            SubscribeDataTypeEnum.exrate => nameof(SubscribeDataTypeEnum.exrate),
            _ => throw new ArgumentOutOfRangeException(nameof(subscribeDataTypeEnum), subscribeDataTypeEnum, null)
        };
    }
}