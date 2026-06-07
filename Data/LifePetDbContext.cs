using Agrosphere.Models;
using Microsoft.EntityFrameworkCore;

namespace Agrosphere.Data;

public class LifePetDbContext : DbContext
{
    public LifePetDbContext(DbContextOptions<LifePetDbContext> options) : base(options) { }

    // Novos DbSets para Sistema de Monitoramento de Fazenda
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Fazenda> Fazendas => Set<Fazenda>();
    public DbSet<Sensor> Sensores => Set<Sensor>();
    public DbSet<Leitura> Leituras => Set<Leitura>();
    public DbSet<AlertaClimatico> AlertasClimaticos => Set<AlertaClimatico>();
    public DbSet<Previsao> Previsoes => Set<Previsao>();
    public DbSet<HistoricoLeitura> HistoricoLeituras => Set<HistoricoLeitura>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações para o Sistema de Monitoramento de Fazenda
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("USUARIOS_MONITORAMENTO");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasColumnName("EMAIL").HasMaxLength(150).IsRequired();
            e.Property(x => x.Telefone).HasColumnName("TELEFONE").HasMaxLength(20);
        });

        modelBuilder.Entity<Fazenda>(e =>
        {
            e.ToTable("FAZENDAS_MONITORAMENTO");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            e.Property(x => x.Localizacao).HasColumnName("LOCALIZACAO").HasMaxLength(200);
            e.Property(x => x.Descricao).HasColumnName("DESCRICAO").HasMaxLength(500);
            e.Property(x => x.UsuarioId).HasColumnName("USUARIO_ID");
            e.HasOne(x => x.Usuario).WithMany(u => u.Fazendas).HasForeignKey(x => x.UsuarioId);
        });

        modelBuilder.Entity<Sensor>(e =>
        {
            e.ToTable("SENSORES_MONITORAMENTO");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            e.Property(x => x.Tipo).HasColumnName("TIPO").HasMaxLength(50).IsRequired();
            e.Property(x => x.Localizacao).HasColumnName("LOCALIZACAO").HasMaxLength(200);
            e.Property(x => x.DataInstalacao).HasColumnName("DATA_INSTALACAO");
            e.Property(x => x.FazendaId).HasColumnName("FAZENDA_ID");
            e.HasOne(x => x.Fazenda).WithMany(f => f.Sensores).HasForeignKey(x => x.FazendaId);
        });

        modelBuilder.Entity<Leitura>(e =>
        {
            e.ToTable("LEITURAS_SENSORES");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.DataHora).HasColumnName("DATA_HORA");
            e.Property(x => x.Valor).HasColumnName("VALOR").HasPrecision(10, 2);
            e.Property(x => x.Unidade).HasColumnName("UNIDADE").HasMaxLength(10);
            e.Property(x => x.SensorId).HasColumnName("SENSOR_ID");
            e.HasOne(x => x.Sensor).WithMany(s => s.Leituras).HasForeignKey(x => x.SensorId);
        });

        modelBuilder.Entity<AlertaClimatico>(e =>
        {
            e.ToTable("ALERTAS_CLIMATICOS_SENSORES");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.Descricao).HasColumnName("DESCRICAO").HasMaxLength(500);
            e.Property(x => x.Tipo).HasColumnName("TIPO").HasMaxLength(50);
            e.Property(x => x.DataAlerta).HasColumnName("DATA_ALERTA");
            e.Property(x => x.DataResolucao).HasColumnName("DATA_RESOLUCAO");
            e.Property(x => x.SensorId).HasColumnName("SENSOR_ID");
            e.HasOne(x => x.Sensor).WithMany(s => s.AlertasClimaticos).HasForeignKey(x => x.SensorId);
        });

        modelBuilder.Entity<Previsao>(e =>
        {
            e.ToTable("PREVISOES_SENSORES");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.Descricao).HasColumnName("DESCRICAO").HasMaxLength(500);
            e.Property(x => x.Recomendacao).HasColumnName("RECOMENDACAO").HasMaxLength(500);
            e.Property(x => x.DataPrevisao).HasColumnName("DATA_PREVISAO");
            e.Property(x => x.DataVigenciaAte).HasColumnName("DATA_VIGENCIA_ATE");
            e.Property(x => x.SensorId).HasColumnName("SENSOR_ID");
            e.HasOne(x => x.Sensor).WithMany(s => s.Previsoes).HasForeignKey(x => x.SensorId);
        });

        modelBuilder.Entity<HistoricoLeitura>(e =>
        {
            e.ToTable("HISTORICO_LEITURAS_SENSORES");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.Data).HasColumnName("DATA");
            e.Property(x => x.Descricao).HasColumnName("DESCRICAO").HasMaxLength(500);
            e.Property(x => x.Tipo).HasColumnName("TIPO").HasMaxLength(50);
            e.Property(x => x.SensorId).HasColumnName("SENSOR_ID");
            e.HasOne(x => x.Sensor).WithMany(s => s.HistoricoLeituras).HasForeignKey(x => x.SensorId);
        });
    }
}
