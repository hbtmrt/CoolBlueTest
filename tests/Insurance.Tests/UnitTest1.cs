using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Insurance.Api.Controllers;
using Insurance.Api.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
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

            var dto = new InsuranceDto
            {
                ProductId = 1,
            };
            var sut = new HomeController(_configuration);

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 1000;

            var dto = new InsuranceDto
            {
                ProductId = 2,
            };
            var sut = new HomeController(_configuration);

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000Euros_ShouldAdd2000EurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 2000;

            var dto = new InsuranceDto
            {
                ProductId = 3,
            };

            var sut = new HomeController(_configuration);

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }
    }

    public class ControllerTestFixture : IDisposable
    {
        private readonly IHost _host;

        public ControllerTestFixture()
        {
            _host = new HostBuilder()
                   .ConfigureWebHostDefaults(
                        b => b.UseUrls("http://localhost:5002")
                              .UseStartup<ControllerTestStartup>()
                    )
                   .Build();

            _host.Start();
        }

        public void Dispose() => _host.Dispose();
    }

    public class ControllerTestStartup
    {
        public List<dynamic> Products => GetProducts();

        private List<dynamic> GetProducts()
        {
            List<dynamic> products = new List<dynamic>
            {
                GenerateSaleBelow500(),
                GenerateSaleBetween500and2000(),
                GenerateSaleAbove2000()
            };

            return products;
        }

        private dynamic GenerateSaleAbove2000()
        {
            return new
            {
                id = 3,
                name = "Test Product above 2000",
                productTypeId = 1,
                salesPrice = 2004
            };
        }

        private dynamic GenerateSaleBetween500and2000()
        {
            return new
            {
                id = 2,
                name = "Test Product between 500 and 2000",
                productTypeId = 1,
                salesPrice = 899
            };
        }

        private dynamic GenerateSaleBelow500()
        {
            return new
            {
                id = 1,
                name = "Test Product below 500",
                productTypeId = 1,
                salesPrice = 366
            };
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(
                ep =>
                {
                    ep.MapGet(
                        "products/{id:int}",
                        context =>
                        {
                            int productId = int.Parse((string)context.Request.RouteValues["id"]);
                            var product = this.Products.FirstOrDefault(p => p.id == productId);
                            return context.Response.WriteAsync(JsonConvert.SerializeObject((object)product));
                        }
                    );
                    ep.MapGet(
                        "product_types",
                        context =>
                        {
                            var productTypes = new[]
                                               {
                                                   new
                                                   {
                                                       id = 1,
                                                       name = "Test type",
                                                       canBeInsured = true
                                                   }
                                               };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productTypes));
                        }
                    );
                }
            );
        }
    }
}