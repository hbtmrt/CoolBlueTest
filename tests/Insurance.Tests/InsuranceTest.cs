using System.Collections.Generic;
using System.IO;
using Insurance.Api.Controllers;
using Insurance.Api.Dtos;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Insurance.Tests
{
    public class InsuranceTests : IClassFixture<ControllerTestFixture>
    {
        private readonly ControllerTestFixture _fixture;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public InsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
            _configuration = CreateConfiguration();
            _logger = CreateMockLogger();
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceLessThan500Euros_ShouldAdd500EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 500;

            HomeController homeController = new HomeController(_configuration, _logger);

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

            HomeController homeController = new HomeController(_configuration, _logger);

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

            HomeController homeController = new HomeController(_configuration, _logger);

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

            HomeController homeController = new HomeController(_configuration, _logger);

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

            HomeController homeController = new HomeController(_configuration, _logger);

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

            HomeController homeController = new HomeController(_configuration, _logger);

            float insurance = homeController.CalculateInsuranceAsync(6).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }

        [Fact]
        public void CalculateInsuranceForOder_ShouldAdd4000EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 3000;

            HomeController homeController = new HomeController(_configuration, _logger);

            var order = new OrderDto()
            {
                Id = 1,
                ProductIds = new List<int> { 1, 2, 5 }
            };

            float insurance = homeController.CalculateInsuranceForOrderAsync(order.Id, order).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }

        [Fact]
        public void CalculateInsuranceForOder_InvalidRequest_ShouldReturnMinusOne()
        {
            const float expectedInsuranceValue = -1;

            HomeController homeController = new HomeController(_configuration, _logger);

            var order = new OrderDto()
            {
                Id = 1,
                ProductIds = new List<int> ()
            };

            float insurance = homeController.CalculateInsuranceForOrderAsync(order.Id, order).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }

        [Fact]
        public void CalculateInsurance_GivenInvalidProductId_ShouldReturnMinuesOne()
        {
            const float expectedInsuranceValue = -1;

            HomeController homeController = new HomeController(_configuration, _logger);

            float insurance = homeController.CalculateInsuranceAsync(1000).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }

        [Fact]
        public void CalculateInsurance_GivenInvalidProductTypeId_ShouldReturnMinuesOne()
        {
            const float expectedInsuranceValue = -1;

            HomeController homeController = new HomeController(_configuration, _logger);

            float insurance = homeController.CalculateInsuranceAsync(1000).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }

        [Fact]
        public void CalculateInsuranceForOder_GivenProductTypeIsDigitalCameras_ShouldAdd4000EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 4000;

            HomeController homeController = new HomeController(_configuration, _logger);

            var order = new OrderDto()
            {
                Id = 1,
                ProductIds = new List<int> { 1, 2, 8 }
            };

            float insurance = homeController.CalculateInsuranceForOrderAsync(order.Id, order).Result;

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: insurance
            );
        }

        private ILogger<HomeController> CreateMockLogger()
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            ILoggerFactory factory = serviceProvider.GetService<ILoggerFactory>();

            return factory.CreateLogger<HomeController>();
        }

        private IConfiguration CreateConfiguration()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json")
                .Build();

            return configuration;
        }
    }
}