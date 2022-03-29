using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.Api.Dtos;
using Insurance.Api.Factories;
using Insurance.Api.ProductTypeInsureCostCalculators;
using Insurance.Api.SalePriceInsureCostCalculators;
using Insurance.Api.SpecialInsuranceTypes;
using Insurance.Api.Wrappers;
using Insurance.Core.CustomExceptions;
using Insurance.Core.Enums;
using Insurance.Core.Statics;

namespace Insurance.Api
{
    public sealed class BusinessRules
    {
        private readonly HashSet<ProductTypeDto> productTypes = new HashSet<ProductTypeDto>();
        private readonly object syncObject = new object();
        public HashSet<ISpecialInsuranceType> SpecialInsurances { get; set; } = new HashSet<ISpecialInsuranceType>();

        public BusinessRules()
        {
            productTypes.Clear();
            SpecialInsurances.Clear();
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

                CheckSpecialInsurance(productType);

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
            productTypes.Clear();
            SpecialInsurances.Clear();

            List<Task<float>> tasks = new List<Task<float>>();

            order.ProductIds.ForEach(productId =>
            {
                tasks.Add(CalculateInsuranceAsync(productId, productApi));
            });

            float[] productInsuranceList = await Task.WhenAll(tasks);
            return productInsuranceList.Sum() + CalculateSpecialInsurances();
        }

        private float CalculateSpecialInsurances()
        {
            float insurance = 0;

            foreach (ISpecialInsuranceType item in SpecialInsurances)
            {
                insurance += item.GetInsurance();
            }

            return insurance;
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
                LoadProductTypesIfNotLoaded(httpClientWrapper);
                var product = httpClientWrapper.Get<dynamic>(string.Format(Constants.ApiPath.Product, productID));

                if (product == null)
                {
                    throw new ProductNotFoundException();
                }

                int productTypeId = product.productTypeId;

                ProductTypeDto productType = productTypes.FirstOrDefault(c => c.Id == productTypeId && c.HasInsurance);

                if (productType == null)
                {
                    throw new ProductTypeNotFoundException();
                }

                return productType;
            }
            catch (AggregateException ex)
            {
                throw new InsuranceServerNotFoundException(ex.Message);
            }
        }

        private void LoadProductTypesIfNotLoaded(HttpClientWrapper httpClientWrapper)
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

        private void CheckSpecialInsurance(ProductTypeDto productType)
        {
            if (productType.Category == ProductCategory.DigitalCameras)
            {
                SpecialInsurances.Add(new DigitalCameraSpecialInsurance());
            }
        }
    }
}