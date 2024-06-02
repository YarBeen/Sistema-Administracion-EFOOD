using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class Bitacora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bitacora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    nombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bitacora", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bitacora");
        }
    }
}
