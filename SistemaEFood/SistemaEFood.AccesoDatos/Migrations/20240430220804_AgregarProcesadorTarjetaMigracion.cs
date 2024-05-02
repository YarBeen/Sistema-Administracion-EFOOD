using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProcesadorTarjetaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcesadorTarjeta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcesadorId = table.Column<int>(type: "int", nullable: false),
                    TarjetaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcesadorTarjeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcesadorTarjeta_ProcesadorDePago_ProcesadorId",
                        column: x => x.ProcesadorId,
                        principalTable: "ProcesadorDePago",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProcesadorTarjeta_Tarjetas_TarjetaId",
                        column: x => x.TarjetaId,
                        principalTable: "Tarjetas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcesadorTarjeta_ProcesadorId",
                table: "ProcesadorTarjeta",
                column: "ProcesadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcesadorTarjeta_TarjetaId",
                table: "ProcesadorTarjeta",
                column: "TarjetaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcesadorTarjeta");
        }
    }
}
