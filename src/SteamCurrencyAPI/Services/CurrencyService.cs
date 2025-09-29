using SteamCurrencyAPI.Interfaces;
using SteamCurrencyAPI.Models;

namespace SteamCurrencyAPI.Services;

public class CurrencyService(ICurrencyDbContext currencyDbContext) : ICurrencyService
{
    public async Task<LatestRateDto> GetLatestRate(LastestRateRequest latestRateRequest)
    {
        if (latestRateRequest == null)
            throw new Exception("LatestRateRequest is null");
        if (latestRateRequest.CurrencyCode == latestRateRequest.CurrencyBaseCode)
            throw new Exception("CurrencyCode and CurrencyBaseCode cannot be the same.");

        if (latestRateRequest.CurrencyCode == "USD")
        {
            var currencyBaseData = await currencyDbContext.GetLatestCurrencyByCodeAsync(latestRateRequest.CurrencyBaseCode)
                                   ?? throw new Exception("currencyBaseData not found");

            var currencyInfo = new CurrencyData
            {
                CurrencyPrice = decimal.Divide(1, currencyBaseData.CurrencyPrice),
                DateAtUtc = currencyBaseData.DateAtUtc,
            };

            return new LatestRateDto()
            {
                CurrencyBaseCode = latestRateRequest.CurrencyBaseCode,
                CurrencyCode = latestRateRequest.CurrencyCode,
                CurrencyInfo = currencyInfo
            };
        }
        if (latestRateRequest.CurrencyBaseCode == "USD")
        {
            var currencyInfo = await currencyDbContext.GetLatestCurrencyByCodeAsync(latestRateRequest.CurrencyCode)
                               ?? throw new Exception("currencyData not found");

            return new LatestRateDto
            {
                CurrencyCode = latestRateRequest.CurrencyCode,
                CurrencyBaseCode = latestRateRequest.CurrencyBaseCode,
                CurrencyInfo = currencyInfo
            };
        }
        else
        {
            var currencyBaseData = await currencyDbContext.GetLatestCurrencyByCodeAsync(latestRateRequest.CurrencyBaseCode)
                                   ?? throw new Exception("currencyBaseData not found");
            var currencyData = await currencyDbContext.GetLatestCurrencyByCodeAsync(latestRateRequest.CurrencyCode)
                                   ?? throw new Exception("currencyData not found");

            if (currencyBaseData.DateAtUtc.Date != currencyData.DateAtUtc.Date)
                throw new InvalidOperationException("Date validation error: 'currencyBaseData.DateAtUtc' does not correspond to 'currencyData.DateAtUtc'.");

            var currencyInfo = new CurrencyData()
            {
                CurrencyPrice = decimal.Divide(currencyData.CurrencyPrice, currencyBaseData.CurrencyPrice),
                DateAtUtc = currencyBaseData.DateAtUtc,
            };

            return new LatestRateDto()
            {
                CurrencyBaseCode = latestRateRequest.CurrencyBaseCode,
                CurrencyCode = latestRateRequest.CurrencyCode,
                CurrencyInfo = currencyInfo
            };
        }
    }

    public async Task<RateDTO> GetRate(RateRequest rateRequest)
    {
        if (rateRequest == null)
            throw new Exception("LatestRateRequest is null");
        if (rateRequest.CurrencyCode == rateRequest.CurrencyBaseCode)
            throw new Exception("CurrencyCode and CurrencyBaseCode cannot be the same.");

        if (rateRequest.CurrencyCode == "USD")
        {
            List<CurrencyInfo> currencyDatas = await currencyDbContext.GetCurrencyInRangeAsync(rateRequest.CurrencyBaseCode, rateRequest.StartDate, rateRequest.EndDate)
                               ?? throw new Exception("currencyData not found");

            List<CurrencyInfo> currencyInfos = new List<CurrencyInfo>();

            foreach (var data in currencyDatas)
            {
                var existingCurrencyInfo = currencyInfos.FirstOrDefault(currencyInfos => data.DateAtUtc == currencyInfos.DateAtUtc);

                if (existingCurrencyInfo == null)
                {
                    currencyInfos.Add(new CurrencyInfo
                    {
                        DateAtUtc = data.DateAtUtc,
                        CurrencyPrice = decimal.Divide(1, data.CurrencyPrice)
                    });
                }
            }
            return new RateDTO()
            {
                CurrencyBaseCode = rateRequest.CurrencyBaseCode,
                CurrencyCode = rateRequest.CurrencyCode,
                CurrencyInfo = currencyInfos
            };
        }
        if (rateRequest.CurrencyBaseCode == "USD")
        {
            List<CurrencyInfo> currencyInfos = await currencyDbContext.GetCurrencyInRangeAsync(rateRequest.CurrencyCode, rateRequest.StartDate, rateRequest.EndDate)
                               ?? throw new Exception("currencyData not found");

            return new RateDTO
            {
                CurrencyCode = rateRequest.CurrencyCode,
                CurrencyBaseCode = rateRequest.CurrencyBaseCode,
                CurrencyInfo = currencyInfos
            };
        }
        else
        {

            List<CurrencyInfo> currencyDatas = await currencyDbContext.GetCurrencyInRangeAsync(rateRequest.CurrencyCode, rateRequest.StartDate, rateRequest.EndDate)
                               ?? throw new Exception("currencyData not found");
            List<CurrencyInfo> currencyBaseDatas = await currencyDbContext.GetCurrencyInRangeAsync(rateRequest.CurrencyBaseCode, rateRequest.StartDate, rateRequest.EndDate)
                               ?? throw new Exception("currencyData not found");

            List<CurrencyInfo> currencyInfos = new List<CurrencyInfo>();

            if (currencyBaseDatas.Count != currencyDatas.Count)
                throw new InvalidOperationException("Date validation error: 'currencyBaseDatas' and 'currencyDatas' do not have the same count.");


            foreach (var cBaseData in currencyBaseDatas)
            {
                foreach (var cData in currencyDatas)
                {
                    if (cBaseData.DateAtUtc == cData.DateAtUtc)
                    {
                        var existingCurrencyInfo = currencyInfos.FirstOrDefault(currencyInfos => cBaseData.DateAtUtc == currencyInfos.DateAtUtc);

                        if (existingCurrencyInfo == null)
                        {
                            currencyInfos.Add(new CurrencyInfo
                            {
                                DateAtUtc = cData.DateAtUtc,
                                CurrencyPrice = decimal.Divide(cData.CurrencyPrice, cBaseData.CurrencyPrice)
                            });
                        }

                    }

                }
            }
            return new RateDTO
            {
                CurrencyBaseCode = rateRequest.CurrencyBaseCode,
                CurrencyCode = rateRequest.CurrencyCode,
                CurrencyInfo = currencyInfos
            };
        }
    }

    public async Task<List<string>> GetAllCurrencyCodes()
    {
        var codes = await currencyDbContext.GetAllCurrencyCodesAsync();

        return codes;
    }
}