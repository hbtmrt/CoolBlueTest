using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Insurance.Tests
{
    public class ControllerTestStartup
    {
        public List<dynamic> Products => GetProducts();

        private List<dynamic> GetProducts()
        {
            List<dynamic> products = new List<dynamic>
            {
                GenerateSaleBelow500(),
                GenerateSaleBetween500and2000(),
                GenerateSaleAbove2000(),
                GenerateSaleBelow500AndTypeOfLaptop(),
                GenerateSaleBetween500and2000AndTypeOfSmartphone(),
                GenerateSaleAbove2000AndTypeOfLaptop(),
                GenerateWithInvalidProductType(),
            };

            return products;
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

        private dynamic GenerateSaleBelow500AndTypeOfLaptop()
        {
            return new
            {
                id = 4,
                name = "Test Product below 500 and type of laptop",
                productTypeId = 2,
                salesPrice = 400
            };
        }

        private dynamic GenerateSaleBetween500and2000AndTypeOfSmartphone()
        {
            return new
            {
                id = 5,
                name = "Test Product between 500 and 2000 and type of smartphone",
                productTypeId = 3,
                salesPrice = 1345
            };
        }

        private dynamic GenerateSaleAbove2000AndTypeOfLaptop()
        {
            return new
            {
                id = 6,
                name = "Test Product above 2000 and type of laptop",
                productTypeId = 2,
                salesPrice = 4456
            };
        }

        private dynamic GenerateWithInvalidProductType()
        {
            return new
            {
                id = 7,
                name = "Test Product with invalid product type",
                productTypeId = 1000,
                salesPrice = 4456
            };
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net("log4net.config");
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
                                                   },
                                                   new
                                                   {
                                                       id = 2,
                                                       name = "Laptops",
                                                       canBeInsured = true
                                                   },
                                                   new
                                                   {
                                                       id = 3,
                                                       name = "Smartphones",
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