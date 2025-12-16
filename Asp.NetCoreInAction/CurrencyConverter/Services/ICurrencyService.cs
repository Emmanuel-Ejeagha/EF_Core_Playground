using System;

namespace CurrencyConverter.Services;

public interface ICurrencyService
{
    decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency);

    Dictionary<string, string> GetCurrencyNames();
    decimal GetExchangeRate(string fromCurrency, string toCurrency);
}
