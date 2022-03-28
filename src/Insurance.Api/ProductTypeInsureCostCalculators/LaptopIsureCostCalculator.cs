using Insurance.Core.Statics;

namespace Insurance.Api.ProductTypeInsureCostCalculators
{
    public sealed class LaptopIsureCostCalculator : IProductTypeInsureCostCalculator
    {
        public float GetInsureCost()
        {
            return Constants.InsureCost.ProductType.Laptops;
        }
    }
}