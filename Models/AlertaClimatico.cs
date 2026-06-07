namespace Agrosphere.Models;

public class AlertaClimatico
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // TEMPERATURA_ALTA, UMIDADE_BAIXA, etc
    public DateTime DataAlerta { get; set; }
    public DateTime? DataResolucao { get; set; }
    public int SensorId { get; set; }
    public Sensor? Sensor { get; set; }
}
