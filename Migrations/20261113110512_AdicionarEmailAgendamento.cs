using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Churrascaria.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarEmailAgendamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Agendamentos",
                type: "TEXT",
                maxLength: 254,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Agendamentos");
        }
    }
}
