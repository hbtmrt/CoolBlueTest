using System;

namespace Insurance.Core.CustomExceptions
{
    public sealed class InsuranceServerNotFoundException : Exception
    {
        public InsuranceServerNotFoundException(string message) : base(message)
        {
        }
    }
}