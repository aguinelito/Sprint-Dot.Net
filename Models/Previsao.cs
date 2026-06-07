namespace Agrosphere.Models;

public class Previsao
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Recomendacao { get; set; } = string.Empty;
    public DateTime DataPrevisao { get; set; }
    public DateTime? DataVigenciaAte { get; set; }
    public int SensorId { get; set; }
    public Sensor? Sensor { get; set; }
}
