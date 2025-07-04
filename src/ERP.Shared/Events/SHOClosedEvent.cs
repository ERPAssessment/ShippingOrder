namespace ERP.Shared.Events;

public record SHOClosedEvent : IntegrationEvent
{
  public string PurchaseOrderNumber { get; set; } = default!;
}
