namespace Insurance.Core.Statics
{
    public static class Constants
    {
        public const string ProductApiText = "ProductApi";
        public const string SurchargeProductTypesStorageFilePath = "ProductTypeSurcharge.json";

        public static class ApiPath
        {
            public const string ProductType = "/product_types";
            public const string Product = "/products/{0:G}";
        }

        public static class ProductTypeName
        {
            public const string Laptops = "Laptops";
            public const string Smartphones = "Smartphones";
            public const string DigitalCameras = "DigitalCameras";
        }

        public static class InsureCost
        {
            public static class ProductType
            {
                public const float Laptops = 500;
                public const float Smartphones = 500;
                public const float Other = 0;
            }

            public static class SalePrice
            {
                public const float Lower = 500;
                public const float Mid = 1000;
                public const float High = 2000;
            }

            public static class SpecialInsurance
            {
                public const float DigitalCameras = 500;
            }
        }

        public static class SalePriceThresholds
        {
            public const float Lower = 500;
            public const float High = 2000;
        }
    }
}