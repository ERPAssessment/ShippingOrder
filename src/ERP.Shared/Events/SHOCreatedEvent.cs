using MediatR;

namespace ERP.Shared.Events;

public record SHOCreatedEvent : IntegrationEvent, INotification
{
  public string PurchaseOrderNumber { get; set; } = default!;
}
