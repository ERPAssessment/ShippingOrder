using MediatR;

namespace ERP.Shared.Events;

public record SHOClosedEvent : IntegrationEvent, INotification
{
  public string PurchaseOrderNumber { get; set; } = default!;
}
