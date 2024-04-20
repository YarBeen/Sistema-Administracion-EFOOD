using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProcesadorDePagoMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcesadorDePago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Procesador = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    NombreOpcionDePago = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Verificacion = table.Column<bool>(type: "bit", nullable: false),
                    Metodo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcesadorDePago", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcesadorDePago");
        }
    }
}
