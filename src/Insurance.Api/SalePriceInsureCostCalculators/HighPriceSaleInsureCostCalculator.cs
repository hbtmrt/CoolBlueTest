using Insurance.Core.Statics;

namespace Insurance.Api.SalePriceInsureCostCalculators
{
    public class HighPriceSaleInsureCostCalculator : ISalePriceInsureCostCalculator
    {
        public float GetInsureCost()
        {
            return Constants.InsureCost.SalePrice.High;
        }
    }
}