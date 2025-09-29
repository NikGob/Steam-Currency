using Microsoft.Extensions.Configuration;
using Moq.AutoMock;
using SteamCurrencyAPI.DataWrapper;
using SteamCurrencyAPI.Interfaces;
using SteamCurrencyAPI.Models;
using SteamCurrencyAPI.Services;
using Xunit;

namespace SteamCurrencyAPI.Tests.Services
{
    public class CurrencyServiceTest
    {
        private CurrencyService Sut { get; set; }
        private AutoMocker _Mocker { get; set; }

        public CurrencyServiceTest()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

            var config = builder.Build();

            _Mocker = new AutoMocker();

            _Mocker.Use(config);

            _Mocker.Use<IConfiguration>(config);

            _Mocker.Use<ICurrencyDbContext>(_Mocker.CreateInstance<CurrencyDbContext>());

            //_Mocker.GetMock<ICurrencyDbContext>()//Todo write this example in Obsidian 
            //    .Setup(x => x.GetLatestCurrencyByCodeAsync(It.IsAny<string>()))
            //    .ReturnsAsync(new CurrencyData { CurrencyPrice = 43, DateAtUtc = DateTime.UtcNow });

            Sut = _Mocker.CreateInstance<CurrencyService>();
        }

        [Fact(Skip = "Not working, fix method for this test")]
        public async Task GetLastestRateTest()
        {
            var result = await Sut.GetLatestRate(new LastestRateRequest() { CurrencyBaseCode = "USD", CurrencyCode = "RUB" });
        }
    }
}
