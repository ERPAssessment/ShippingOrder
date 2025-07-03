using System.Text.Json.Serialization;

namespace ShippingOrder.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ShippingOrderState
{
  Draft = 1,
  Created = 2,
  Closed = 3
}