using System.Threading.Tasks;
using Insurance.Api.Resources;
using Insurance.Core.CustomExceptions;
using Insurance.Core.Statics;
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
                logger.LogInformation(string.Format(Resource.CalculateInsuranceRequestReceived, id));
                string productApi = configuration.GetValue<string>(Constants.ProductApiText);

                float insureCost = await new BusinessRules().CalculateInsuranceAsync(id, productApi);
                logger.LogInformation(string.Format(Resource.InsureCostText, insureCost));
                return insureCost;
            }
            catch (ProductTypeNotFoundException ex)
            {
                string message = string.Format(Resource.ProductTypeNotFound, id, ex.Message);
                logger.LogError(message);

                return unsuccessfulResult;
            }
            catch (InsuranceServerNotFoundException)
            {
                string message = string.Format(Resource.ApiNotFound);
                logger.LogError(message);

                return unsuccessfulResult;
            }
        }
    }
}