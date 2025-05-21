using Microsoft.EntityFrameworkCore;
using SysTrack.Infrastructure.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Configurar string de conexão Oracle (via appsettings.json)
var oracleConnectionString = builder.Configuration.GetConnectionString("Oracle");

// Adicionar apenas o DbContext unificado
builder.Services.AddDbContext<SysTrackDbContext>(options =>
    options.UseOracle(oracleConnectionString));

// Adicionar suporte a controllers (MVC)
builder.Services.AddControllers();

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
