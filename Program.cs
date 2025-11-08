using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json.Serialization;
using SysTrack.Infrastructure.Contexts;
using SysTrack.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        var oracleConnectionString = builder.Configuration.GetConnectionString("Oracle")
            ?? Environment.GetEnvironmentVariable("ORACLE_CONNECTION_STRING");

        builder.Services.AddDbContext<SysTrackDbContext>(options =>
            options.UseOracle(oracleConnectionString));

        builder.Services.AddHealthChecks()
            .AddCheck("Database", () =>
            {
                try
                {
                    using var scope = builder.Services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<SysTrackDbContext>();
                    db.Database.CanConnect();
                    return HealthCheckResult.Healthy("Conectado ao banco de dados com sucesso.");
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy("Falha ao conectar ao banco de dados.", ex);
                }
            });

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        builder.Services.AddScoped<MotocicletaService>();
        builder.Services.AddSingleton<ManutencaoPredictionService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseMiddleware<SysTrack.Middleware.ApiKeyMiddleware>();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/health");

        app.Run();
    }
}
