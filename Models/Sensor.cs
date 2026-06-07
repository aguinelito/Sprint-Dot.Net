namespace Agrosphere.Models;

public class Sensor
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // TEMPERATURA, UMIDADE, LUMINOSIDADE
    public string Localizacao { get; set; } = string.Empty;
    public DateTime DataInstalacao { get; set; }
    public int FazendaId { get; set; }

    public Fazenda? Fazenda { get; set; }
    public ICollection<AlertaClimatico> AlertasClimaticos { get; set; } = new List<AlertaClimatico>();
    public ICollection<Leitura> Leituras { get; set; } = new List<Leitura>();
    public ICollection<Previsao> Previsoes { get; set; } = new List<Previsao>();
    public ICollection<HistoricoLeitura> HistoricoLeituras { get; set; } = new List<HistoricoLeitura>();
}
