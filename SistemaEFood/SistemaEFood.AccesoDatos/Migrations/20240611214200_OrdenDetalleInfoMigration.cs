using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class OrdenDetalleInfoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdenDetalle_Productos_ProductoId",
                table: "OrdenDetalle");

            migrationBuilder.DropIndex(
                name: "IX_OrdenDetalle_ProductoId",
                table: "OrdenDetalle");

            migrationBuilder.RenameColumn(
                name: "cliente",
                table: "Ordenes",
                newName: "Cliente");

            migrationBuilder.RenameColumn(
                name: "ProductoId",
                table: "OrdenDetalle",
                newName: "ChequeNumero");

            migrationBuilder.RenameColumn(
                name: "Precio",
                table: "OrdenDetalle",
                newName: "Monto");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "OrdenDetalle",
                newName: "ChequeCuenta");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "OrdenDetalle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Medio",
                table: "OrdenDetalle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "OrdenDetalle",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "OrdenDetalle");

            migrationBuilder.DropColumn(
                name: "Medio",
                table: "OrdenDetalle");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "OrdenDetalle");

            migrationBuilder.RenameColumn(
                name: "Cliente",
                table: "Ordenes",
                newName: "cliente");

            migrationBuilder.RenameColumn(
                name: "Monto",
                table: "OrdenDetalle",
                newName: "Precio");

            migrationBuilder.RenameColumn(
                name: "ChequeNumero",
                table: "OrdenDetalle",
                newName: "ProductoId");

            migrationBuilder.RenameColumn(
                name: "ChequeCuenta",
                table: "OrdenDetalle",
                newName: "Cantidad");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenDetalle_ProductoId",
                table: "OrdenDetalle",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenDetalle_Productos_ProductoId",
                table: "OrdenDetalle",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");
        }
    }
}
