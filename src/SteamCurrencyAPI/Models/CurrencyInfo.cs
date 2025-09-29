namespace SteamCurrencyAPI.Models;

public class CurrencyInfo
{
    public DateOnly DateAtUtc { get; set; }
    public decimal CurrencyPrice { get; set; }
}