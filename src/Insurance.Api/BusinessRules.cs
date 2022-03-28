using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Insurance.Api.Dtos;
using Insurance.Api.Factories;
using Insurance.Api.ProductTypeInsureCostCalculators;
using Insurance.Api.SalePriceInsureCostCalculators;
using Newtonsoft.Json;

namespace Insurance.Api
{
    public sealed class BusinessRules
    {
        public async Task<float> CalculateInsuranceAsync(int id, string productApi)
        {
            return await Task.Run(() =>
            {
                InsuranceDto toInsure = new InsuranceDto()
                {
                    ProductId = id
                };

                ProductTypeDto productType = GetProductType(productApi, id);
                if (!productType.HasInsurance)
                {
                    return 0;
                }

                IProductTypeInsureCostCalculator productTypeInsureCostCalculator = new ProductTypeInsureCostCalculatorFactory().Create(productType.Category);
                float productTypeInsureCost = productTypeInsureCostCalculator.GetInsureCost();

                float salePrice = GetSalesPrice(productApi, id);
                ISalePriceInsureCostCalculator salePriceInsureCostCalculator = new SalePriceInsureCostCalculatorFactory().Create(salePrice);
                float salePriceInsureCost = salePriceInsureCostCalculator.GetInsureCost();

                return productTypeInsureCost + salePriceInsureCost;
            });
        }

        private float GetSalesPrice(string baseAddress, int productID)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) };
            string json = client.GetAsync(string.Format("/products/{0:G}", productID)).Result.Content.ReadAsStringAsync().Result;
            var product = JsonConvert.DeserializeObject<dynamic>(json);

            return product.salesPrice;
        }

        private ProductTypeDto GetProductType(string baseAddress, int productID)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) };
            string json = client.GetAsync("/product_types").Result.Content.ReadAsStringAsync().Result;
            var collection = JsonConvert.DeserializeObject<List<ProductTypeDto>>(json);

            json = client.GetAsync(string.Format("/products/{0:G}", productID)).Result.Content.ReadAsStringAsync().Result;
            var product = JsonConvert.DeserializeObject<dynamic>(json);

            int productTypeId = product.productTypeId;

            ProductTypeDto productType = collection.FirstOrDefault(c => c.Id == productTypeId && c.HasInsurance);

            return productType;
        }
    }
}