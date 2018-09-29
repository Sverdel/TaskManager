using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Moq;
using TaskManager.Api.Controllers;
using TaskManager.Api.Models.Dto;
using TaskManager.Core.Model;
using TaskManager.Core.Repository;
using Xunit;

namespace TaskManager.Api.Tests
{

    public class ExchangeRateControllerTests : IClassFixture<StaticFixture>
    {
        private readonly StaticFixture _automapperFixture;
        private Fixture _fixture = new Fixture();

        public ExchangeRateControllerTests(StaticFixture automapperFixture)
        {
            _automapperFixture = automapperFixture;
        }

        [Theory]
        [InlineData(Currency.EUR)]
        [InlineData(Currency.USD)]
        public async Task GetTest(Currency currency)
        {
            var rate = _fixture.Build<ExchangeRate>().With(x => x.Currency, currency).Create();

            var repo = new Mock<IExchangeRepository>();
            repo.Setup(r => r.GetLatest(currency)).Returns(Task.FromResult(rate));
            var controller = new ExchangeRateController(repo.Object);

            var result = await controller.Get(currency.ToString()).ConfigureAwait(false);

            Assert.Equal(Mapper.Map<ExchangeRateDto>(rate), result.Value, new ObjectComparer<ExchangeRateDto>());
        }
    }
}
