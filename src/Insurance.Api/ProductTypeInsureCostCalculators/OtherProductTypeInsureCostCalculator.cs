using Insurance.Core.Statics;

namespace Insurance.Api.ProductTypeInsureCostCalculators
{
    public sealed class OtherProductTypeInsureCostCalculator : IProductTypeInsureCostCalculator
    {
        public float GetInsureCost()
        {
            return Constants.InsureCost.ProductType.Other;
        }
    }
}