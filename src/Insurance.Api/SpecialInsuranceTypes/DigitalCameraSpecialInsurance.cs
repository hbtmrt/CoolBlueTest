using Insurance.Core.Statics;

namespace Insurance.Api.SpecialInsuranceTypes
{
    public class DigitalCameraSpecialInsurance : ISpecialInsuranceType
    {
        public float GetInsurance()
        {
            return Constants.InsureCost.SpecialInsurance.DigitalCameras;
        }
    }
}