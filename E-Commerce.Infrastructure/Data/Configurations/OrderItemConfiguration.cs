using E_Commerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Price)
                    .HasColumnType("decimal(8,2)");

            builder.OwnsOne(x => x.Product, p =>
            {
                p.Property(x => x.ProductName)
                 .HasMaxLength(100);
                p.Property(x => x.PictureUrl)
                 .HasMaxLength(200);
            });
        }
    }
}
