using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Baskets
{
    public class CustomerBasket
    {
        public string Id { get; set; } = default!; // created from frontend side [guid]
        public ICollection<BasketItem> Items { get; set; } = [];
        public string? ClientSecret { get; set; }
        public string? PaymentIntent { get; set; }
        public int? DeliveryMethod { get; set; }
        public decimal? ShippingPrice { get; set; }


    }
}
