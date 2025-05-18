using Microsoft.EntityFrameworkCore;
using SysTrack.Infrastructure.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Configurar string de conex�o Oracle (via appsettings.json)
var oracleConnectionString = builder.Configuration.GetConnectionString("Oracle");

// Adicionar DbContexts com Oracle
builder.Services.AddDbContext<PatioDbContext>(options =>
    options.UseOracle(oracleConnectionString));

builder.Services.AddDbContext<MotocicletaDbContext>(options =>
    options.UseOracle(oracleConnectionString));

// Adicionar suporte a controllers (MVC)
builder.Services.AddControllers();

// Adicionar Swagger/OpenAPI para documenta��o
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar pipeline HTTP

if (app.Environment.IsDevelopment())
{
    // Usar Swagger e UI s� no desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI();

    // P�gina de erro detalhada para desenvolvedores
    app.UseDeveloperExceptionPage();
}
else
{
    // Em produ��o, redirecionar HTTP para HTTPS
    app.UseHttpsRedirection();
}

// Middlewares padr�o
app.UseAuthorization();

// Mapear controllers para rotas
app.MapControllers();

app.Run();
