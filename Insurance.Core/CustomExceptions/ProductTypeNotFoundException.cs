using System;
using System.Collections.Generic;
using System.Text;

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
