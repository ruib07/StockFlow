using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Configurations;

public class AdministratorsConfig : IEntityTypeConfiguration<Administrators>
{
    public void Configure(EntityTypeBuilder<Administrators> builder)
    {
        builder.ToTable("Administrators");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(60);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(60);
        builder.Property(p => p.Password).IsRequired().HasMaxLength(100);
    }
}
