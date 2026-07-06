using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Baskets
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
