using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.DTOs.Orders
{
    public class OrderTOReturnDto
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public DateTimeOffset dateTime { get; set; }
        public AddressDto ShipToAddress { get; set; } = default!;
        public string DeliveryMethod { get; set; } = default!;
        public string Status { get; set; } = default!;
        public Decimal SubTotal { get; set; }
        public Decimal Total { get; set; }
        public Decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = [];

    }
}
