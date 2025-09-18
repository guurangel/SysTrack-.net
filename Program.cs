using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using SysTrack.Infrastructure.Contexts;
using SysTrack.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar string de conexão Oracle (via appsettings.json)
var oracleConnectionString = builder.Configuration.GetConnectionString("Oracle");

// Adicionar apenas o DbContext unificado
builder.Services.AddDbContext<SysTrackDbContext>(options =>
    options.UseOracle(oracleConnectionString));

// Configurar controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Registrar suas services
builder.Services.AddScoped<MotocicletaService>();

// Adicionar Swagger/OpenAPI para documentação
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
