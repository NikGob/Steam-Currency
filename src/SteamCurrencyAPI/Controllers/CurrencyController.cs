using Microsoft.AspNetCore.Mvc;
using SteamCurrencyAPI.Interfaces;
using SteamCurrencyAPI.Models;

namespace SteamCurrencyAPI.Controllers;

[ApiController]
[Route("api/currency")]
public class CurrencyController(ICurrencyService currencyService) : ControllerBase
{
    /// <summary>
    /// EN: Gets the latest exchange rate for a specified currency pair. / RU: Получает последний курс обмена для указанной валютной пары.
    /// </summary>
    /// <remarks>
    /// Sample request / Пример запроса:
    ///
    ///     GET /api/currency/latest-rate?currencyCode=RUB&amp;currencyBaseCode=KZT
    ///
    /// </remarks>
    [HttpGet("latest-rate")]
    public async Task<IActionResult> GetLatestRate([FromQuery] LastestRateRequest latestRateRequest)
    {
        try
        {
            var latestRate = await currencyService.GetLatestRate(latestRateRequest);

            return Ok(new
            {
                latestRate.CurrencyCode,
                latestRate.CurrencyBaseCode,
                CurrencyInfo = new CurrencyInfo
                {
                    DateAtUtc = DateOnly.FromDateTime(latestRate.CurrencyInfo.DateAtUtc),
                    CurrencyPrice = latestRate.CurrencyInfo.CurrencyPrice
                }
            });
        }
        catch (Exception e)
        {
            return BadRequest($"Error getting last currency rate.");
        }
    }

    /// <summary>
    /// EN: Gets the exchange rates for a specified currency pair over a specific period. / RU: Получает курсы обмена для указанной валютной пары за определённый период.
    /// </summary>
    /// <remarks>
    /// Sample request / Пример запроса:
    ///
    ///     GET /api/currency/rates?currencyCode=RUB&amp;currencyBaseCode=KZT&amp;startDate=2023-01-01&amp;endDate=2023-01-31
    ///
    /// Date format / Формат даты: YYYY-MM-DD
    /// </remarks>
    [HttpGet("rates")]
    public async Task<IActionResult> GetRate([FromQuery] RateRequest rateRequest)
    {
        var rate = await currencyService.GetRate(rateRequest);

        return Ok(rate);
    }

    /// <summary>
    /// EN: Gets all available currency codes. / RU: Получает все доступные коды валют.
    /// </summary>
    /// <remarks>
    /// Sample request / Пример запроса:
    ///
    ///     GET /api/currency/codes
    ///
    /// </remarks>
    [HttpGet("codes")]
    public async Task<IActionResult> GetAllCurrencyCodes()
    {
        var codes = await currencyService.GetAllCurrencyCodes();

        return Ok(codes);
    }
}