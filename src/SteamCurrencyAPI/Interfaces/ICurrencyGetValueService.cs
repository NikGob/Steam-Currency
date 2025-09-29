namespace SteamCurrencyAPI.Interfaces
{
    public interface ICurrencyGetValueService
    {
        Task<decimal> GetSteamRate(string currencyType);
    }
}
