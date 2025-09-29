using SteamCurrencyAPI.Models;

namespace SteamCurrencyAPI.Interfaces;

public interface ICurrencyService
{
    Task<LatestRateDto> GetLatestRate(LastestRateRequest latestRateRequest);
    Task<RateDTO> GetRate(RateRequest rateRequest);
    Task<List<string>> GetAllCurrencyCodes();
}