using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShippingOrder.Infrastructure.Data.Configuration;

public class ShippingItemConfiguration : IEntityTypeConfiguration<ShippingItem>
{
  public void Configure(EntityTypeBuilder<ShippingItem> builder)
  {
    builder.HasKey(po => po.Id);
    builder.Property(po => po.Id).HasConversion(po => po.Value,
                                                dbID => ShippingItemId.Of(dbID));

    builder.Property(pi => pi.ShippingOrderId)
           .HasConversion(po => po.Value, dbID => ShippingOrderId.Of(dbID));

    builder.Property(pi => pi.PurchaseGoodCode)
       .HasConversion(po => po.Code, dbID => PurchaseGoodCode.Of(dbID));

    builder.Property(po => po.Price).IsRequired();
    builder.Property(po => po.Price).HasConversion(po => po.Amount,
                                            dbID => Money.Of(dbID));
  }
}
