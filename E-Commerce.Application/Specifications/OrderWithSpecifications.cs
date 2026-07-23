using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal class OrderWithSpecifications : BaseSpecifications<Order, Guid>
    {
        public OrderWithSpecifications(string email) : base(x => x.BuyerEmail == email)
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.Items);
            AddOrderByDesc(o => o.OrderDate);
        }
        public OrderWithSpecifications(Guid id,string email) : base(x => x.BuyerEmail == email && x.Id == id)
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.Items);
            AddOrderByDesc(o => o.OrderDate);
        }
    }
}
