using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposTablaUsuarioMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreguntaSeguridad",
                table: "AspNetUsers",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RespuestaSeguridad",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PreguntaSeguridad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RespuestaSeguridad",
                table: "AspNetUsers");
        }
    }
}
