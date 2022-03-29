using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Insurance.Api.CustomValidators;

namespace Insurance.Api.Dtos
{
    public sealed class OrderDto
    {
        public int Id { get; set; }

        [Required]
        [NotNullOrEmptyCollection]
        public List<int> ProductIds { get; set; }
    }
}