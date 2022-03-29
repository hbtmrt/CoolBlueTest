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
        private readonly HashSet<ProductTypeDto> productTypes = new HashSet<ProductTypeDto>();
        private object syncObject = new object();

        public BusinessRules()
        {
            productTypes.Clear();
        }

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

        public async Task<float> CalculateInsuranceForOrderAsync(OrderDto order, string productApi)
        {
            List<Task<float>> tasks = new List<Task<float>>();

            order.ProductIds.ForEach(productId =>
            {
                tasks.Add(CalculateInsuranceAsync(productId, productApi));
            });

            float[] productInsuranceList = await Task.WhenAll(tasks);
            return productInsuranceList.Sum();
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
                LoadProductTypes(httpClientWrapper);
                var product = httpClientWrapper.Get<dynamic>(string.Format(Constants.ApiPath.Product, productID));

                if (product == null)
                {
                    throw new ProductNotFoundException();
                }

                int productTypeId = product.productTypeId;
                try
                {
                    ProductTypeDto productType = productTypes.Single(c => c.Id == productTypeId && c.HasInsurance);

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

        private void LoadProductTypes(HttpClientWrapper httpClientWrapper)
        {
            lock (syncObject)
            {
                if (productTypes.Count == 0)
                {
                    List<ProductTypeDto> collection = httpClientWrapper.Get<List<ProductTypeDto>>(Constants.ApiPath.ProductType);
                    collection.ForEach(c =>
                    {
                        lock (syncObject)
                        {
                            productTypes.Add(c);
                        }
                    });
                }
            }
        }
    }
}