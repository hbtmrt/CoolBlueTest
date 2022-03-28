using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.Api.Dtos;
using Insurance.Api.Factories;
using Insurance.Api.ProductTypeInsureCostCalculators;
using Insurance.Api.SalePriceInsureCostCalculators;
using Insurance.Api.Wrappers;

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

                HttpClientWrapper httpClientWrapper = new HttpClientWrapper(new Uri(productApi));

                ProductTypeDto productType = GetProductType(httpClientWrapper, id);
                if (!productType.HasInsurance)
                {
                    return 0;
                }

                IProductTypeInsureCostCalculator productTypeInsureCostCalculator = new ProductTypeInsureCostCalculatorFactory().Create(productType.Category);
                float productTypeInsureCost = productTypeInsureCostCalculator.GetInsureCost();

                float salePrice = GetSalesPrice(httpClientWrapper, id);
                ISalePriceInsureCostCalculator salePriceInsureCostCalculator = new SalePriceInsureCostCalculatorFactory().Create(salePrice);
                float salePriceInsureCost = salePriceInsureCostCalculator.GetInsureCost();

                return productTypeInsureCost + salePriceInsureCost;
            });
        }

        private float GetSalesPrice(HttpClientWrapper httpClientWrapper, int productID)
        {
            var product = httpClientWrapper.Get<dynamic>(string.Format("/products/{0:G}", productID));
            return product.salesPrice;
        }

        private ProductTypeDto GetProductType(HttpClientWrapper httpClientWrapper, int productID)
        {
            List<ProductTypeDto> collection = httpClientWrapper.Get<List<ProductTypeDto>>("/product_types");
            var product = httpClientWrapper.Get<dynamic>(string.Format("/products/{0:G}", productID));

            int productTypeId = product.productTypeId;
            ProductTypeDto productType = collection.FirstOrDefault(c => c.Id == productTypeId && c.HasInsurance);

            return productType;
        }
    }
}