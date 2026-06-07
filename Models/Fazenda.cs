namespace Agrosphere.Models;

public class Fazenda
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int UsuarioId { get; set; }

    public Usuario? Usuario { get; set; }
    public ICollection<Sensor> Sensores { get; set; } = new List<Sensor>();
}
