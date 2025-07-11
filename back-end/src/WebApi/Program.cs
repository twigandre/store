using MediatR;
using Microsoft.EntityFrameworkCore;
using Store.App.Core.Application;
using Store.App.Crosscutting.Commom.HandleError;
using Store.App.Crosscutting.Commom.Security;
using Store.App.Crosscutting.Commom.Utils;
using Store.App.Crosscutting.IoC.DependencyInjection;
using Store.App.Infrastructure.Context;

string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (env.IsNullOrEmpty())
{
    throw new Exception("enviroment not found");
}

Console.WriteLine("Run in .: " + env);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<DafaultContext>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.SetJwt();

string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

if (connectionString.IsNullOrEmpty())
{
    throw new Exception("Connection string not found");
}

builder.Services.AddDbContext<DafaultContext>(options => {
    options.UseNpgsql(connectionString);
});

builder.Services.Initialize();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(ApplicationLayer).Assembly,
        typeof(Program).Assembly
    );
});

WebApplication app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.HandlerInternalError();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();