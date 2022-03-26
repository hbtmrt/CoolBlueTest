using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Insurance.Api.Controllers;
using Newtonsoft.Json;
using Insurance.Api.Dtos;

namespace Insurance.Api
{
    public class BusinessRules
    {
        public void GetProductType(string baseAddress, int productID, ref InsuranceDto insurance)
        {
            HttpClient client = new HttpClient{ BaseAddress = new Uri(baseAddress)};
            string json = client.GetAsync("/product_types").Result.Content.ReadAsStringAsync().Result;
            var collection = JsonConvert.DeserializeObject<dynamic>(json);

            json = client.GetAsync(string.Format("/products/{0:G}", productID)).Result.Content.ReadAsStringAsync().Result;
            var product = JsonConvert.DeserializeObject<dynamic>(json);

            int productTypeId = product.productTypeId;
            string productTypeName = null;
            bool hasInsurance = false;

            insurance = new InsuranceDto();

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].id == productTypeId && collection[i].canBeInsured == true)
                {
                    insurance.ProductTypeName = collection[i].name;
                    insurance.ProductTypeHasInsurance = true;
                }
            }
        }

        public float CalculateInsurance(int id, string productApi)
        {
            InsuranceDto toInsure = new InsuranceDto()
            {
                ProductId = id
            };

            GetProductType(productApi, id, ref toInsure);
            GetSalesPrice(productApi, id, ref toInsure);

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

            return toInsure.InsuranceValue;
        }

        public void GetSalesPrice(string baseAddress, int productID, ref InsuranceDto insurance)
        {
            HttpClient client = new HttpClient{ BaseAddress = new Uri(baseAddress)};
            string json = client.GetAsync(string.Format("/products/{0:G}", productID)).Result.Content.ReadAsStringAsync().Result;
            var product = JsonConvert.DeserializeObject<dynamic>(json);

            insurance.SalesPrice = product.salesPrice;
        }
    }
}