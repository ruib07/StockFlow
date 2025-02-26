using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Configurations;

public class CustomersConfig : IEntityTypeConfiguration<Customers>
{
    public void Configure(EntityTypeBuilder<Customers> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(60);
        builder.Property(p => p.NIF).IsRequired().HasMaxLength(10);
        builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(10);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(60);
        builder.Property(p => p.Address).IsRequired().HasMaxLength(300);
    }
}
