using SteamCurrencyAPI.Models;

namespace SteamCurrencyAPI.Interfaces
{
    public interface ICurrencyDbContext
    {
        Task<CurrencyData> GetLatestCurrencyByCodeAsync(string currencyCode);
        Task<List<CurrencyInfo>> GetCurrencyInRangeAsync(string currencyCode, DateOnly start, DateOnly end);
        Task AddCurrencyDateAsync(string currencyCode, CurrencyData newCurrencyDate);
        Task<List<string>> GetAllCurrencyCodesAsync();
        Task<List<Currency>> GetAllCurrenciesAsync();
    }
}
