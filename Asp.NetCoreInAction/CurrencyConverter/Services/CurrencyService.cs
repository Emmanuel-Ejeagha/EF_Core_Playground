using System;

namespace CurrencyConverter.Services;

public class CurrencyService : ICurrencyService
{
    private readonly Dictionary<string, decimal> _exchangeRates = new Dictionary<string, decimal>
    {
        // Base: USD
        {"USD_TO_EUR", 0.85m},
        {"USD_TO_GBP", 0.73m},
        {"USD_TO_CAD", 1.25m},
        {"USD_TO_JPY", 110.85m},
        {"USD_TO_AUD", 1.35m},

        // Inverse rates
            {"EUR_TO_USD", 1.18m},
            {"GBP_TO_USD", 1.37m},
            {"CAD_TO_USD", 0.80m},
            {"JPY_TO_USD", 0.0090m},
            {"AUD_TO_USD", 0.74m},
            
            // Cross rates
            {"EUR_TO_GBP", 0.86m},
            {"GBP_TO_EUR", 1.16m},
            {"EUR_TO_CAD", 1.47m},
            {"CAD_TO_EUR", 0.68m},
            {"GBP_TO_CAD", 1.71m},
            {"CAD_TO_GBP", 0.58m}

    };

    public decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
    {
        if (fromCurrency == toCurrency) return amount;

        var rate = GetExchangeRate(fromCurrency, toCurrency);
        return Math.Round(amount * rate, 2);
    }

    public decimal GetExchangeRate(string fromCurrency, string toCurrency)
    {
        if (fromCurrency == toCurrency) return 1;

        var key = $"{fromCurrency.ToUpper()}_TO_{toCurrency.ToUpper()}";

        if (_exchangeRates.ContainsKey(key))
            return _exchangeRates[key];

        // If direct rate not found, try through USD
        var fromToUsd = GetExchangeRate(fromCurrency, "USD");
        var usdToTo = GetExchangeRate("USD", toCurrency);

        return fromToUsd * usdToTo;

    }
    public Dictionary<string, string> GetCurrencyNames()
    {
        return new Dictionary<string, string>
        {
            {"USD", "US Dollar"},
            {"EUR", "EURO"},
            {"GBP", "British Pound"},
            {"CAD", "Canadian Dollar"},
            {"JPY", "Japannese Yen"},
            {"AUD", "Australian Dollar"}
        };
    }
}
