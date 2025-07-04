namespace ERP.Shared.Events;

public record SHOCreatedEvent : IntegrationEvent
{
  public string PurchaseOrderNumber { get; set; } = default!;
}
