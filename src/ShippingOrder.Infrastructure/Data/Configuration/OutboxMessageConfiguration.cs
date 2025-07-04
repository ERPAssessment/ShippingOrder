using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingOrder.Infrastructure.Data.Constants;

namespace ShippingOrder.Infrastructure.Data.Configuration;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
  public void Configure(EntityTypeBuilder<OutboxMessage> builder)
  {
    builder.ToTable(TableNames.OutboxMessages);
    builder.HasKey(x => x.Id);
  }
}
