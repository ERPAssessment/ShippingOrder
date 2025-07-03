using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShippingOrder.Infrastructure.Data.Configuration;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<Domain.Models.ShippingOrder>
{
  public void Configure(EntityTypeBuilder<Domain.Models.ShippingOrder> builder)
  {

    builder.Ignore(orders => orders.DomainEvents);
    builder.Ignore(po => po.TotalPrice);
    builder.Ignore(po => po.TotalPurchaseItemsCount);

    builder.HasKey(po => po.Id);
    builder.Property(po => po.Id).HasConversion(po => po.Value,
                                                dbID => ShippingOrderId.Of(dbID));

    builder.HasMany(po => po.ShippingItems)
      .WithOne()
      .HasForeignKey(po => po.ShippingOrderId);

    builder.ComplexProperty(
            po => po.SHONumber, POBuilder =>
            {
              POBuilder.Property(p => p.Value)
              .HasColumnName("SHONumber")
              .HasMaxLength(200)
              .IsRequired();
            });

    builder.ComplexProperty(
                po => po.PONumber, POBuilder =>
                {
                  POBuilder.Property(p => p.Value)
                  .HasColumnName("PONumber")
                  .HasMaxLength(200)
                  .IsRequired();
                });

    builder.Property(po => po.DocumentState)
                    .HasConversion<string>()
                    .HasMaxLength(30);

    builder.Property(po => po.PalletsCount);
    builder.Property(po => po.DeliveryDate);

  }
}
