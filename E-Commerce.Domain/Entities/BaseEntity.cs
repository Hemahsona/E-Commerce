using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities
{
    public abstract class BaseEntity<TKey>
    {
        public TKey id { get; set; } = default!;
    }
}
