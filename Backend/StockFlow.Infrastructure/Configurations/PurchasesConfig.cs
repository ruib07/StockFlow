using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Configurations;

public class PurchasesConfig : IEntityTypeConfiguration<Purchases>
{
    public void Configure(EntityTypeBuilder<Purchases> builder)
    {
        builder.ToTable("Purchases");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(p => p.SupplierId).IsRequired();
        builder.Property(p => p.PurchaseDate).IsRequired();
        builder.Property(p => p.Total).IsRequired().HasColumnType("decimal(18, 2)");

        builder.HasOne(p => p.Supplier)
               .WithMany()
               .HasForeignKey(p => p.SupplierId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
