using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Entities
{
    public abstract class BaseEntity<TKey>
    {
        public TKey id { get; set; } = default!;
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
