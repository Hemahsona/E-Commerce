using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.DTOs.Baskets
{
    public class BasketDto
    {
        public string Id { get; set; } = default!; // created from frontend side [guid]
        public ICollection<BasketItemDto> Items { get; set; } = [];
    }
}
