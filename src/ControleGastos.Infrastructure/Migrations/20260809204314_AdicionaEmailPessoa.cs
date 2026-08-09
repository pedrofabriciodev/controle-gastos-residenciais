using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleGastos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaEmailPessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Pessoas",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Pessoas");
        }
    }
}
