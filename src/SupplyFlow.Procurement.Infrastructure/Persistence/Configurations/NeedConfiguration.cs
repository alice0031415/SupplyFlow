using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplyFlow.Procurement.Domain.Needs;

namespace SupplyFlow.Procurement.Infrastructure.Persistence.Configurations;

public class NeedConfiguration : IEntityTypeConfiguration<Need>
{
    public void Configure(EntityTypeBuilder<Need> builder)
    {
        builder.ToTable("needs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Quantity)
            .HasPrecision(18, 3);

        builder.Property(x => x.RequiredBy)
            .HasColumnType("date");

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(x => x.Version)
            .IsRowVersion();

        builder.HasIndex(x => new { x.Status, x.RequiredBy });
    }
}