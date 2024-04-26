using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class TablaProcesadorTarjeta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TarjetaProcesador",
                columns: table => new
                {
                    ProcesadoresDePagosId = table.Column<int>(type: "int", nullable: false),
                    TarjetasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarjetaProcesador", x => new { x.ProcesadoresDePagosId, x.TarjetasId });
                    table.ForeignKey(
                        name: "FK_TarjetaProcesador_ProcesadorDePago_ProcesadoresDePagosId",
                        column: x => x.ProcesadoresDePagosId,
                        principalTable: "ProcesadorDePago",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TarjetaProcesador_Tarjetas_TarjetasId",
                        column: x => x.TarjetasId,
                        principalTable: "Tarjetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TarjetaProcesador_TarjetasId",
                table: "TarjetaProcesador",
                column: "TarjetasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TarjetaProcesador");
        }
    }
}
