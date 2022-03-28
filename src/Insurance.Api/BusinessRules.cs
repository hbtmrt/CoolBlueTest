using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.Api.Dtos;
using Insurance.Api.Factories;
using Insurance.Api.ProductTypeInsureCostCalculators;
using Insurance.Api.SalePriceInsureCostCalculators;
using Insurance.Api.Wrappers;
using Insurance.Core.CustomExceptions;
using Insurance.Core.Statics;

namespace Insurance.Api
{
    public sealed class BusinessRules
    {
        public async Task<float> CalculateInsuranceAsync(int id, string productApi)
        {
            return await Task.Run(() =>
            {
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
            try
            {
                var product = httpClientWrapper.Get<dynamic>(string.Format(Constants.ApiPath.Product, productID));
                return product.salesPrice;
            }
            catch (AggregateException ex)
            {
                throw new InsuranceServerNotFoundException(ex.Message);
            }
        }

        private ProductTypeDto GetProductType(HttpClientWrapper httpClientWrapper, int productID)
        {
            try
            {
                List<ProductTypeDto> collection = httpClientWrapper.Get<List<ProductTypeDto>>(Constants.ApiPath.ProductType);
                var product = httpClientWrapper.Get<dynamic>(string.Format(Constants.ApiPath.Product, productID));

                int productTypeId = product.productTypeId;
                try
                {
                    ProductTypeDto productType = collection.Single(c => c.Id == productTypeId && c.HasInsurance);

                    return productType;
                }
                catch (Exception ex)
                {
                    throw new ProductTypeNotFoundException(ex.Message);
                }
            }
            catch (AggregateException ex)
            {
                throw new InsuranceServerNotFoundException(ex.Message);
            }
        }
    }
}