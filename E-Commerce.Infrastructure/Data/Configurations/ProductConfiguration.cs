using E_Commerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.Type)
                .WithMany(t => t.Products)
                .HasForeignKey(p => p.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.Name)
                .HasMaxLength(100);
            builder.Property(p => p.Description)
                .HasMaxLength (500);
            builder.Property(p => p.PictureUrl)
                .HasMaxLength(200);
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

        }
    }
}
