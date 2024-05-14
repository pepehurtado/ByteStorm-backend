using Microsoft.EntityFrameworkCore;

namespace ByteStorm.Models;

public class ByteStormContext : DbContext
{
    public ByteStormContext(DbContextOptions<ByteStormContext> options)
        : base(options)
    {
    }

    public DbSet<Equipo> EquipoItems { get; set; } = null!;
    public DbSet<Mision> MisionItems { get; set; } = null!;
    public DbSet<Operativo> OperativoItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mision>(entity =>
        {
            entity.HasOne(d => d.OpAsig)
            .WithMany(p => p.MisionAsignada)
            .HasForeignKey(d => d.OpId);
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasOne(d => d.perteneceMision)
            .WithMany(p => p.EquipoAsignado)
            .HasForeignKey(d => d.MisionId);
        });
    }

}