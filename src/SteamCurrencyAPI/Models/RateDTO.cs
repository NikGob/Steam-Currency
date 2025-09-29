namespace SteamCurrencyAPI.Models
{
    public class RateDTO
    {
        public required string CurrencyCode { get; set; }
        public required string CurrencyBaseCode { get; set; }
        public List<CurrencyInfo>? CurrencyInfo { get; set; }
    }
}
