using Carter;
using ERP.Shared.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace ShippingOrder.API;

public static class DependencyInjection
{
  public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddCarter();

    services.AddExceptionHandler<CustomExceptionHandler>();
    services.AddHealthChecks()
        .AddSqlServer(configuration.GetConnectionString("Database")!);

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Debug()
        .CreateLogger();

    return services;
  }

  public static WebApplication UseApiServices(this WebApplication app)
  {
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    //app.UseAuthorization();

    app.MapCarter();

    app.UseExceptionHandler(options => { });
    app.UseHealthChecks("/health",
        new HealthCheckOptions
        {
          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

    return app;
  }
}

