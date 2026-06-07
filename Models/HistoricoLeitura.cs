namespace Agrosphere.Models;

public class HistoricoLeitura
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // LEITURA, MANUTENCAO, CALIBRACAO, etc
    public int SensorId { get; set; }
    public Sensor? Sensor { get; set; }
}
