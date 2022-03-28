using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Api.SalePriceInsureCostCalculators
{
    public interface ISalePriceInsureCostCalculator
    {
        float GetInsureCost();
    }
}
