namespace SteamCurrencyAPI.Models
{
    public class RateRequest
    {
        public string CurrencyCode { get; set; }
        public string CurrencyBaseCode { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
