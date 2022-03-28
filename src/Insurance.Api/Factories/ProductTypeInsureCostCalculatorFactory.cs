using Insurance.Api.ProductTypeInsureCostCalculators;
using Insurance.Core.Enums;

namespace Insurance.Api.Factories
{
    public sealed class ProductTypeInsureCostCalculatorFactory
    {
        public IProductTypeInsureCostCalculator Create(ProductCategory category)
        {
            return category switch
            {
                ProductCategory.Laptops => new LaptopIsureCostCalculator(),
                ProductCategory.Smartphones => new SmartphoneInsureCostCalculator(),
                _ => new OtherProductTypeInsureCostCalculator()
            };
        }
    }
}