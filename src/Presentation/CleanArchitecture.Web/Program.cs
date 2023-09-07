using CleanArchitecture.Application;
using CleanArchitecture.Persistence;
using CleanArchitecture.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddEfCore(builder.Configuration);

var app = builder.Build();

app.UseCustomExceptionMiddleware();
app.MapControllers();

app.Run();