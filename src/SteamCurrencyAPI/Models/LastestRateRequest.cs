namespace SteamCurrencyAPI.Models
{
    public class LastestRateRequest
    {
        public required string CurrencyCode { get; set; }
        public required string CurrencyBaseCode { get; set; }
    }
}
