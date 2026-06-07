namespace Agrosphere.Models;

public class Leitura
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public decimal Valor { get; set; }
    public string Unidade { get; set; } = string.Empty; // °C, %, Lux, etc
    public int SensorId { get; set; }
    public Sensor? Sensor { get; set; }
}
