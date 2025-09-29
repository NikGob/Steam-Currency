namespace SteamCurrencyAPI.Models;

public class LatestRateDto
{
    public required string CurrencyCode { get; set; }
    public required string CurrencyBaseCode { get; set; }
    public CurrencyData CurrencyInfo { get; set; } = new();
}