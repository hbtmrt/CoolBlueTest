using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<HomeController> logger;

        public HomeController(
            IConfiguration configuration,
            ILogger<HomeController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpGet]
        [Route("api/product/{id}/insurance")]
        public async Task<float> CalculateInsuranceAsync(int id)
        {
            logger.LogInformation($"Received request to calculate insurance for the product id: {id}");
            string productApi = configuration.GetValue<string>("ProductApi");

            float insureCost = await new BusinessRules().CalculateInsuranceAsync(id, productApi);
            logger.LogInformation($"The insure cost is : {insureCost}");

            return insureCost;
        }
    }
}