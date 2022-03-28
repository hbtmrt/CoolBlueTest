using Insurance.Core.Statics;

namespace Insurance.Api.SalePriceInsureCostCalculators
{
    public sealed class LowerPriceSaleInsureCostCalculator : ISalePriceInsureCostCalculator
    {
        public float GetInsureCost()
        {
            return Constants.InsureCost.SalePrice.Lower;
        }
    }
}