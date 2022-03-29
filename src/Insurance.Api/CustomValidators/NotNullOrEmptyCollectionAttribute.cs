using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Api.CustomValidators
{
    public sealed class NotNullOrEmptyCollectionAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is ICollection collection)
            {
                return collection.Count != 0;
            }

            return value is IEnumerable enumerable && enumerable.GetEnumerator().MoveNext();
        }
    }
}