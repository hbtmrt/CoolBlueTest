using System;
using System.Threading.Tasks;
using Insurance.Api.Dtos;
using Insurance.Api.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private readonly ILogger<ProductTypesController> logger;

        public ProductTypesController(
           ILogger<ProductTypesController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductTypeDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProductTypeAsync([FromBody] AddProductTypeRequest request)
        {
            logger.LogInformation(string.Format(Resource.AddProductTypeRequestReceived));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                ProductTypeDto productType = await new BusinessRules().AddProductTypeAsync(request);
                logger.LogInformation(string.Format(Resource.ProductTypeAddedSuccessfully), request);

                return Ok(productType);
            }
            catch (Exception ex)
            {
                logger.LogInformation(string.Format(Resource.ErrorOnAddingProductType), ex);
                return BadRequest(ex);
            }
        }
    }
}