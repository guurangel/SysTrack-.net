using SysTrack.ML;
using SysTrack.Services;
using Xunit;

namespace SysTrack.Tests.Services
{
    public class ManutencaoPredictionServiceTests
    {
        private readonly ManutencaoPredictionService _service;

        public ManutencaoPredictionServiceTests()
        {
            _service = new ManutencaoPredictionService();
        }

        [Fact]
        public void PreverManutencao_DeveRetornarResultadoValido()
        {
            // Arrange
            var input = new ManutencaoInput
            {
                Quilometragem = 12000,
                Ano = 2018,
                IdadeMoto = 2025 - 2018,
                Marca = "Honda",
                Modelo = "CG 160"
            };

            // Act
            var resultado = _service.PreverManutencao(input);

            // Assert
            Assert.NotNull(resultado);
            Assert.InRange(resultado.Probability, 0, 1); // probabilidade entre 0 e 1
        }

        [Fact]
        public void PreverManutencao_DeveIdentificarMotoComAltaQuilometragem()
        {
            // Arrange
            var input = new ManutencaoInput
            {
                Quilometragem = 80000,
                Ano = 2012,
                IdadeMoto = 2025 - 2012,
                Marca = "Yamaha",
                Modelo = "Factor"
            };

            // Act
            var resultado = _service.PreverManutencao(input);

            // Assert
            Assert.True(resultado.NecessitaManutencao || resultado.Probability > 0.5f);
        }
    }
}
