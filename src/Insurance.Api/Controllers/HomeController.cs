using Insurance.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Insurance.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("api/product/{id}/insurance")]
        public InsuranceDto CalculateInsurance(int id)
        {
            InsuranceDto toInsure = new InsuranceDto()
            {
                ProductId = id
            };

            int productId = toInsure.ProductId;
            string productApi = configuration.GetValue<string>("ProductApi");

            BusinessRules.GetProductType(productApi, productId, ref toInsure);
            BusinessRules.GetSalesPrice(productApi, productId, ref toInsure);

            float insurance = 0f;

            if (toInsure.SalesPrice < 500)
                toInsure.InsuranceValue = 500;
            else
            {
                if (toInsure.SalesPrice > 500 && toInsure.SalesPrice < 2000)
                    if (toInsure.ProductTypeHasInsurance)
                        toInsure.InsuranceValue += 1000;
                if (toInsure.SalesPrice >= 2000)
                    if (toInsure.ProductTypeHasInsurance)
                        toInsure.InsuranceValue += 2000;
                if (toInsure.ProductTypeName == "Laptops" || toInsure.ProductTypeName == "Smartphones" && toInsure.ProductTypeHasInsurance)
                    toInsure.InsuranceValue += 500;
            }

            return toInsure;
        }
    }
}