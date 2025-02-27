using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockFlow.Domain.Entities;

namespace StockFlow.Infrastructure.Configurations;

public class PasswordResetTokenConfig : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("PasswordResetTokens");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(p => p.AdminId).IsRequired();
        builder.Property(p => p.Token).IsRequired();
        builder.Property(p => p.ExpiryDate).IsRequired();

        builder.HasOne(p => p.Administrator)
               .WithMany()
               .HasForeignKey(p => p.AdminId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
