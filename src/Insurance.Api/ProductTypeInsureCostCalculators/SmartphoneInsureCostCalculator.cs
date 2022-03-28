using Insurance.Core.Statics;

namespace Insurance.Api.ProductTypeInsureCostCalculators
{
    public sealed class SmartphoneInsureCostCalculator : IProductTypeInsureCostCalculator
    {
        public float GetInsureCost()
        {
            return Constants.InsureCost.ProductType.Smartphones;
        }
    }
}