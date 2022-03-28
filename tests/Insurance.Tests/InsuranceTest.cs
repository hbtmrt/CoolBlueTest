using System.IO;
using Insurance.Api.Controllers;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Insurance.Tests
{
    public class InsuranceTests : IClassFixture<ControllerTestFixture>
    {
        private readonly ControllerTestFixture _fixture;
        private readonly IConfiguration _configuration;

        public InsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json")
                .Build();

            _configuration = configuration;
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceLessThan500Euros_ShouldAdd500EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 500;

            HomeController homeController = new HomeController(_configuration);

            float insurance = homeController.CalculateInsuranceAsync(1).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 1000;

            HomeController homeController = new HomeController(_configuration);

            float insurance = homeController.CalculateInsuranceAsync(2).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000Euros_ShouldAdd2000EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 2000;

            HomeController homeController = new HomeController(_configuration);

            float insurance = homeController.CalculateInsuranceAsync(3).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceLessThan500Euros_AndTypeOfLaptop_ShouldAdd1000EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 1000;

            HomeController homeController = new HomeController(_configuration);

            float insurance = homeController.CalculateInsuranceAsync(4).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_AndTypeOfSmartphone_ShouldAdd1500EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 1500;

            HomeController homeController = new HomeController(_configuration);

            float insurance = homeController.CalculateInsuranceAsync(5).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000Euros_AndTypeOfLaptop_ShouldAdd2000EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 2500;

            HomeController homeController = new HomeController(_configuration);

            float insurance = homeController.CalculateInsuranceAsync(6).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }
    }
}