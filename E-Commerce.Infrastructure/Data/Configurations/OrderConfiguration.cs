using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Infrastructure.Identity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(o => o.Items)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.SubTotal)
                    .HasColumnType("decimal(8,2)");

            builder.OwnsOne(o => o.ShipToAddress, address =>
            {
                address.Property(a => a.FirstName).HasMaxLength(180);
                address.Property(a => a.LastName).HasMaxLength(180);
                address.Property(a => a.City).HasMaxLength(100);
                address.Property(a => a.Street).HasMaxLength(100);
                address.Property(a => a.Country).HasMaxLength(20);
            });

            builder.Property(x => x.Status)
                    .HasConversion<string>()
                    .HasMaxLength(50);

        }
    }
}
