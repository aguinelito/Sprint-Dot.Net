namespace Agrosphere.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public ICollection<Fazenda> Fazendas { get; set; } = new List<Fazenda>();
}
