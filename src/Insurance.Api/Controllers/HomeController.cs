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
        public float CalculateInsurance(int id)
        {
            string productApi = configuration.GetValue<string>("ProductApi");
            return new BusinessRules().CalculateInsurance(id, productApi);
        }
    }
}