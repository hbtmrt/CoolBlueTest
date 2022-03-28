using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Insurance.Api.Dtos;
using Insurance.Core.Enums;
using Newtonsoft.Json;

namespace Insurance.Api
{
    public sealed class BusinessRules
    {
        public async Task<float> CalculateInsuranceAsync(int id, string productApi)
        {
            return await Task.Run(() => {
                InsuranceDto toInsure = new InsuranceDto()
                {
                    ProductId = id
                };

                ProductTypeDto productType = GetProductType(productApi, id);
                if (!productType.HasInsurance)
                {
                    return 0;
                }

                float salePrice = GetSalesPrice(productApi, id);

                float insurance = 0f;

                // get productTypeInsureCost
                // get sale price insure cost
                // return produttype + saleprice;

                if (salePrice < 500)
                    toInsure.InsuranceValue = 500;
                else
                {
                    if (salePrice > 500 && salePrice < 2000)
                        toInsure.InsuranceValue += 1000;
                    if (salePrice >= 2000)
                        toInsure.InsuranceValue += 2000;
                    if (productType.Category == ProductCategory.Laptops ||
                        productType.Category == ProductCategory.Smartphones)
                        toInsure.InsuranceValue += 500;
                }

                return toInsure.InsuranceValue;
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