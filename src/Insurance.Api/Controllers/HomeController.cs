using System;
using System.Net.Http;
using System.Threading.Tasks;
using Insurance.Core.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<HomeController> logger;
        private const float unsuccessfulResult = -1;

        public HomeController(
            IConfiguration configuration,
            ILogger<HomeController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpGet]
        [Route("api/product/{id}/insurance")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(float))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<float> CalculateInsuranceAsync(int id)
        {
            try
            {
                logger.LogInformation($"Received request to calculate insurance for the product id: {id}");
                string productApi = configuration.GetValue<string>("ProductApi");

                float insureCost = await new BusinessRules().CalculateInsuranceAsync(id, productApi);
                logger.LogInformation($"The insure cost is : {insureCost}");
                return insureCost;
            }
            catch (ProductTypeNotFoundException ex)
            {
                string message = $"The product type cannot be found for the product id: {id}, for more information:{ex.Message}";
                logger.LogError(message);

                return unsuccessfulResult;
            }
            catch (InsuranceServerNotFoundException)
            {
                string message = $"The insurance api cannot be found.";
                logger.LogError(message);

                return unsuccessfulResult;
            }
        }
    }
}