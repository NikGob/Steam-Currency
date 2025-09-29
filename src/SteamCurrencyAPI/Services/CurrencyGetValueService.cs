using SteamCurrencyAPI.Interfaces;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SteamCurrencyAPI.Services;

public class CurrencyGetValueService(IConfiguration configuration)
    : ICurrencyGetValueService
{
    private readonly string _steamApiUrl = configuration.GetValue<string>("SteamAPI:URL") 
        ?? throw new Exception("SteamApiUrl Not Found");

    public async Task<decimal> GetSteamRate(string currencyType)
    {
        try
        {
            var value1 = ParseCurrency(await GetCurrency(currencyType));

            var value2 = ParseCurrency(await GetCurrency("USD"));

            var value3 = decimal.Divide(value1, value2);

            return (value3);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    public decimal ParseCurrency(string input)
    {
        var match = Regex.Match(input, @"[\d\s,\.]+");
        if (!match.Success)
            throw new ArgumentException("The number is not found in the string.");

        string numberStr = match.Value.Trim();

        numberStr = numberStr.Replace(" ", "");
        if (numberStr.Contains(",") && numberStr.Contains("."))
        {
            numberStr = numberStr.Replace(",", "");
        }
        else if (numberStr.Contains(",") && !numberStr.Contains("."))
        {
            numberStr = numberStr.Replace(",", ".");
        }
        return decimal.Parse(numberStr, CultureInfo.InvariantCulture);
    }
    public async Task<string> GetCurrency(string currencyType)
    {
        using var client = new HttpClient();

        var link = _steamApiUrl
            .Replace("{appId}", "730")
            .Replace("{product}", "AK-47 | Redline (Field-Tested)")
            .Replace("{currencyId}", GetCurrencyIdByCode(currencyType));

        var response = await client.GetAsync(link);

        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(responseBody);

        return json["median_price"]?.ToString() ?? "Not found";
    }
    public string GetCurrencyIdByCode(string code)
    {
        var keyValues = new Dictionary<string, int>() {
            { "USD", 1 },
            { "GBP", 2 },
            { "EUR", 3 },
            { "CHF", 4 },
            { "RUB", 5 },
            { "PLN", 6 },
            { "BRL", 7 },
            { "JPY", 8 },
            { "NOK", 9 },
            { "IDR", 10 },
            { "MYR", 11 },
            { "PHP", 12 },
            { "SGD", 13 },
            { "THB", 14 },
            { "VND", 15 },
            { "KRW", 16 },
            { "TRY", 17 },
            { "UAH", 18 },
            { "MXN", 19 },
            { "CAD", 20 },
            { "AUD", 21 },
            { "NZD", 22 },
            { "CNY", 23 },
            { "INR", 24 },
            { "CLP", 25 },
            { "PEN", 26 },
            { "COP", 27 },
            { "ZAR", 28 },
            { "HKD", 29 },
            { "TWD", 30 },
            { "SAR", 31 },
            { "AED", 32 },
            { "SEK", 33 },
            { "ARS", 34 },
            { "ILS", 35 },
            { "BYN", 36 },
            { "KZT", 37 },
            { "KWD", 38 },
            { "QAR", 39 },
            { "CRC", 40 },
            { "UYU", 41 },
            { "BGN", 42 },
            { "HRK", 43 },
            { "CZK", 44 },
            { "DKK", 45 },
            { "HUF", 46 },
            { "RON", 47 }};

        return $"{keyValues.GetValueOrDefault(code)}";
    }
}