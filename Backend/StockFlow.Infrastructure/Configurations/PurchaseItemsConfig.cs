using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Configurations;

public class PurchaseItemsConfig : IEntityTypeConfiguration<PurchaseItems>
{
    public void Configure(EntityTypeBuilder<PurchaseItems> builder)
    {
        builder.ToTable("PurchaseItems");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(p => p.PurchaseId).IsRequired();
        builder.Property(p => p.ProductId).IsRequired();
        builder.Property(p => p.Quantity).IsRequired();
        builder.Property(p => p.UnitPrice).IsRequired().HasColumnType("decimal(18, 2)");
        builder.Property(p => p.SubTotal).IsRequired().HasColumnType("decimal(18, 2)");

        builder.HasOne(p => p.Purchase)
               .WithMany()
               .HasForeignKey(p => p.PurchaseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Product)
               .WithMany()
               .HasForeignKey(p => p.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
