using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using SysTrack;
using SysTrack.ML;
using Xunit;

namespace SysTrack.Tests.Integration
{
    public class MotocicletaIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public MotocicletaIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PreverManutencaoEndpoint_DeveRetornarStatus200()
        {
            // Arrange
            var input = new ManutencaoInput
            {
                Quilometragem = 15000,
                Ano = 2020,
                IdadeMoto = 5,
                Marca = "Honda",
                Modelo = "Biz 125"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/motocicleta/prever-manutencao", input);
            var resultado = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.True(response.IsSuccessStatusCode, "A resposta da API não foi sucesso (200).");
            Assert.Contains("StatusSugerido", resultado);
        }
    }
}
