using CleanArchitecture.Application;
using CleanArchitecture.Persistence;
using CleanArchitecture.Web.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddEfCore(builder.Configuration);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

var app = builder.Build();

//app.UseSerilogRequestLogging();

app.UseCustomExtensionHandler();
app.MapControllers();

app.Run();