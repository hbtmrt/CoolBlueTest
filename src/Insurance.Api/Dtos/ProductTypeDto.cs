using Insurance.Core.Enums;
using Insurance.Core.Statics;
using Newtonsoft.Json;

namespace Insurance.Api.Dtos
{
    public sealed class ProductTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonProperty("canBeInsured")]
        public bool HasInsurance { get; set; }

        public float Surcharge { get; set; }

        [JsonIgnore]
        public ProductCategory Category => GetProductCategory();

        private ProductCategory GetProductCategory()
        {
            // I feel like bad about following since if a new type is added, this file has to be touch and modified.
            // but since the change is minimal, I will keep this for the moment.

            return Name switch
            {
                Constants.ProductTypeName.Laptops => ProductCategory.Laptops,
                Constants.ProductTypeName.Smartphones => ProductCategory.Smartphones,
                Constants.ProductTypeName.DigitalCameras => ProductCategory.DigitalCameras,
                _ => ProductCategory.Other
            };
        }
    }
}