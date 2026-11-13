using Churrascaria.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Churrascaria.Migrations;

[DbContext(typeof(ChurrascariaContext))]
[Migration("20261001013000_EventosInicial")]
public class EventosInicial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Agendamentos",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false).Annotation("Sqlite:Autoincrement", true),
                Protocolo = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                NomeCliente = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Telefone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                DataHora = table.Column<DateTime>(type: "TEXT", nullable: false),
                QuantidadePessoas = table.Column<int>(type: "INTEGER", nullable: false),
                TipoEvento = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                Cidade = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                LocalEvento = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Observacoes = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: true),
                Status = table.Column<int>(type: "INTEGER", nullable: false),
                CriadoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Agendamentos", x => x.Id));
        migrationBuilder.CreateIndex("IX_Agendamentos_DataHora", "Agendamentos", "DataHora");
        migrationBuilder.CreateIndex("IX_Agendamentos_Protocolo", "Agendamentos", "Protocolo", unique: true);
    }
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable("Agendamentos");
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.0");
        modelBuilder.Entity("Churrascaria.Models.Agendamento", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<string>("Protocolo").IsRequired().HasMaxLength(12).HasColumnType("TEXT");
            b.Property<string>("NomeCliente").IsRequired().HasMaxLength(100).HasColumnType("TEXT");
            b.Property<string>("Telefone").IsRequired().HasMaxLength(20).HasColumnType("TEXT");
            b.Property<DateTime>("DataHora").HasColumnType("TEXT");
            b.Property<int>("QuantidadePessoas").HasColumnType("INTEGER");
            b.Property<string>("TipoEvento").IsRequired().HasMaxLength(40).HasColumnType("TEXT");
            b.Property<string>("Cidade").IsRequired().HasMaxLength(100).HasColumnType("TEXT");
            b.Property<string>("LocalEvento").IsRequired().HasMaxLength(200).HasColumnType("TEXT");
            b.Property<string>("Observacoes").HasMaxLength(1500).HasColumnType("TEXT");
            b.Property<int>("Status").HasColumnType("INTEGER");
            b.Property<DateTime>("CriadoEmUtc").HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("DataHora");
            b.HasIndex("Protocolo").IsUnique();
            b.ToTable("Agendamentos");
        });
    }
}
