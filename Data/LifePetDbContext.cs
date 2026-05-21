using LifePetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LifePetApi.Data;

public class LifePetDbContext : DbContext
{
    public LifePetDbContext(DbContextOptions<LifePetDbContext> options) : base(options) { }

    public DbSet<Tutor> Tutores => Set<Tutor>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Vacina> Vacinas => Set<Vacina>();
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<Medicamento> Medicamentos => Set<Medicamento>();
    public DbSet<Historico> Historicos => Set<Historico>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tutor>(e =>
        {
            e.ToTable("TUTORES");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasColumnName("EMAIL").HasMaxLength(150).IsRequired();
            e.Property(x => x.Telefone).HasColumnName("TELEFONE").HasMaxLength(20);
        });

        modelBuilder.Entity<Pet>(e =>
        {
            e.ToTable("PETS");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            e.Property(x => x.Especie).HasColumnName("ESPECIE").HasMaxLength(50);
            e.Property(x => x.Raca).HasColumnName("RACA").HasMaxLength(50);
            e.Property(x => x.DataNascimento).HasColumnName("DATA_NASCIMENTO");
            e.Property(x => x.TutorId).HasColumnName("TUTOR_ID");
            e.HasOne(x => x.Tutor).WithMany(t => t.Pets).HasForeignKey(x => x.TutorId);
        });

        modelBuilder.Entity<Vacina>(e =>
        {
            e.ToTable("VACINAS");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            e.Property(x => x.DataAplicacao).HasColumnName("DATA_APLICACAO");
            e.Property(x => x.DataProximaDose).HasColumnName("DATA_PROXIMA_DOSE");
            e.Property(x => x.PetId).HasColumnName("PET_ID");
            e.HasOne(x => x.Pet).WithMany(p => p.Vacinas).HasForeignKey(x => x.PetId);
        });

        modelBuilder.Entity<Consulta>(e =>
        {
            e.ToTable("CONSULTAS");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.DataHora).HasColumnName("DATA_HORA");
            e.Property(x => x.Veterinario).HasColumnName("VETERINARIO").HasMaxLength(100);
            e.Property(x => x.Motivo).HasColumnName("MOTIVO").HasMaxLength(200);
            e.Property(x => x.PetId).HasColumnName("PET_ID");
            e.HasOne(x => x.Pet).WithMany(p => p.Consultas).HasForeignKey(x => x.PetId);
        });

        modelBuilder.Entity<Medicamento>(e =>
        {
            e.ToTable("MEDICAMENTOS");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(100).IsRequired();
            e.Property(x => x.Dosagem).HasColumnName("DOSAGEM").HasMaxLength(50);
            e.Property(x => x.Frequencia).HasColumnName("FREQUENCIA").HasMaxLength(50);
            e.Property(x => x.DataInicio).HasColumnName("DATA_INICIO");
            e.Property(x => x.DataFim).HasColumnName("DATA_FIM");
            e.Property(x => x.PetId).HasColumnName("PET_ID");
            e.HasOne(x => x.Pet).WithMany(p => p.Medicamentos).HasForeignKey(x => x.PetId);
        });

        modelBuilder.Entity<Historico>(e =>
        {
            e.ToTable("HISTORICOS");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID").UseIdentityColumn();
            e.Property(x => x.Data).HasColumnName("DATA");
            e.Property(x => x.Descricao).HasColumnName("DESCRICAO").HasMaxLength(500);
            e.Property(x => x.Tipo).HasColumnName("TIPO").HasMaxLength(50);
            e.Property(x => x.PetId).HasColumnName("PET_ID");
            e.HasOne(x => x.Pet).WithMany(p => p.Historicos).HasForeignKey(x => x.PetId);
        });
    }
}
