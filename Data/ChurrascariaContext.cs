using Churrascaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Churrascaria.Data;

public class ChurrascariaContext : DbContext
{
    public ChurrascariaContext(DbContextOptions<ChurrascariaContext> options) : base(options) { }
    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamento>().HasIndex(x => x.Protocolo).IsUnique();
        modelBuilder.Entity<Agendamento>().HasIndex(x => x.DataHora);
    }
}
