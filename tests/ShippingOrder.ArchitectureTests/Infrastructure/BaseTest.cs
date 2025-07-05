using ShippingOrder.Application.ShippingOrder.Commands.CloseShippingOrder;
using ShippingOrder.Domain.Abstractions;
using ShippingOrder.Infrastructure.Data;
using System.Reflection;

namespace ShippingOrder.ArchitectureTests.Infrastructure;

public class BaseTest
{
  protected static readonly Assembly ApplicationAssembly = typeof(CloseShippingOrderCommand).Assembly;

  protected static readonly Assembly DomainAssembly = typeof(IEntity).Assembly;

  protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;

  protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}