using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.DTOs.Orders
{
    public class OrderItemDto
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string ProductUrl { get; set; } = default!;
    }
}
