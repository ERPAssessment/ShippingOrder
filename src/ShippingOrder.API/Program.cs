using ERP.Shared.Logging;
using Serilog;
using ShippingOrder.API;
using ShippingOrder.Application;
using ShippingOrder.Infrastructure;
using ShippingOrder.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

/// Add services to the container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();

app.UseApiServices();

if (app.Environment.IsDevelopment())
{
  await app.InitialiseDatabaseAsync();
}

app.Run();
