using System.Text.Json.Serialization;

namespace Insurance.Api.Dtos
{
    public sealed class InsuranceDto
    {
        public InsuranceDto()
        {
            ProductType = new ProductTypeDto();
        }

        public int ProductId { get; set; }
        public float InsuranceValue { get; set; }

        [JsonIgnore]
        public ProductTypeDto ProductType { get; set; }

        [JsonIgnore]
        public float SalesPrice { get; set; }
    }
}