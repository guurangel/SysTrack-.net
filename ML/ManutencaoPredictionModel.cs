using Microsoft.ML.Data;

namespace SysTrack.ML
{
    public class ManutencaoInput
    {
        [LoadColumn(0)] public float Quilometragem { get; set; }

        [LoadColumn(1)] public float Ano { get; set; }

        // Idade calculada (baseada no ano atual - ano da moto)
        [LoadColumn(2)] public float IdadeMoto { get; set; }

        // Marca e Modelo (opcionalmente podem ser usados como features)
        [LoadColumn(3)] public string Marca { get; set; } = string.Empty;

        [LoadColumn(4)] public string Modelo { get; set; } = string.Empty;
    }

    public class ManutencaoPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool NecessitaManutencao { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
}
