using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities.Products
{
    public class ProductType :BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public ICollection<Product> Products { get; set; } = [];
        public int ProductId { get; set; }

    }
}
