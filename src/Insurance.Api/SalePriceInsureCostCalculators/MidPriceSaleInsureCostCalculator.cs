using Insurance.Core.Statics;

namespace Insurance.Api.SalePriceInsureCostCalculators
{
    public sealed class MidPriceSaleInsureCostCalculator : ISalePriceInsureCostCalculator
    {
        public float GetInsureCost()
        {
            return Constants.InsureCost.SalePrice.Mid;
        }
    }
}