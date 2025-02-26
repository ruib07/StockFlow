using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Configurations;

public class SaleItemsConfig : IEntityTypeConfiguration<SaleItems>
{
    public void Configure(EntityTypeBuilder<SaleItems> builder)
    {
        builder.ToTable("SaleItems");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(p => p.SaleId).IsRequired();
        builder.Property(p => p.ProductId).IsRequired();
        builder.Property(p => p.Quantity).IsRequired();
        builder.Property(p => p.UnitPrice).IsRequired().HasColumnType("decimal(18, 2)");
        builder.Property(p => p.SubTotal).IsRequired().HasColumnType("decimal(18, 2)");

        builder.HasOne(p => p.Sale)
               .WithMany()
               .HasForeignKey(p => p.SaleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Product)
               .WithMany()
               .HasForeignKey(p => p.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
