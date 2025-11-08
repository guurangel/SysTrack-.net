using Microsoft.ML;
using Microsoft.ML.Data;
using SysTrack.ML;

namespace SysTrack.Services
{
    public class ManutencaoPredictionService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _modeloTreinado;

        public ManutencaoPredictionService()
        {
            _mlContext = new MLContext();

            // 🔹 Gera dados sintéticos de treino
            var dadosTreino = GerarDadosTreino();
            var data = _mlContext.Data.LoadFromEnumerable(dadosTreino);

            // 🔹 Pipeline de pré-processamento (sem Label)
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("MarcaKey", nameof(ManutencaoInput.Marca))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("ModeloKey", nameof(ManutencaoInput.Modelo)))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("MarcaEncoded", "MarcaKey"))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("ModeloEncoded", "ModeloKey"))
                .Append(_mlContext.Transforms.Concatenate("Features",
                    nameof(ManutencaoInput.Quilometragem),
                    nameof(ManutencaoInput.Ano),
                    nameof(ManutencaoInput.IdadeMoto),
                    "MarcaEncoded",
                    "ModeloEncoded"))
                .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                // ✅ O Label é configurado diretamente no treinador
                .Append(_mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(
                    labelColumnName: nameof(ManutencaoInputTreino.NecessitaManutencao),
                    featureColumnName: "Features"));

            // 🔹 Treina o modelo
            _modeloTreinado = pipeline.Fit(data);
        }

        public ManutencaoPrediction PreverManutencao(ManutencaoInput input)
        {
            // 🔹 Cria engine de previsão (sem precisar da coluna Label)
            var engine = _mlContext.Model.CreatePredictionEngine<ManutencaoInput, ManutencaoPrediction>(_modeloTreinado);
            return engine.Predict(input);
        }

        // 🔹 Geração de dados sintéticos para o modelo
        private List<ManutencaoInputTreino> GerarDadosTreino()
        {
            var random = new Random();
            var marcas = new[] { "Honda", "Yamaha", "Suzuki", "Kawasaki", "BMW" };
            var modelos = new[] { "CG 160", "Fazer 250", "Biz 125", "Factor 150", "Yes 125", "Ninja 400", "G310R" };

            var lista = new List<ManutencaoInputTreino>();

            for (int i = 0; i < 300; i++)
            {
                var marca = marcas[random.Next(marcas.Length)];
                var modelo = modelos[random.Next(modelos.Length)];
                var ano = random.Next(2010, 2025);
                var idade = DateTime.Now.Year - ano;
                var km = random.Next(500, 80000);

                // 🔹 Lógica realista para determinar necessidade de manutenção
                bool precisaManutencao =
                    km > 30000 || idade > 7 ||
                    (marca == "Suzuki" && km > 20000) ||
                    (modelo.Contains("Biz") && km > 25000);

                lista.Add(new ManutencaoInputTreino
                {
                    Quilometragem = km,
                    Ano = ano,
                    IdadeMoto = idade,
                    Marca = marca,
                    Modelo = modelo,
                    NecessitaManutencao = precisaManutencao
                });
            }

            return lista;
        }
    }

    // 🔹 Classe auxiliar usada apenas no treino (com a coluna Label)
    public class ManutencaoInputTreino : ManutencaoInput
    {
        public bool NecessitaManutencao { get; set; }
    }
}
