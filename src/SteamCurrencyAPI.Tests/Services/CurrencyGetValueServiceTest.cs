using Microsoft.Extensions.Configuration;
using Moq.AutoMock;
using SteamCurrencyAPI.DataWrapper;
using SteamCurrencyAPI.Interfaces;
using SteamCurrencyAPI.Services;
using Xunit;

namespace SteamCurrencyAPI.Tests.Services
{
    public class CurrencyGetValueServiceTest
    {
        private CurrencyGetValueService Sut { get; set; }
        private AutoMocker _Mocker { get; set; }

        public CurrencyGetValueServiceTest()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

            var config = builder.Build();

            _Mocker = new AutoMocker();

            _Mocker.Use(config);

            _Mocker.Use<IConfiguration>(config);

            _Mocker.Use<ICurrencyDbContext>(_Mocker.CreateInstance<CurrencyDbContext>());

            Sut = _Mocker.CreateInstance<CurrencyGetValueService>();
        }

        [Fact]
        public async Task GetSteamRateTest()
        {
            var result = await Sut.GetSteamRate("USD");

            Assert.True(result > 1);
        }
    }
}
