using System;

namespace Insurance.Core.CustomExceptions
{
    public sealed class ProductTypeNotFoundException : Exception
    {
        public ProductTypeNotFoundException()
        {
        }

        public ProductTypeNotFoundException(string message) : base(message)
        {
        }
    }
}