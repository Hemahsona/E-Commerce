using E_Commerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    internal class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(x => x.Cost)
                    .HasColumnType("decimal(8,2)");

            builder.Property(x => x.ShortName)
                    .HasMaxLength(50);

            builder.Property(x => x.Description)
                    .HasMaxLength (50);

            builder.Property(x => x.DeliveryTime)
                    .HasMaxLength(50);
        }
    }
}
