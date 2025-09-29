using Quartz;
using SteamCurrencyAPI.Interfaces;
using SteamCurrencyAPI.Models;

namespace SteamCurrencyAPI.Jobs;

public class CurrencyUpdaterJob(ICurrencyDbContext currencyDbContext, ICurrencyGetValueService currencyGetValueService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var currencies = await currencyDbContext.GetAllCurrenciesAsync();

        foreach (var currency in currencies)
        {
            var steamRate = await currencyGetValueService.GetSteamRate(currency.CurrencyCode);

            var currencyData = new CurrencyData()
            {
                CurrencyPrice = steamRate,
                DateAtUtc = DateTime.UtcNow
            };

            await currencyDbContext.AddCurrencyDateAsync(currency.CurrencyCode, currencyData);
        }

        await Task.CompletedTask;
    }
}