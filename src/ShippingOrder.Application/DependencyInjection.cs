﻿using ERP.Shared.Behaviors;
using ERP.Shared.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace ShippingOrder.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplicationServices
      (this IServiceCollection services, IConfiguration configuration)
  {
    services.AddMediatR(config =>
    {
      config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
      config.AddOpenBehavior(typeof(ValidationBehavior<,>));
      config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

    services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

    return services;
  }
}
