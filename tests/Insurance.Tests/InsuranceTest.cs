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
        private readonly ILogger<ProductsController> productsLogger;
        private readonly ILogger<OrdersController> ordersLogger;

        public InsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
            _configuration = CreateConfiguration();
            productsLogger = CreateMockLogger<ProductsController>();
            ordersLogger = CreateMockLogger<OrdersController>();
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceLessThan500Euros_ShouldAdd500EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 500;

            ProductsController homeController = new ProductsController(_configuration, productsLogger);

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

            ProductsController homeController = new ProductsController(_configuration, productsLogger);

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

            ProductsController homeController = new ProductsController(_configuration, productsLogger);

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

            ProductsController homeController = new ProductsController(_configuration, productsLogger);

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

            ProductsController homeController = new ProductsController(_configuration, productsLogger);

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

            ProductsController homeController = new ProductsController(_configuration, productsLogger);

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

            OrdersController homeController = new OrdersController(_configuration, ordersLogger);

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

            OrdersController homeController = new OrdersController(_configuration, ordersLogger);

            var order = new OrderDto()
            {
                Id = 1,
                ProductIds = new List<int>()
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

            ProductsController homeController = new ProductsController(_configuration, productsLogger);

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

            ProductsController homeController = new ProductsController(_configuration, productsLogger);

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

            OrdersController homeController = new OrdersController(_configuration, ordersLogger);

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

        private ILogger<T> CreateMockLogger<T>()
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            ILoggerFactory factory = serviceProvider.GetService<ILoggerFactory>();

            return factory.CreateLogger<T>();
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