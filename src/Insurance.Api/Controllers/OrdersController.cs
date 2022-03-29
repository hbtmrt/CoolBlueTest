using System.Linq;
using System.Threading.Tasks;
using Insurance.Api.Dtos;
using Insurance.Api.Resources;
using Insurance.Core.CustomExceptions;
using Insurance.Core.Statics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<OrdersController> logger;
        private const float unsuccessfulResult = -1;

        public OrdersController(
           IConfiguration configuration,
           ILogger<OrdersController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpPost("{id}/insurance")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(float))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<float> CalculateInsuranceForOrderAsync(int id, [FromBody] OrderDto order)
        {
            if (!ModelState.IsValid || order.ProductIds == null || !order.ProductIds.Any())
            {
                return unsuccessfulResult;
            }

            try
            {
                logger.LogInformation(string.Format(Resource.CalculateInsuranceRequestForOrderReceived, id));
                string productApi = configuration.GetValue<string>(Constants.ProductApiText);

                float insureCost = await new BusinessRules().CalculateInsuranceForOrderAsync(order, productApi);
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