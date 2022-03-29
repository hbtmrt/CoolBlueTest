using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Api.Dtos
{
    public sealed class OrderDto
    {
        public int Id { get; set; }

        [Required]
        public List<int> ProductIds { get; set; }
    }
}