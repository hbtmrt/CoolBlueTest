using Insurance.Api.SalePriceInsureCostCalculators;
using Insurance.Core.Statics;

namespace Insurance.Api.Factories
{
    public sealed class SalePriceInsureCostCalculatorFactory
    {
        public ISalePriceInsureCostCalculator Create(float salePrice)
        {
            if (salePrice < Constants.SalePriceThresholds.Lower)
            {
                return new LowerPriceSaleInsureCostCalculator();
            }

            if (salePrice >= Constants.SalePriceThresholds.Lower && salePrice < Constants.SalePriceThresholds.High)
            {
                return new MidPriceSaleInsureCostCalculator();
            }

            return new HighPriceSaleInsureCostCalculator();
        }
    }
}